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

      for (int i = 0; i < _enemyLevelData.Count; i++)
      {
        _enemyLevelDataDictionary.Add(i + 1, _enemyLevelData[i]);
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