using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class EndGamePanel : BasePanel
  {
    [Header("Win Part")]
    [SerializeField]
    private GameObject _winPart;
    
    [SerializeField]
    private Button _nextLevelButton;
    
    [SerializeField]
    private TextMeshProUGUI _winLevelText;
    
    [Header("Lose Part")]
    [SerializeField]
    private GameObject _losePart;

    [SerializeField]
    private Button _retryButton;
    
    [SerializeField]
    private TextMeshProUGUI _loseLevelText;

    public override void Awake()
    {
      base.Awake();
      
      ChangePanelLayer(PanelLayer.EndGamePanel);
      ButtonSubscriptions();
      SetPanelSituation();
    }

    protected override void ChangePanelLayer(int layer)
    {
      SortingGroup.sortingOrder = layer;
    }

    private void ButtonSubscriptions()
    {
      _nextLevelButton.onClick.AddListener(OnNextLevel);
      _retryButton.onClick.AddListener(OnRetry);
    }
    
    private void OnNextLevel()
    {
      LevelManager.Instance.NextLevel();
    }
    
    private void OnRetry()
    {
      LevelManager.Instance.RestartLevel();
    }

    #region Open & Close

    private void SetPanelSituation()
    {
      switch (GameManager.Instance.GameState)
      {
        case GameState.LevelCompleted:
          LevelCompleted();
          break;
        case GameState.LevelFailed:
          LevelFailed();
          break;
      }
    }
    
    
    private void LevelCompleted()
    {
      _winLevelText.text= $"Level {LevelManager.Instance.GetLevel()}";

      SetPanelPartActive(true);
    }
    
    private void LevelFailed()
    {
      _loseLevelText.text= $"Level {LevelManager.Instance.GetLevel()}";
      
      SetPanelPartActive(false);
    }
    
    private void SetPanelPartActive(bool isSuccess)
    {
      _winPart.SetActive(isSuccess);
      _losePart.SetActive(!isSuccess);
    }
    
    #endregion
  }
}