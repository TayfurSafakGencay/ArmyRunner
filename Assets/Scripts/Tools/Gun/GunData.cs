using Managers;
using UnityEngine;

namespace Tools.Gun
{
  [CreateAssetMenu(fileName = "GunData", menuName = "Tools/Gun/Create Gun Stat", order = 2)]
  public class GunData : ScriptableObject
  {
    public GunStat GunStat;

    [Space(20)]
    public GameObject Projectile;
    
    [SerializeField]
    private Material _material;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    [Space(30)]
    [Header("Material")]
    public Color EmissionColor;

    public float EmissionIntensity;

    [Header("Trail Renderer")]
    public Gradient Gradient;

    public float Time;
    public GameObject InstantiateBullet()
    {
      return Instantiate(Projectile);
    }

    public Material GetMaterial()
    {
      Material newMaterial = new(_material);
      newMaterial.SetColor("_EmissionColor", EmissionColor * EmissionIntensity);
      return newMaterial;
    }

    private TrailRenderer GetTrailRenderer()
    {
      TrailRenderer trailRenderer = _trailRenderer;

      trailRenderer.colorGradient = Gradient;
      trailRenderer.time = Time;

      return trailRenderer;
    }

    public (TrailRenderer, Material) GetProjectileVisualElements()
    {
      TrailRenderer trailRenderer = GetTrailRenderer();
      Material material = GetMaterial();

      return (trailRenderer, material);
    }
  }

  public enum GunAttribute
  {
    None,
    AttackDamage,
    AttackSpeed,
    FireRate,
    ProjectileCount,
    Range,
    BulletSpeed,
    Penetrating,
    CriticalDamage,
    CriticalChance,
  }
}