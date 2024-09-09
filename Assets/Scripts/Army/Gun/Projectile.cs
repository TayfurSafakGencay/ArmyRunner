using Enemies;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Army.Gun
{
  public class Projectile : MonoBehaviour
  {
    private ParticleManager _particleManager;
    
    private Rigidbody _rb;

    private ProjectileVo _projectileVo;
    
    private bool _execute;

    private bool _isDataReady;

    private GunStat _gunStat;
    
    [SerializeField]
    private TrailRenderer _trailRenderer;

    [SerializeField]
    private MeshRenderer _projectileMeshRenderer;

    private void Awake()
    {
      _particleManager = ParticleManager.Instance;
      _rb = GetComponent<Rigidbody>();

      gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      _projectileVo.StartPosition = transform.position;
      
      gameObject.transform.position = _projectileVo.AimPoint.position;

      Vector3 forward = new(0,0,1);
      Quaternion world = Quaternion.LookRotation(-forward);
      transform.localRotation = world;
      
      _rb.velocity = forward * _projectileVo.ProjectileSpeed;

      _projectileDuration = _gunStat.Penetrating;
      _execute = true;
    }

    private void OnDisable()
    {
      _execute = false;

      if (!_isDataReady)
        return;
      
      _projectileVo.GunBase.ProjectileEnqueue(gameObject);
    }

    private void FixedUpdate()
    {
      if (!_isDataReady)
        return;
      
      if (!_execute)
        return;
      
      float distanceTravelled = Vector3.Distance(_projectileVo.StartPosition, transform.position);

      if (distanceTravelled >= _projectileVo.Range)
      {
        gameObject.SetActive(false);
      }
    }

    public void SetData(ProjectileVo projectileVo)
    {
      _projectileVo = projectileVo;
      _gunStat = _projectileVo.GunBase.GetGunData();

      SetProjectileVisualElements(projectileVo);
      
      _execute = true;
      _isDataReady = true;
    }

    private void SetProjectileVisualElements(ProjectileVo projectileVo)
    {
      _trailRenderer.colorGradient = projectileVo.TrailRenderer.colorGradient  ;
      _trailRenderer.time = projectileVo.TrailRenderer.time;
      
      _projectileMeshRenderer.material = projectileVo.ProjectileMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Enemy"))
      {
        CalculateDamage(other.gameObject.GetComponent<Enemy>());
        CalculateProjectileDuration();
        _particleManager.PlayParticleEffect(transform.position, VFX.HitEnemy);
      }
    }

    private void CalculateDamage(Enemy enemy)
    {
      float damage = _gunStat.AttackDamage;
      
      float chance = Random.Range(0f, 1f);

      if (chance <= _gunStat.CriticalChance)
      {
        damage *= _gunStat.CriticalDamage;
      }
      
      enemy.TakeDamage(damage);
    }

    private int _projectileDuration;
    private void CalculateProjectileDuration()
    {
      _projectileDuration--;

      if (_projectileDuration <= 0)
      {
        gameObject.SetActive(false);
      }
    }
  }

  public struct ProjectileVo
  {
    public float Range { get; set; }
    
    public Vector3 StartPosition { get; set; }
    
    public GunBase GunBase { get; set; }
    
    public Transform AimPoint { get; set; }
    
    public float ProjectileSpeed { get; set; }
    
    public TrailRenderer TrailRenderer { get; set; }
    
    public Material ProjectileMaterial { get; set; }
  }
}