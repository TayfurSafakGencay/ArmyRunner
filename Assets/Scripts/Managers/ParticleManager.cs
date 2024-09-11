using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
  public class ParticleManager : MonoBehaviour
  {
    public static ParticleManager Instance;
    
    [SerializeField]
    private List<ParticleEffectVo> _particleEffectVos;
    
    private Dictionary<string, Queue<ParticleSystem>> _particleSystemPools = new();

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(Instance);
    }

    private void Start()
    {
      for (int i = 0; i < _particleEffectVos.Count; i++)
      {
        ParticleEffectVo particleEffectVo = _particleEffectVos[i];
        CreateParticlesInPool(particleEffectVo.Count, particleEffectVo.ParticleSystem, particleEffectVo.Name);
      }
    }

    public void PlayParticleEffect(Vector3 position, VFX vfx)
    {
      string particlePoolName = vfx.ToString();
      
      if (_particleSystemPools[particlePoolName].Count <= 0) return;
      ParticleSystem particleInstance = _particleSystemPools[particlePoolName].Dequeue();
      particleInstance.transform.position = position;
      particleInstance.gameObject.SetActive(true);
      particleInstance.Play();

      StartCoroutine(ReturnParticleToPool(particleInstance, _particleSystemPools[particlePoolName]));
    }

    private void CreateParticlesInPool(int count, ParticleSystem particle, VFX vfx)
    {
      string poolName = vfx.ToString();
      
      _particleSystemPools.Add(poolName, new Queue<ParticleSystem>());
      
      Transform oTransform = transform;
      Vector3 position = oTransform.position;
      
      for (int i = 0; i < count; i++)
      {
        ParticleSystem particleInstance = Instantiate(particle, position, Quaternion.identity, oTransform);
        particleInstance.gameObject.SetActive(false);
        _particleSystemPools[poolName].Enqueue(particleInstance);
      }
    }

    private static IEnumerator ReturnParticleToPool(ParticleSystem particleInstance, Queue<ParticleSystem> pool)
    {
      yield return new WaitWhile(() => particleInstance.IsAlive(true));
      particleInstance.gameObject.SetActive(false);
      pool.Enqueue(particleInstance);
    }
  }
  
  [Serializable]
  public class ParticleEffectVo
  {
    public VFX Name;

    public ParticleSystem ParticleSystem;

    public int Count;
  }
  
  public enum VFX
  {
    Shooting,
    HitZombie,
    HitSoldier,
  }
}
