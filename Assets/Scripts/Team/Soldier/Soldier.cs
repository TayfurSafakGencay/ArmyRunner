using Managers;
using UnityEngine;

namespace Team.Soldier
{
  public class Soldier : MonoBehaviour
  {
    private ArmyManager _armyManager;

    [SerializeField]
    private Transform _gunContainer;
    public string Key { get; private set; }

    public GunVo CurrentWeapon { get; private set; }

    private void Awake()
    {
      transform.localRotation = new Quaternion(0, 0, 0, 0);

      _armyManager = ArmyManager.Instance;
    }

    public void SetKey(string newKey)
    {
      Key = newKey;
    }

    public void EquipGun(GunVo gun)
    {
      CurrentWeapon = gun;
      
      ChangeGun(gun.Model, gun.AttachmentPoint);
    }

    private void ChangeGun(GameObject gunModel, Transform gunTransform)
    {
      for (int i = 0; i < _gunContainer.childCount; i++)
      {
        Destroy(_gunContainer.GetChild(i));
      }

      GameObject gun = Instantiate(gunModel, Vector3.zero, Quaternion.identity, _gunContainer);

      gun.transform.localScale = gunTransform.localScale;
      gun.transform.localPosition = gunTransform.localPosition;
      gun.transform.localRotation = gunTransform.localRotation;
    }

    public void Death()
    {
      ArmyManager.Instance.RemoveSoldier(this);
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
      if (!_reachedToTarget)
      {
        MoveToTargetPosition();
      }
    }

    private const float _acceptableDistance = 0.01f;
    public void MoveToTargetPosition()
    {
      _armyManager.AnimationStateChange?.Invoke(AnimationKey.IsRunning, true);
      _armyManager.AnimationStateChange?.Invoke(AnimationKey.IsRunningRight, true);

      transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _unitSpeed * Time.deltaTime);
      
      if (Vector3.Distance(transform.position, _targetPosition) < _acceptableDistance)
      {
        _reachedToTarget = true;
        _armyManager.AnimationStateChange?.Invoke(AnimationKey.IsRunning, false);
        _armyManager.AnimationStateChange?.Invoke(AnimationKey.IsRunningRight, false);
      }
    }
  }
}