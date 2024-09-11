using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class MainMenuPanel : BasePanel
  {
    [SerializeField]
    private Button _levelButton;

    public override void Awake()
    {
      base.Awake();
      
      ChangePanelLayer(PanelLayer.MainMenuPanel);
      
      _levelButton.onClick.AddListener(OnStartLevel);
    }

    public void OnStartLevel()
    {
      int level = LevelManager.Instance.GetLevel();

      GameManager.Instance.StartGame(level);
    }
  }
}