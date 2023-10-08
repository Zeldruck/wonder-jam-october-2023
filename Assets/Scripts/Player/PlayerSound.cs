using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] List<AudioClip> footstepClips;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip SlideClip;
    [SerializeField] AudioClip HurtClip;
    [SerializeField] AudioClip DeathClip;
    private AudioSource audioSource;
    private float footstepTimer;
    private bool isPlayingFootsteps = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        audioSource.PlayOneShot(footstepClips[clip]);
        footstepTimer = footstepClips[clip].length * 0.5f;
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }
}
