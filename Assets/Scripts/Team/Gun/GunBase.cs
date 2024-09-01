using Managers;
using UnityEngine;

namespace Team.Gun
{
  public abstract class GunBase : MonoBehaviour
  {
    public GunVo GunVo { get; set; }
    
    [SerializeField]
    private Transform _aimPoint;

    private float _fireTime;

    // private int _bulletCount = 20;

    private GunManager _gunManager;

    private void Awake()
    {
      _gunManager = GunManager.Instance;
    }

    private void Start()
    {
      // _fireTime = GunVo.AttackSpeed;
    }

    private void Update()
    {
      Fire();
    }

    private void Fire()
    {
      // _fireTime -= Time.deltaTime;
      //
      // if (!(_fireTime <= 0)) return;
      //
      // _fireTime = GunVo.AttackSpeed;
      // FireAnimation();
    }

    private void FireAnimation()
    {
      GameObject projectile = Instantiate(GunVo.Projectile, _aimPoint.position, Quaternion.identity, _gunManager.ProjectileContainer);
    }

    protected virtual void UpgradeGun()
    {
      GunVo.UpgradeWeaponStage();
    }
  }
}