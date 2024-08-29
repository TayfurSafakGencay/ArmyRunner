using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
  public class TaskManager : MonoBehaviour
  {
    public static TaskManager Instance;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
    }

    public List<Task> LoadAssetTasks = new();
  }
}