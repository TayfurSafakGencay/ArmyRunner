using System;
using System.Collections.Generic;
using Army.Soldiers;
using Managers;
using Tools.Gun;
using UnityEngine;

namespace Army.Gun
{
  public class GunBase : MonoBehaviour
  {
    private GunManager _gunManager;
    
    private ParticleManager _particleManager;

    [SerializeField]
    private Transform _aimPoint;

    [SerializeField]
    private Transform _gunTransform;

    [SerializeField]
    private GunData _gunData;

    private Soldier _owner;
    
    private float _fireTime;

    private Queue<GameObject> _projectiles = new();
    
    private void Awake()
    {
      _gunManager = GunManager.Instance;
      _particleManager = ParticleManager.Instance;

      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
      
      SetInitialPhysicalGunFeatures();
    }

    private void SetInitialPhysicalGunFeatures()
    {
      transform.localScale = _gunTransform.localScale;
      transform.localPosition = _gunTransform.localPosition;
      transform.localRotation = _gunTransform.localRotation;
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

    public void SetStats(Soldier soldier)
    {
      _owner = soldier;
      
      _owner.OnDie += OnDeathOwner;
      _firePermission = CheckGameStateIsStartGame();

      CreateInitialProjectiles();
    }

    public GunStat GetStats()
    {
      return _gunData.GunStat;
    }

    private void CreateInitialProjectiles()
    {
      for (int i = 0; i < 10; i++)
      {
        GameObject projectile = _gunData.InstantiateBullet();
        ProjectileSetData(projectile);
      }
    }

    private void Update()
    {
      if (_isOwnerDead) return;
      if (!_firePermission) return;
      
      Fire();
    }

    private void Fire()
    {
      _fireTime -= Time.deltaTime;
      
      if (!(_fireTime <= 0)) return;
      
      _fireTime = _gunData.GunStat.FireRate;

      if (_projectiles.Count > _gunData.GunStat.ProjectileCount)
      {
        ProjectileDequeue(_gunData.GunStat.ProjectileCount);
      }
      else
      {
        GameObject projectile  = _gunData.InstantiateBullet();
        ProjectileSetData(projectile);
        ProjectileDequeue(_gunData.GunStat.ProjectileCount);
      }

      _particleManager.PlayParticleEffectWithSoundKey(_aimPoint.position, VFX.Shooting, _gunData.SoundKey);
      
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
    }

    private void ProjectileSetData(GameObject projectile)
    {
      projectile.transform.parent = _gunManager.ProjectileContainer;
      
      ProjectileVo projectileVo = new()
      {
        GunBase = this,
        AimPoint = _aimPoint,
        Range = _gunData.GunStat.Range,
        ProjectileSpeed = _gunData.GunStat.BulletSpeed,
        ProjectileMaterial = _gunData.GetProjectileVisualElements().Item2,
        TrailRenderer = _gunData.GetProjectileVisualElements().Item1
      };
        
      projectile.GetComponent<Projectile>().SetData(projectileVo);
      projectile.SetActive(false);
      ProjectileEnqueue(projectile);
    }

    public GunStat GetGunData()
    {
      return _gunData.GunStat;
    }

    private void OnDestroy()
    {
      for (int i = 0; i < _projectiles.Count; i++)
      {
        DestroyImmediate(_projectiles.Dequeue());
      }
    }

    private bool _isOwnerDead;
    private void OnDeathOwner()
    {
      _isOwnerDead = true;
    }
  }
}