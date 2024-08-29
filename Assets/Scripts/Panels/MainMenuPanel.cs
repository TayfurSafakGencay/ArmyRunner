using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class MainMenuPanel : BasePanel
  {
    [SerializeField]
    private Button _levelButton;

    [SerializeField]
    private TextMeshProUGUI _levelButtonText;

    public override void Awake()
    {
      base.Awake();
      
      ChangePanelLayer(PanelLayer.MainMenuPanel);
      
      _levelButton.onClick.AddListener(OnStartLevel);
      
      Init();
    }

    private void Init()
    {
      _levelButtonText.text = "Level " + LevelManager.Instance.GetLevel();
    }

    public void OnStartLevel()
    {
      int level = LevelManager.Instance.GetLevel();

      GameManager.Instance.StartGame(level);
    }
  }
}