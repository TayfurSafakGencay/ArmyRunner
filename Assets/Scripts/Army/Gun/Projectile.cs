using UnityEngine;

namespace Army.Gun
{
  public class Projectile : MonoBehaviour
  {
    private Rigidbody _rb;

    private ProjectileVo _projectileVo;
    
    private bool _execute;

    private bool _isDataReady;

    private void Awake()
    {
      _rb = GetComponent<Rigidbody>();
      
      gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      _projectileVo.StartPosition = transform.position;
      
      gameObject.transform.position = _projectileVo.AimPoint.position;
      
      Vector3 forward = _projectileVo.AimPoint.forward;
      Quaternion world = Quaternion.LookRotation(-forward);
      transform.localRotation = world;
      
      _rb.velocity = forward * _projectileVo.ProjectileSpeed;

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

      _execute = true;
      _isDataReady = true;
    }
  }

  public struct ProjectileVo
  {
    public float Range { get; set; }
    
    public Vector3 StartPosition { get; set; }
    
    public GunBase GunBase { get; set; }
    
    public Transform AimPoint { get; set; }
    
    public float ProjectileSpeed { get; set; }
  }
}