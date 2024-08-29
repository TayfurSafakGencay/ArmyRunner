using UnityEngine;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    public static LevelManager Instance;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
    }

    // TODO - Safak: Json'dan yuklenecek.
    public int GetLevel()
    {
      return 1;
    }
  }
}