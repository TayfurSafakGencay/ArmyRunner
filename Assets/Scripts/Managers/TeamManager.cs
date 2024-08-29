using System;
using UnityEngine;

namespace Managers
{
  public class TeamManager : MonoBehaviour
  {
    public static TeamManager Instance;

    public Action<AnimationType> AnimationStateChange;
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
    }
  }

  public enum AnimationType
  {
    Firing,
    RunRight,
    RunLeft
  }
}