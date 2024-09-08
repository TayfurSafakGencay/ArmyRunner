using UnityEngine;

namespace Enemies
{
  public class Enemy : MonoBehaviour
  {
    [SerializeField]
    private EnemyData _stat;

    private void Awake()
    {
      _stat.SetInitialEnemyStat(1);
      
      _stat.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
      print("dd");
    }

    public void TakeDamage(float damage)
    {
      _stat.TakeDamage(damage);
    }
  }
}