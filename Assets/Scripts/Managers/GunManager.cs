using System;
using UnityEngine;

namespace Managers
{
  public class GunManager : MonoBehaviour
  {
    public static GunManager Instance;

    public Transform ProjectileContainer;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
    }
  }

  [Serializable]
  public class GunStat
  {
    public GunKey GunKey;

    public float AttackDamage;
    
    public float FireRate;
    
    public int ProjectileCount;
    
    public float Range;
    
    public float BulletSpeed;
    
    public int Penetrating;
    
    public float CriticalDamage ;
    
    public float CriticalChance ;
  }

  [Serializable]
  public enum GunKey
  {
    MP5,
    FAMAS,
    AK47,
    AR,
    AUG,
    SHOTGUN_1,
    SHOTGUN_2,
    KAR97,
    AWP,
  }
}