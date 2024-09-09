using UnityEngine;

namespace Enemies
{
  [RequireComponent(typeof(Animator))]
  public class EnemyAnimator : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = gameObject.GetComponent<Animator>();
    }
    
    public void SetAnimationState(EnemyAnimationState animationState)
    {
      _animator.SetTrigger(animationState.ToString());
    }
  }
  
  public enum EnemyAnimationState
  {
    Run,
    Attack,
    Die
  }
}