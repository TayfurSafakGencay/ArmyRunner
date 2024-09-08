using System;
using UnityEngine;

namespace Enemies
{
  [CreateAssetMenu(fileName = "EnemyData", menuName = "Tools/Enemy/Create Enemy Stat", order = 0)]
  public class EnemyData : ScriptableObject
  {
    [SerializeField]
    private EnemyVo Stat;

    public void SetInitialEnemyStat(int level)
    {
      EnemyVo enemyVo = new()
      {
        AttackDamage = Stat.AttackDamage + level * 1,
        MaxHealth = Stat.MaxHealth + level * 5,
        Health = Stat.MaxHealth + level * 5
      };

      Stat = enemyVo;
    }

    public EnemyVo GetEnemyStat()
    {
      return Stat;
    }

    public void TakeDamage(float damage)
    {
      Stat.Health -= damage;

      if (Stat.Health <= 0)
      {
        Death();
      }
    }

    public Action OnDeath;
    private void Death()
    {
      OnDeath?.Invoke();
    }
  }

  [Serializable]
  public struct EnemyVo
  {
    [HideInInspector]
    public float Health;

    public float MaxHealth;

    public int AttackDamage;
  }
}