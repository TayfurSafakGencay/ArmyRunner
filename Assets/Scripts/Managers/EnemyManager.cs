﻿using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class EnemyManager : MonoBehaviour
  {
    public static EnemyManager Instance { get; private set; }
    
    [SerializeField]
    private Transform _enemyParent;

    [SerializeField]
    private EnemyLevelManagerData EnemyLevelManagerData;

    private List<Enemy> _enemies;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    
    private void OnGameStateChanged(GameState gameState)
    {
      if (gameState == GameState.StartGame)
      {
        _enemies = new List<Enemy>();
        SpawnEnemies(LevelManager.Instance.GetLevel());
      }
      else
      {
        _enemies.Clear();
      }
    }
    
    public async void SpawnEnemies(int level)
    {
      EnemyLevelData enemyLevelData = EnemyLevelManagerData.GetLevelData(level);
      
      foreach (EnemyCountData enemyCountData in enemyLevelData.EnemyCountData)
      {
        for (int i = 0; i < enemyCountData.Count; i++)
        {
          string key = enemyCountData.Key.ToString();
          AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(key);
          await asyncOperationHandle.Task;

          if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
          {
            GameObject enemyObject = asyncOperationHandle.Result;
            enemyObject.transform.parent = _enemyParent;
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            _enemies.Add(enemy);
            SetInitialPositionOfEnemy(enemyObject);
          }
          else
          {
            Debug.LogError("Failed to instantiate enemy");
          }
        }
      }
    }

    private readonly Vector3 _maxPosition = new(4, 0,80);
    
    private readonly Vector3 _minPosition = new(-4, 0,40);
    
    private void SetInitialPositionOfEnemy(GameObject enemy)
    {
      enemy.transform.position = new Vector3(Random.Range(_minPosition.x, _maxPosition.x),
        _minPosition.y, Random.Range(_minPosition.z, _maxPosition.z));
    }

    public void OnDeathEnemy(Enemy enemy)
    {
      _enemies.Remove(enemy);

      if (_enemies.Count == 0)
      {
        GameManager.Instance.GameFinished(true);
      }
    }
  }
}