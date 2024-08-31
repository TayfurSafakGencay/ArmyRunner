using Managers;
using UnityEngine;

namespace Team.Soldier
{
  public class Soldier : MonoBehaviour
  {
    public string Key { get; private set; }

    public GunVo CurrentWeapon { get; private set; }

    public void SetKey(string newKey)
    {
      Key = newKey;
    }

    public void EquipGun(GunVo gun)
    {
      CurrentWeapon = gun;
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
      transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _unitSpeed * Time.deltaTime);
      
      if (Vector3.Distance(transform.position, _targetPosition) < _acceptableDistance)
      {
        _reachedToTarget = true;
      }
    }
  }
}