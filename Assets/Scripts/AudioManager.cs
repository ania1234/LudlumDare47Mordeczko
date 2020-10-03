using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    public List<AudioClip> clips;
    private int currentClipIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void ChangeTrack()
    {
        currentClipIndex = (currentClipIndex + 1) % clips.Count;
        audioSource.clip = clips[currentClipIndex];
        audioSource.Play();
    }
}
