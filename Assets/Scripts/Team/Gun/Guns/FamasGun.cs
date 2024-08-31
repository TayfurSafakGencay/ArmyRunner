using Managers;

namespace Team.Gun.Guns
{
  public class FamasGun : GunBase
  {
    protected override void UpgradeGun()
    {
      base.UpgradeGun();

      switch (GunVo.WeaponStage)
      {
        case WeaponRarity.Rare:
          UpgradeToRare();
          break;
        case WeaponRarity.Epic:
          UpgradeToEpic();
          break;
        case WeaponRarity.Legendary:
          break;
      }
    }

    private const int _rareUpgradeStat = 100;

    private void UpgradeToRare()
    {
      GunVo.AttackDamage += _rareUpgradeStat;
    }

    public string UpgradeRareText()
    {
      return $"Increases attack damage by {_rareUpgradeStat}";
    }

    private const float _epicUpgradeStat = 30;
    
    private void UpgradeToEpic()
    {
      GunVo.AttackSpeed -= GunVo.AttackSpeed * 30 / 100;
    }
    
    public string UpgradeEpicText()
    {
      return $"Increases attack speed by %{_epicUpgradeStat}";
    }
  }
}