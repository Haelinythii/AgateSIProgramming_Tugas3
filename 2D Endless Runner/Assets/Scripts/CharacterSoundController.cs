using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioClip jumpAudio;
    public AudioClip scoreHighlight;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpAudio);
    }

    public void PlayScoreHighlight()
    {
        audioSource.PlayOneShot(scoreHighlight);
    }
}
