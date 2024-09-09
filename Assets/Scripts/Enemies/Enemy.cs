using Managers;
using UnityEngine;

namespace Enemies
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Enemy : MonoBehaviour
  {
    [SerializeField]
    private EnemyStat _stat;

    private EnemyAnimator _enemyAnimator;
    
    private EnemyVo Stat => _stat.GetEnemyStat();

    private void Awake()
    {
      _enemyAnimator = GetComponent<EnemyAnimator>();
      
      _stat.SetInitialEnemyStat(1);
    }

    private void Death()
    {
      _enemyAnimator.SetAnimationState(EnemyAnimationState.Die);

      gameObject.GetComponent<BoxCollider>().enabled = false;
      
      EnemyManager.Instance.OnDeathEnemy(this);

      Destroy(gameObject, 4f);
    }

    public void TakeDamage(float damage)
    {
      Stat.Health -= damage;

      if (Stat.Health <= 0)
      {
        Death();
      }
    }
  }
}