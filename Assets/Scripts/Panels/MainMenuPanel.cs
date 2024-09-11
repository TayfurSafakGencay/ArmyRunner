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

    protected override void ChangePanelLayer(int layer)
    {
      SortingGroup.sortingOrder = layer;
    }

    public void OnStartLevel()
    {
      GameManager.Instance.StartGame();
    }
  }
}