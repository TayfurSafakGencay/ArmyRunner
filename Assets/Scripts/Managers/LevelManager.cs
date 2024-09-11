using UnityEngine;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    public static LevelManager Instance;

    public static int Level;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      Level = 1;
    }

    public int GetLevel()
    {
      return Level;
    }

    public void NextLevel()
    {
      Level++;
      
      GameManager.Instance.ChangeGameState(GameState.PreparingStart);
    }
    
    public void RestartLevel()
    {
      GameManager.Instance.ChangeGameState(GameState.PreparingStart);
    }
  }
}