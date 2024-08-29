using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
  public class PanelManager : MonoBehaviour
  {
    public static PanelManager Instance;

    private Transform _panelContainer;
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      _panelContainer = GameObject.FindWithTag("PanelCanvas").transform;
      
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState gameState)
    {
      if (gameState == GameState.MainMenu)
      {
        Addressables.InstantiateAsync(PanelKey.MainMenuPanel.ToString(), _panelContainer);
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