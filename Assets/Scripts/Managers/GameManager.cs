using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools.ManagerController;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    public Action<GameState> OnGameStateChanged;

    public List<ManagerKey> InstantiatedManagers;

    private GameState _gameState = GameState.None;

    public GameState GameState
    {
      get => _gameState;
      private set
      {
        if (_gameState == value) return; 
        _gameState = value;
        OnGameStateChanged?.Invoke(value);
      }
    }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      OnGameStateChanged += LoadGameStateManagers;

      InitializeManagers();
    }
    
    public void StartGame(int level)
    {
      ChangeGameState(GameState.PreparingStart);
    }

    public void ChangeGameState(GameState gameState)
    {
      GameState = gameState;
    }

    private async void InitializeManagers()
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(ManagerKey.DataManager.ToString(), transform.parent);
      await asyncOperationHandle.Task;

      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
      {
        await LoadManagerData();
        
        ChangeGameState(GameState.LoadAssets);
      }
      else Debug.LogError("Data Manager could not loaded!");
    }

    private async void LoadGameStateManagers(GameState gameState)
    {
      if (!managerVos.ContainsKey(gameState)) return;
      
      List<ManagerKey> managerKeys = managerVos[gameState];
      
      for (int i = 0; i < managerKeys.Count; i++)
      {
        if (InstantiatedManagers.Contains(managerKeys[i]))
          continue;
        
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(managerKeys[i].ToString(), transform.parent);
        await asyncOperationHandle.Task;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
          InstantiatedManagers.Add(managerKeys[i]);

          Debug.Log($"{managerKeys[i]} successfully loaded.");
        }
        else Debug.LogError($"{managerKeys[i]} could not loaded!");
      }
    }

    private Dictionary<GameState, List<ManagerKey>> managerVos;
    
    private async Task LoadManagerData()
    {
      ManagerLoader managerLoader = await DataManager.Instance.LoadData<ManagerLoader>(DataKey.ManagerData);
      managerLoader.Init();
      managerVos = managerLoader.Managers;
    }
  }

  public enum ManagerKey
  {
    DataManager,
    TaskManager,
    SaveManager,
    LevelManager,
    PanelManager,
    ParticleManager,
    GunManager,
    ArmyManager,
    EnemyManager,
  }

  public enum GameState
  {
    None,
    LoadAssets,
    MainMenu,
    PreparingStart,
    StartGame,
    LevelCompleted,
    LevelFailed,
  }
}