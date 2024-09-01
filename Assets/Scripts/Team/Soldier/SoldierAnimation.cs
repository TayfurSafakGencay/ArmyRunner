using UnityEngine;

namespace Team.Soldier
{
  [RequireComponent(typeof(Animator))]
  public class SoldierAnimation : MonoBehaviour
  {
    private Animator _animator;

    private Transform _armyContainerTransform;

    private void Awake()
    {
      _animator = gameObject.GetComponent<Animator>();

      _armyContainerTransform = transform.parent;
      previousPosition = _armyContainerTransform.position;
    }

    private Vector3 previousPosition;

    private void FixedUpdate()
    {
      Vector3 currentPosition = transform.position;
      float direction = currentPosition.x - previousPosition.x;

      if (Mathf.Abs(direction) <= 0.01f)
      {
        AnimationStateChanged(AnimationState.Stop, 0);
      }
      else
      {
        AnimationStateChanged(direction > 0 ? AnimationState.Right : AnimationState.Left, direction);
      }

      previousPosition = currentPosition;
    }

    private AnimationState _animationState = AnimationState.Stop;
    private void AnimationStateChanged(AnimationState animationState, float direction)
    {
      if (_animationState == animationState) return;

      _animationState = animationState;

      if (animationState == AnimationState.Stop)
      {
        SetAnimationStates(0, false);
      }
      else
      {
        SetAnimationStates(direction, true);
      }
    }

    private void SetAnimationStates(float movementSide, bool isRunning)
    {
      SetAnimationRunning(isRunning);
      SetAnimationSide(movementSide);
    }
    
    private void SetAnimationRunning(bool value)
    {
      _animator.SetBool(AnimationKey.IsRunning.ToString(), value);
    }

    private void SetAnimationSide(float movement)
    {
      _animator.SetFloat(AnimationKey.Side.ToString(), movement);
    }
  }

  public enum AnimationState
  {
    Stop,
    Left,
    Right
  }
  
  public enum AnimationKey
  {
    IsRunning,
    Side
  } 
}