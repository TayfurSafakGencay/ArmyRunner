using System.Threading.Tasks;
using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class InGamePanel : BasePanel
  {
    [Header("Preparing Part")]
    [SerializeField]
    private GameObject _preparingPart;

    [SerializeField]
    private TextMeshProUGUI _countDownText;

    private int _countDown = 3;

    [Space(20)]
    [Header("In Game Part")]
    [SerializeField]
    private GameObject _inGamePanelPart;

    [SerializeField]
    private TextMeshProUGUI _levelText;
    
    public override void Awake()
    {
      base.Awake();
      
      ChangePanelLayer(PanelLayer.InGamePanel);
      
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;

      _countDown = 3;
      OpenInGamePart(false);
      CountDown();
    }

    protected override void ChangePanelLayer(int layer)
    {
      SortingGroup.sortingOrder = layer;
    }

    private void OnGameStateChanged(GameState gameState)
    {
      switch (gameState)
      {
        case GameState.PreparingStart:
          OpenInGamePart(false);
          CountDown();
          break;
        case GameState.StartGame:
          OpenInGamePart(true);
          break;
      }
    }

    private void OpenInGamePart(bool value)
    {
      _levelText.text = "Level " + LevelManager.Instance.GetLevel();
      
      _inGamePanelPart.SetActive(value);
      _preparingPart.SetActive(!value);
    }

    private const int _sec = 1000;
    private async void CountDown()
    {
      if (_countDown < 0)
      {
        GameManager.Instance.ChangeGameState(GameState.StartGame);
        return;
      }
      
      _countDownText.text = _countDown.ToString();
      _countDown--;

      await Task.Delay(_sec);
      
      CountDown();
    }

    private void OnDestroy()
    {
      GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
  }
}