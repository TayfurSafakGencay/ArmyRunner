using Managers;
using UnityEngine;

namespace Team
{
  [RequireComponent(typeof(Animator))]
  public class SoldierAnimation : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = gameObject.GetComponent<Animator>();
      TeamManager.Instance.AnimationStateChange += OnAnimationStateChanged;
    }

    private void OnAnimationStateChanged(AnimationType animationType)
    {
      _animator.SetTrigger(animationType.ToString());
    }
  }
}