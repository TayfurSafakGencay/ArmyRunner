using System;
using System.Collections.Generic;
using Team.Soldier;
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
  public class GunVo
  {
    public GunKey Key;

    public GameObject Model;
    
    public GameObject Projectile;

    public Transform AttachmentPoint;
    public WeaponRarity WeaponStage { get; private set; }
    
    [Header("Stats")]

    public int AttackDamage;

    public float AttackSpeed;

    public void UpgradeWeaponStage()
    {
      int nextValue = (int)WeaponStage + 1;

      if (Enum.IsDefined(typeof(WeaponRarity), nextValue))
      {
        WeaponStage = (WeaponRarity)nextValue;
      }
      else
      {
        WeaponStage = WeaponStage;
      }
    }
  }

  public enum WeaponRarity
  {
    Common,
    Rare,
    Epic,
    Legendary
  }

  public enum GunKey
  {
    Famas,
  }
}