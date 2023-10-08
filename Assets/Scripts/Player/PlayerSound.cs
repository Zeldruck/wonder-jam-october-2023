using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] List<AudioClip> footstepClips;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip slideClip;
    [SerializeField] AudioClip HurtClip;
    [SerializeField] AudioClip DeathClip;
    [Header("Voice")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] List<AudioClip> voiceJumpClips;
    [SerializeField] List<AudioClip> voiceHurtClips;
    [SerializeField] List<AudioClip> voiceDieClips;
    [SerializeField] List<AudioClip> voiceDashClips;
    [SerializeField] List<AudioClip> voiceLoseClips;
    [SerializeField] List<AudioClip> voiceWinClips;
    private float footstepTimer;
    private bool isPlayingFootsteps = false;
    private bool isPlaying;

    private void Update()
    {
        if (isPlayingFootsteps)
        {
            if (footstepTimer > 0)
                footstepTimer -= Time.deltaTime;
            else
                PlayRandomFootstepClip();
        }
    }

    #region SFX
    public void PlayFootsteps()
    {
        isPlayingFootsteps = true;
    }

    public void StopFootsteps()
    {
        isPlayingFootsteps = false;
    }

    public void PlayRandomFootstepClip()
    {
        if (footstepClips.Count <= 0)
            return;
        int clip = Random.Range(0, footstepClips.Count);
        sfxSource.PlayOneShot(footstepClips[clip]);
        footstepTimer = footstepClips[clip].length * 0.5f;
    }

    public void PlayJump()
    {
        sfxSource.PlayOneShot(jumpClip);
        PlayRandomVoiceJumpClip();
    }
    public void PlayDash()
    {
        sfxSource.PlayOneShot(dashClip);
        PlayRandomVoiceDashClip();
    }
    public void PlaySlide()
    {
        float startTime = Random.Range(0, slideClip.length);
        sfxSource.clip = slideClip;
        sfxSource.Play();
        sfxSource.time = startTime;
        sfxSource.loop = true;
        isPlaying = true;
    }
    /// <summary>
    /// only works if it is a loop clip
    /// </summary>
    public void Resume()
    {
        if (!isPlaying)
        {
            sfxSource.Play();
            isPlaying = true;
        }
    }

    public void Pause()
    {
        if (isPlaying)
        {
            isPlaying = false;
            sfxSource.Pause();
        }
    }

    public void StopSlide()
    {
        sfxSource.Stop();
        sfxSource.clip = null;
        sfxSource.loop = false;
        isPlaying = false;
    }
    #endregion
    #region VoiceOnly
    public void PlayRandomVoiceJumpClip()
    {
        if (voiceJumpClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceJumpClips.Count);
        voiceSource.PlayOneShot(voiceJumpClips[clip]);
        footstepTimer = voiceJumpClips[clip].length * 0.5f;
    }

    public void PlayRandomVoiceDashClip()
    {
        if (voiceDashClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceDashClips.Count);
        voiceSource.PlayOneShot(voiceDashClips[clip]);
        footstepTimer = voiceDashClips[clip].length * 0.5f;
    }

    public void PlayRandomVoiceLoseMinigameClip()
    {
        if (voiceLoseClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceLoseClips.Count);
        voiceSource.PlayOneShot(voiceLoseClips[clip]);
        footstepTimer = voiceLoseClips[clip].length * 0.5f;
    }
    public void PlayRandomVoiceWinMinigameClip()
    {
        if (voiceWinClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceWinClips.Count);
        voiceSource.PlayOneShot(voiceWinClips[clip]);
        footstepTimer = voiceWinClips[clip].length * 0.5f;
    }
    public void PlayRandomVoiceHurtClip()
    {
        if (voiceHurtClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceHurtClips.Count);
        voiceSource.PlayOneShot(voiceHurtClips[clip]);
        footstepTimer = voiceHurtClips[clip].length * 0.5f;
    }
    public void PlayRandomVoiceDeathClip()
    {
        if (voiceDieClips.Count <= 0)
            return;
        int clip = Random.Range(0, voiceDieClips.Count);
        voiceSource.PlayOneShot(voiceDieClips[clip]);
        footstepTimer = voiceDieClips[clip].length * 0.5f;
    }
    #endregion
}
