using System;
using UnityEngine;

namespace Enemies
{
  [CreateAssetMenu(fileName = "EnemyStat", menuName = "Tools/Enemy/Create Enemy Stat", order = 0)]
  public class EnemyStat : ScriptableObject
  {
    [SerializeField]
    private EnemyVo Stat;

    public EnemyVo GetInitialEnemyStat(int level)
    {
      EnemyVo enemyVo = new()
      {
        AttackDamage = Stat.AttackDamage + level * 1,
        MaxHealth = Stat.MaxHealth + level * 5,
        Health = Stat.MaxHealth + level * 5
      };

      return enemyVo;
    }
  }

  [Serializable]
  public class EnemyVo
  {
    [HideInInspector]
    public float Health;

    public float MaxHealth;

    public int AttackDamage;
  }
}