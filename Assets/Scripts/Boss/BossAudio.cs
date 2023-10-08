using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceInternalBoss;
        [SerializeField] private AudioSource _audioSourceExternalBoss;
        
        [Header("Internal Clips")]
        [SerializeField] private AudioClip _audioClipAttack;
        [SerializeField] private AudioClip _audioClipRefilling;
        [SerializeField] private AudioClip _audioClipRoar;
        [SerializeField] private AudioClip _audioClipDie;
        
        [Header("External Clips")]
        [SerializeField] private AudioClip _audioClipReceiveDamage;
        [SerializeField] private AudioClip _audioClipReceiveHeal;

        public void PlaySound(string soundClip)
        {
            switch (soundClip)
            {
                case "attack":
                    _audioSourceInternalBoss.PlayOneShot(_audioClipAttack);
                    break;
                
                case "refilling":
                    _audioSourceInternalBoss.clip = _audioClipRefilling;
                    _audioSourceInternalBoss.Play();
                    break;
                
                case "roar":
                    _audioSourceInternalBoss.PlayOneShot(_audioClipRoar);
                    break;
                
                case "die":
                    _audioSourceInternalBoss.PlayOneShot(_audioClipDie);
                    break;
                
                
                case "damage":
                    _audioSourceInternalBoss.PlayOneShot(_audioClipReceiveDamage);
                    break;
                
                case "heal":
                    _audioSourceInternalBoss.PlayOneShot(_audioClipReceiveHeal);
                    break;
            }
        }

        public void StopSounds()
        {
            _audioSourceExternalBoss.Stop();
            _audioSourceInternalBoss.Stop();
        }
    }
}
