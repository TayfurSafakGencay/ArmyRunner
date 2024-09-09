using UnityEngine;

namespace Enemies
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Enemy : MonoBehaviour
  {
    [SerializeField]
    private EnemyData _stat;

    private EnemyAnimator _enemyAnimator;

    private void Awake()
    {
      _enemyAnimator = GetComponent<EnemyAnimator>();
      
      _stat.SetInitialEnemyStat(1);
      
      _stat.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
      _enemyAnimator.SetAnimationState(EnemyAnimationState.Die);
    }

    public void TakeDamage(float damage)
    {
      _stat.TakeDamage(damage);
    }
  }
}