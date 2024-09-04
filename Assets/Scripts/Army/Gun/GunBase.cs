using System.Collections.Generic;
using System.Threading.Tasks;
using Army.Soldiers;
using Managers;
using Unity.Mathematics;
using UnityEngine;

namespace Army.Gun
{
  public class GunBase : MonoBehaviour
  {
    private GunManager _gunManager;

    [SerializeField]
    private Transform _aimPoint;

    [SerializeField]
    private Transform _gunTransform;

    private GunKey _key;
    
    private GunData _data;

    private Soldier _owner;
    
    private float _fireTime;

    private Queue<GameObject> _projectiles = new();

    private void Awake()
    {
      _gunManager = GunManager.Instance;

      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
      
      SetInitialPhysicalGunFeatures();
    }

    private void SetInitialPhysicalGunFeatures()
    {
      transform.localScale = _gunTransform.localScale;
      transform.localPosition = _gunTransform.localPosition;
      transform.localRotation = _gunTransform.localRotation;
    }

    private async void CreateInitialProjectiles()
    {
      for (int i = 0; i < 10; i++)
      {
        GameObject projectile = await _gunManager.CreateProjectile(_key, _data.WeaponLevel);
        ProjectileDataSet(projectile);
      }
    }

    private bool _firePermission;
    private void OnGameStateChanged(GameState gameState)
    {
      _firePermission = gameState == GameState.StartGame;
    }

    private bool CheckGameStateIsStartGame()
    {
      return GameManager.Instance.GameState == GameState.StartGame;
    }

    public void SetStats(GunData gunData, Soldier soldier, GunKey gunKey)
    {
      _data = gunData;
      _owner = soldier;
      _key = gunKey;
      
      _fireTime = _data.AttackSpeed;
      _firePermission = CheckGameStateIsStartGame();

      CreateInitialProjectiles();
    }

    private void Update()
    {
      if (!_firePermission) return;
      
      Fire();
    }

    private async void Fire()
    {
      _fireTime -= Time.deltaTime;
      
      if (!(_fireTime <= 0)) return;
      
      _fireTime = _data.AttackSpeed;

      if (_projectiles.Count > _data.ProjectileCount)
      {
        ProjectileDequeue(_data.ProjectileCount);
      }
      else
      {
        GameObject projectile = await _gunManager.CreateProjectile(_key, _data.WeaponLevel);
        ProjectileDataSet(projectile);
        ProjectileDequeue(_data.ProjectileCount);
      }
      
      _owner.FireAnimation();
    }

    public void ProjectileDequeue(int count)
    {
      for (int i = 0; i < count; i++)
      {
        GameObject projectile = _projectiles.Dequeue();

        projectile.transform.position = _aimPoint.position;
        projectile.SetActive(true);
      }
    }

    public void ProjectileEnqueue(GameObject projectile)
    {
      _projectiles.Enqueue(projectile);

      projectile.transform.position = _aimPoint.position;
      // projectile.transform.rotation = quaternion.Euler(90, 0, 0);
      // projectile.transform.forward = _aimPoint.forward;
    }

    private void ProjectileDataSet(GameObject projectile)
    {
      ProjectileVo projectileVo = new()
      {
        Range = _data.Range,
        GunBase = this,
        AimPoint = _aimPoint,
        ProjectileSpeed = _data.BulletSpeed
      };
        
      projectile.GetComponent<Projectile>().SetData(projectileVo);
      projectile.SetActive(false);
      ProjectileEnqueue(projectile);
    }
  }
}