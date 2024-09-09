using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
  [CreateAssetMenu(fileName = "Enemy Level Data", menuName = "Tools/Enemy/Create Enemy Level Data", order = 0)]
  public class EnemyLevelManagerData : ScriptableObject
  {
    [SerializeField]
    private List<EnemyLevelData> _enemyLevelData;
    
    private Dictionary<int, EnemyLevelData> _enemyLevelDataDictionary;
    
    private void OnEnable()
    {
      _enemyLevelDataDictionary = new Dictionary<int, EnemyLevelData>();
      
      foreach (EnemyLevelData enemyLevelData in _enemyLevelData)
      {
        _enemyLevelDataDictionary.Add(enemyLevelData.Level, enemyLevelData);
      }
    }

    public EnemyLevelData GetLevelData(int level)
    {
      return _enemyLevelDataDictionary[level];
    }
  }

  [Serializable]
  public struct EnemyLevelData
  {
    public int Level;

    public List<EnemyCountData> EnemyCountData;
  }

  [Serializable]
  public struct EnemyCountData
  {
    public EnemyKey Key;
    
    public int Count;
  }

  [Serializable]
  public enum EnemyKey
  {
    StandardEnemy,
    BossEnemy,
  }
}