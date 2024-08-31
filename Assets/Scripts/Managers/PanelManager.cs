using System.Collections.Generic;
using System.Linq;
using Tools.Panel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class PanelManager : MonoBehaviour
  {
    public static PanelManager Instance;

    private Transform _panelContainer;

    private PanelData _panelData;

    private readonly Dictionary<PanelKey, AsyncOperationHandle<GameObject>> _panelHandles = new();
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
      
      InitializePanelData();
      
      _panelContainer = GameObject.FindWithTag("PanelCanvas").transform;
      
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
      GameManager.Instance.ChangeGameState(GameState.MainMenu);
    }

    private async void InitializePanelData()
    {
      _panelData = await DataManager.Instance.LoadData<PanelData>(DataKey.PanelData);
      _panelData.Init();
    }

    private void OnGameStateChanged(GameState gameState)
    {
      PanelKey panelKey;
      
      if (gameState == GameState.MainMenu)
      {
        panelKey = PanelKey.MainMenuPanel;
      }
      else
      {
        RemoveUnnecessaryPanels(gameState);

        return;
      }
      
      RemoveUnnecessaryPanels(gameState);
      CreatePanel(panelKey);
    }

    private async void CreatePanel(PanelKey panelKey)
    {
      if (!_panelHandles.ContainsKey(panelKey))
      {
        AsyncOperationHandle<GameObject> panelHandle = Addressables.InstantiateAsync(panelKey.ToString(), _panelContainer);
        await panelHandle.Task;

        if (panelHandle.Status == AsyncOperationStatus.Succeeded) 
        {
          _panelHandles.Add(panelKey, panelHandle);
        }
        else
        {
          Debug.LogWarning("Panel could not created!");
        }
      }
      else
      {
        Debug.LogWarning("The panel already in the list!");
      }
    }

    private void RemoveUnnecessaryPanels(GameState gameState)
    {
      for (int i = 0; i < _panelHandles.Count; i++)
      {
        KeyValuePair<PanelKey, AsyncOperationHandle<GameObject>> panelHandle = _panelHandles.ElementAt(i);

        List<GameState> panelGameStateList = _panelData.Panels[panelHandle.Key];

        if (!panelGameStateList.Contains(gameState))
        {
          Addressables.Release(panelHandle.Value);
        }
      }
    }
  }

  public enum PanelKey
  {
    MainMenuPanel
  }

  public struct PanelLayer
  {
    public const int MainMenuPanel = 10;
  }
}