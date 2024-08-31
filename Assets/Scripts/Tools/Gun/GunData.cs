using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tools.Gun
{
  [CreateAssetMenu(fileName = "GunData", menuName = "Tools/Gun/Create Gun Data", order = 0)]
  public class GunData : ScriptableObject
  {
    public List<GunVo> GunVos;
  }
}