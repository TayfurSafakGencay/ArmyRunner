using System.Collections.Generic;
using System.Threading.Tasks;
using Army.Soldiers;
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

    private EnemyVo _enemyVo;

    private Transform _target;
    
    private Soldier _targetSoldier;

    private const float _stopDistance = 10f;

    private const float _checkInterval = 1.0f;

    private float _checkTimer;

    private void Awake()
    {
      _enemyAnimator = GetComponent<EnemyAnimator>();

      _enemyVo = _stat.GetInitialEnemyStat(LevelManager.Instance.GetLevel());

      _target = ArmyManager.Instance.GetArmyContainer();
    }

    private void Update()
    {
      if (_isDead) return;
      if (!_target) return;
      
      float distance = Vector3.Distance(transform.position, _target.position);

      if (distance > _stopDistance)
      {
        transform.forward = (_target.position - transform.position).normalized;
      }
      else
      {
        if (distance <= 1.25f)
        {
          Attack();
        }
        else
        {
          if (_isAttacking) return;
          
          _enemyAnimator.SetAnimationState(EnemyAnimationState.Run);
        }

        _checkTimer += Time.deltaTime;
        if (!(_checkTimer >= _checkInterval)) return;

        _checkTimer = 0f;
        _target = GetClosestSoldier();
        
        if (!_target) return;
        transform.forward = (_target.position - transform.position).normalized;
      }
    }

    private Transform GetClosestSoldier()
    {
      List<Soldier> soldiers = ArmyManager.Instance.GetSoldiers();
      Transform closestSoldier = null;
      float closestDistanceSqr = Mathf.Infinity;
      Vector3 currentEnemyPosition = transform.transform.position;

      foreach (Soldier soldier in soldiers)
      {
        Vector3 directionToTarget = soldier.transform.position - currentEnemyPosition;
        float dSqrToTarget = directionToTarget.sqrMagnitude;

        if (!(dSqrToTarget < closestDistanceSqr)) continue;

        closestDistanceSqr = dSqrToTarget;
        closestSoldier = soldier.transform;
        _targetSoldier = soldier;
      }

      return closestSoldier;
    }

    private bool _isAttacking;
    private async void Attack()
    {
      if (_isAttacking) return;
      
      _isAttacking = true;
      _enemyAnimator.ResetAnimationState(EnemyAnimationState.Run);
      _enemyAnimator.SetAnimationState(EnemyAnimationState.Attack);
      
      float time = _enemyAnimator.GetCurrentAnimationStateLength();
      int attackAnimationTime = (int)(time * 1000);
      await Task.Delay(attackAnimationTime);
      
      _targetSoldier.TakeDamage(_enemyVo.AttackDamage);
      
      ParticleManager.Instance.PlayParticleEffect(_targetSoldier.transform.position, VFX.HitSoldier);

      _isAttacking = false;
    }

    private bool _isDead;

    private void Death()
    {
      _enemyAnimator.SetAnimationState(EnemyAnimationState.Die);

      gameObject.GetComponent<BoxCollider>().enabled = false;
      _isDead = true;

      EnemyManager.Instance.OnDeathEnemy(this);

      Destroy(gameObject, 4f);
    }

    public void TakeDamage(float damage)
    {
      _enemyVo.Health -= damage;

      if (_enemyVo.Health <= 0)
      {
        Death();
      }
    }
  }
}