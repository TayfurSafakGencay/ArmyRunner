using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tools.Panel
{
  [CreateAssetMenu(fileName = "Panel Data", menuName = "Tools/Panel/Create Panel Data", order = 0)]
  public class PanelData : ScriptableObject
  {
    [SerializeField]
    private List<PanelVO> PanelsData;

    public readonly Dictionary<PanelKey, List<GameState>> Panels = new();

    public void Init()
    {
      for (int i = 0; i < PanelsData.Count; i++)
      {
        PanelVO panelVo = PanelsData[i];
        if (!Panels.ContainsKey(panelVo.PanelKey))
        {
          Panels.Add(panelVo.PanelKey, panelVo.GameStates);
        }
      }
    }
  }

  [Serializable]
  public struct PanelVO
  {
    public PanelKey PanelKey;

    public List<GameState> GameStates;
  }
}