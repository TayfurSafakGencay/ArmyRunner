using System;
using System.Data;
using Army.Gun;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Army.Soldiers
{
  public class Soldier : MonoBehaviour
  {
    private ArmyManager _armyManager;

    [SerializeField]
    private Transform _gunContainer;

    public string Key { get; private set; }

    private GunStat _gunStat;

    private SoldierStat _soldierStat;

    private void Awake()
    {
      _soldierAnimation = GetComponent<SoldierAnimation>();
      transform.localRotation = Quaternion.Euler(0, 48.5f, 0);

      _soldierStat = new SoldierStat(100);

      _armyManager = ArmyManager.Instance;
    }

    public void SetKey(string newKey)
    {
      Key = newKey;
    }

    public void EquipGun(GunKey gunKey)
    {
      ChangeGun(gunKey);
    }

    public GunKey GetGunKey()
    {
      return _gunStat.GunKey;
    }

    private async void ChangeGun(GunKey gunKey)
    {
      for (int i = 0; i < _gunContainer.childCount; i++)
        Destroy(_gunContainer.GetChild(i).gameObject);
      
      AsyncOperationHandle<GameObject> asyncOperationHandle =
        Addressables.InstantiateAsync(gunKey.ToString(), Vector3.zero, Quaternion.identity, _gunContainer);
      await asyncOperationHandle.Task;

      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        GameObject gun = asyncOperationHandle.Result;
        GunBase gunBase = gun.GetComponent<GunBase>();
        gunBase.SetStats(this);
        _gunStat = gunBase.GetStats();
      }
      else
      {
        throw new DataException();
      }
    }

    private bool _reachedToTarget = true;

    private Vector3 _targetPosition;

    private const float _unitSpeed = 2f;

    public void ChangeTargetPosition(Vector3 pos)
    {
      _targetPosition = pos;

      _reachedToTarget = false;
    }

    private void FixedUpdate()
    {
      if (_isDead) return;

      MoveToTargetPosition();
    }

    private const float _acceptableDistance = 0.01f;

    public void MoveToTargetPosition()
    {
      if (_reachedToTarget)
        return;

      transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _unitSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, _targetPosition) < _acceptableDistance)
      {
        _reachedToTarget = true;
      }
    }

    private SoldierAnimation _soldierAnimation;

    public void FireAnimation()
    {
      _soldierAnimation.FireAnimation();
    }

    public void TakeDamage(float damage)
    {
      _soldierStat.Health -= damage;

      if (_soldierStat.Health <= 0)
      {
        Die();
      }
    }

    public Action OnDie;

    private bool _isDead;

    private void Die()
    {
      if (_isDead) return;

      _armyManager.RemoveSoldier(this);

      _isDead = true;
      OnDie?.Invoke();

      _soldierAnimation.DieAnimation();
    }
  }

  public struct SoldierStat
  {
    public float MaxHealth;

    public float Health;

    public SoldierStat(float maxHealth)
    {
      MaxHealth = maxHealth;
      Health = maxHealth;
    }
  }
}