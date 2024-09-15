using System;
using DG.Tweening;
using Interfaces;
using Managers;
using TMPro;
using UnityEngine;

namespace Rewards
{
  public class RewardBehaviour : MonoBehaviour, IDamageable
  {
    [SerializeField]
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private RewardVo _rewardVo;

    private void Awake()
    {
      _rewardVo.Health *= LevelManager.Instance.GetLevel();
      SetHealthText();
    }

    private Vector3 _speed => _rewardVo.Speed * Vector3.back;
    private void FixedUpdate()
    {
      transform.position += _speed * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
      transform.DOShakeScale(0.1f, new Vector3(0.05f, 0.05f, 0), 1, 90, true).OnComplete(() =>
      {
        // Ensure it returns to original scale after the shake.
        transform.localScale = Vector3.one;
      });
        
      _rewardVo.Health -= damage;
      SetHealthText();

      if (_rewardVo.Health <= 0)
      {
        OpenReward();
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("End"))
      {
        Destroy(gameObject);
      }
    }

    private bool _isOpened;
    private void OpenReward()
    {
      if (_isOpened) return;
      _isOpened = true;
      
      if (_rewardVo.RewardKey == RewardKey.Soldier)
      {
        for (int i = 0; i < _rewardVo.count; i++)
        {
          ArmyManager.Instance.CreateSoldier();
        }
      }
      else if (_rewardVo.RewardKey == RewardKey.Gun) 
      {
        ArmyManager.Instance.EquipGunToNextSoldier(_rewardVo.GunKey);
      }

      Destroy(gameObject);
    }

    private void SetHealthText()
    {
      if (_rewardVo.Health > 1000)
      {
        float divided = _rewardVo.Health / 1000f;
        string text = Math.Round(divided, 1).ToString("0.0") + "k";
        _healthText.text = text;
        return;
      }

      _healthText.text = _rewardVo.Health.ToString("f0");
    }
  }

  [Serializable]
  public class RewardVo
  {
    public GunKey GunKey;
    
    public RewardKey RewardKey;

    public float Health;

    public float Speed = 4.5f;

    public int count;
  }

  [Serializable]
  public enum RewardKey
  {
    Gun,
    Soldier,
  }
}