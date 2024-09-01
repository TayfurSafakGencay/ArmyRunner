using Managers;
using UnityEngine;

namespace Team.Soldier
{
  [RequireComponent(typeof(Animator))]
  public class SoldierAnimation : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = gameObject.GetComponent<Animator>();
      
      ArmyManager.Instance.AnimationStateChange += OnAnimationStateChanged;
    }

    private void OnAnimationStateChanged(AnimationKey animationKey, bool value)
    {
      _animator.SetBool(animationKey.ToString(), value);
    }
  }
  
  public enum AnimationKey
  {
    IsRunning,
    IsRunningLeft,
    IsRunningRight,
  } 
}