using System;
using System.Collections.Generic;
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
    [SerializeField]
    private List<GunLevelUpgradeVo> _upgrades;

    public GameObject InstantiateBullet()
    {
      return Instantiate(Projectile);
    }

    public Material GetMaterial(float emissionIntensity, Color emissionColor)
    {
      Material newMaterial = new(_material);
      newMaterial.SetColor("_EmissionColor", emissionColor * emissionIntensity);
      return newMaterial;
    }

    private TrailRenderer GetTrailRenderer(Gradient gradient, float time)
    {
      TrailRenderer trailRenderer = _trailRenderer;

      trailRenderer.colorGradient = gradient;
      trailRenderer.time = time;

      return trailRenderer;
    }

    public (TrailRenderer, Material) GetProjectileVisualElements()
    {
      for (int i = 0; i < _upgrades.Count; i++)
      {
        GunLevelUpgradeVo gunLevelUpgradeVo = _upgrades[i];
        if (gunLevelUpgradeVo.GunLevel != GunStat.gunLevel) continue;
        
        TrailRenderer trailRenderer = GetTrailRenderer(gunLevelUpgradeVo.Gradient, gunLevelUpgradeVo.Time);
        Material material = GetMaterial(gunLevelUpgradeVo.EmissionIntensity, gunLevelUpgradeVo.EmissionColor);
        
        return (trailRenderer, material);
      }

      return (null, null);
    }

    #region Upgrade

    public void UpgradeToSpecificGunLevel(GunLevel targetGunLevel)
    {
      while (GunStat.gunLevel != targetGunLevel)
      {
        UpgradeNextLevel();
      }
    }

    public void UpgradeNextLevel()
    {
      GunLevel nextLevel = (GunLevel)(((int)GunStat.gunLevel + 1) % Enum.GetValues(typeof(GunLevel)).Length);
      GunStat.gunLevel = nextLevel;

      GunLevelUpgradeVo gunLevelUpgradeVo = new();

      for (int i = 0; i < _upgrades.Count; i++)
      {
        if (nextLevel != _upgrades[i].GunLevel) continue;
        gunLevelUpgradeVo = _upgrades[i];
        break;
      }

      if (gunLevelUpgradeVo.GunAttribute == GunAttribute.None)
      {
        return;
      }

      switch (gunLevelUpgradeVo.GunAttribute)
      {
        case GunAttribute.AttackDamage:
          break;
        case GunAttribute.AttackSpeed:
          UpgradeAttackSpeed(gunLevelUpgradeVo.Value);
          break;
        case GunAttribute.FireRate:
          break;
        case GunAttribute.BulletsInMagazine:
          break;
        case GunAttribute.ReloadTime:
          UpgradeReloadTime(gunLevelUpgradeVo.Value);
          break;
        case GunAttribute.ProjectileCount:
          break;
        case GunAttribute.Range:
          UpgradeRange(gunLevelUpgradeVo.Value);
          break;
        case GunAttribute.BulletSpeed:
          break;
        case GunAttribute.Penetrating:
          break;
        case GunAttribute.CriticalDamage:
          break;
        case GunAttribute.CriticalChance:
          break;
        case GunAttribute.Accuracy:
          break;
      }
    }

    private void UpgradeReloadTime(float value)
    {
      GunStat.ReloadTime *= 1 - value;
    }

    private void UpgradeAttackSpeed(float value)
    {
      GunStat.FireRate *= 1 - value;
    }

    private void UpgradeRange(float value)
    {
      GunStat.Range += value;
    }

    #endregion
  }

  [Serializable]
  public struct GunLevelUpgradeVo
  {
    public GunLevel GunLevel;

    [Header("Material")]
    public Color EmissionColor;

    public float EmissionIntensity;

    [Header("Trail Renderer")]
    public Gradient Gradient;

    public float Time;

    [Header("Upgrade")]
    public GunAttribute GunAttribute;

    public float Value;
  }

  public enum GunAttribute
  {
    None,
    AttackDamage,
    AttackSpeed,
    FireRate,
    BulletsInMagazine,
    ReloadTime,
    ProjectileCount,
    Range,
    BulletSpeed,
    Penetrating,
    CriticalDamage,
    CriticalChance,
    Accuracy,
  }
}