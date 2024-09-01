using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tools.Gun
{
  [CreateAssetMenu(fileName = "GunData", menuName = "Tools/Gun/Create Gun Data", order = 0)]
  public class GunData : ScriptableObject
  {
    [SerializeField]
    private List<GunVo> GunVos;

    public readonly Dictionary<GunKey, GunVo> Guns = new();
    
    public void Init()
    {
      for (int i = 0; i < GunVos.Count; i++)
      {
        GunVo gunVo = GunVos[i];

        Guns.TryAdd(gunVo.Key, gunVo);
      }
    }
  }
}