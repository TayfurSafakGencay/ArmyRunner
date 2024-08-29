using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tools.ManagerController
{
  [CreateAssetMenu(fileName = "ManagerData", menuName = "Tools/Manager/Create Manager Data", order = 0)]
  public class ManagerLoader : ScriptableObject
  {
    [SerializeField]
    private List<ManagerVo> ManagersData;

    public readonly Dictionary<GameState, List<ManagerKey>> Managers = new();

    public void Init()
    {
      for (int i = 0; i < ManagersData.Count; i++)
      {
        ManagerVo managerVo = ManagersData[i];

        if (Managers.TryGetValue(managerVo.GameState, out List<ManagerKey> manager))
        {
          manager.Add(managerVo.ManagerKey);
        }
        else
        {
          List<ManagerKey> newList = new() { managerVo.ManagerKey };
          Managers.Add(managerVo.GameState, newList);
        }
      }
    }
  }

  [Serializable]
  public struct ManagerVo
  {
    public ManagerKey ManagerKey;

    public GameState GameState;
  }
}