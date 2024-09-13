using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
  public class RewardManager : MonoBehaviour
  {
    public static RewardManager Instance;
    
    [SerializeField]
    private GameObject _rewardPrefab;

    private int _wave;
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
        SetWave();
        SpawnReward();
      }
    }

    private readonly Vector3 _maxPosition = new(0, 0,85);

    private void SetWave()
    {
      _wave = LevelManager.Instance.GetLevel();
    }

    private const int _waveTime = 10000;
    
    private async void SpawnReward()
    {
      if (_wave <= 0) return;
      
      GameObject rewardObject = Instantiate(_rewardPrefab, transform);
      rewardObject.transform.position = _maxPosition;
      _wave--;

      await Task.Delay(_waveTime);
      
      SpawnReward();
    }
  }
}