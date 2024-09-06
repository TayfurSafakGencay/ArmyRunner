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

    private void Awake()
    {
      _soldierAnimation = GetComponent<SoldierAnimation>();
      transform.localRotation = new Quaternion(0, 0, 0, 0);

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

    private async void ChangeGun(GunKey gunKey)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = 
        Addressables.InstantiateAsync(gunKey.ToString(), Vector3.zero, Quaternion.identity, _gunContainer);
      
      for (int i = 0; i < _gunContainer.childCount; i++)
        Destroy(_gunContainer.GetChild(i));

      await asyncOperationHandle.Task;

      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        GameObject gun = asyncOperationHandle.Result;
        gun.GetComponent<GunBase>().SetStats(this);
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
      _targetPosition.y = 0.22f;

      _reachedToTarget = false;
    }

    private void FixedUpdate()
    {
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
  }
}