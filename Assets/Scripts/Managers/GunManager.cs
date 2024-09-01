using System;
using Tools.Gun;
using UnityEngine;

namespace Managers
{
  public class GunManager : MonoBehaviour
  {
    public static GunManager Instance;

    private GunData _gunData;

    public Transform ProjectileContainer;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);

      InitializePanelData();
    }

    private async void InitializePanelData()
    {
      _gunData = await DataManager.Instance.LoadData<GunData>(DataKey.GunData);
      _gunData.Init();
    }

    public GunVo GetGunData(GunKey gunKey)
    {
      return _gunData.Guns[gunKey];
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