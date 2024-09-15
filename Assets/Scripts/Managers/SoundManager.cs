using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
  public class SoundManager : MonoBehaviour
  {
    [SerializeField]
    private List<SoundVo> _soundVos;

    private static readonly Dictionary<SoundKey, AudioClip> _sounds = new();

    private void Awake()
    {
      for (int i = 0; i < _soundVos.Count; i++)
      {
        SoundVo soundVo = _soundVos[i];
        _sounds.Add(soundVo.SoundKey, soundVo.AudioClip);
      }
    }
    
    public static AudioClip GetAudioClip(SoundKey soundKey)
    {
      return _sounds[soundKey];
    }
  }

  [Serializable]
  public struct SoundVo
  {
    public SoundKey SoundKey;

    public AudioClip AudioClip;
  }
  
  
  public enum SoundKey
  {
    AK47,
    AR,
    AUG,
    Sniper,
    Shotgun,
    MP5,
    FAMAS,
  }
}