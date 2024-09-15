using Managers;
using UnityEngine;

namespace Tools.Sound
{
  [RequireComponent(typeof(AudioSource))]
  public class ParticleSoundBehaviour : MonoBehaviour
  {
    private AudioSource _audioSource;
    
    private bool _readyToPlay;

    private void Awake()
    {
      _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
      if (!_readyToPlay) return;
      
      _audioSource.Play();
    }

    private void OnDisable()
    {
      _audioSource.Stop();
    }

    public void SetSoundKey(SoundKey soundKey)
    {
      _audioSource.clip = SoundManager.GetAudioClip(soundKey);
      _readyToPlay = true;
    }
  }
}