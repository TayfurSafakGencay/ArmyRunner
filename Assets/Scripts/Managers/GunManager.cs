using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class GunManager : MonoBehaviour
  {
    public static GunManager Instance;

    public Transform ProjectileContainer;

    private Dictionary<string, GunData> _guns;
    
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
      
      GetGunDataFromJson();
    }

    private void GetGunDataFromJson()
    {
      _guns = SaveManager.LoadJson<Dictionary<string, GunData>>(JsonKey.GunData);
    }

    public GunData GetGunData(GunKey gunKey)
    {
      return _guns[gunKey.ToString()];
    }

    public async Task<GameObject> CreateProjectile(GunKey gunKey, WeaponLevel weaponLevel)
    {
      string projectileKey = gunKey.ToString() + weaponLevel;

      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync
        (projectileKey, Vector3.zero, quaternion.identity, ProjectileContainer);

      await asyncOperationHandle.Task;

      return asyncOperationHandle.Status == AsyncOperationStatus.Succeeded ? asyncOperationHandle.Result : null;
    }
  }

  public class Upgrade
  {
    public string Key { get; set; }
    
    public string Description { get; set; }
    
    public int Value { get; set; }
  }

  public class GunData
  {
    public int AttackDamage { get; set; }
    
    public float AttackSpeed { get; set; }
    
    public int BulletsInMagazine { get; set; }
    
    public float ReloadTime { get; set; }
    
    public int ProjectileCount { get; set; }
    
    public float Range { get; set; }
    
    public float BulletSpeed { get; set; }

    public WeaponLevel WeaponLevel { get; set; } = WeaponLevel.COMMON;
    
    public Dictionary<string, Upgrade> Upgrades { get; set; }
  }

  public enum WeaponLevel
  {
    COMMON,
    RARE,
    EPIC,
    LEGENDARY
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