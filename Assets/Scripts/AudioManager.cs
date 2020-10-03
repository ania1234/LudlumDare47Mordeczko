using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    public List<AudioClip> clips;
    private int currentClipIndex;
    public float minVolume = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentClipIndex = Random.Range(0, clips.Count);
        audioSource.clip = clips[currentClipIndex];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            ChangeTrack();
        }

        if (GameManager.instance != null && GameManager.instance.levelEnd != null)
        {
            float levelProgress = (Player.instance.transform.position.x - GameManager.instance.levelEnd.teleportPoint.position.x) / (GameManager.instance.levelEnd.transform.position.x - GameManager.instance.levelEnd.teleportPoint.position.x);
            audioSource.volume = Mathf.Lerp(minVolume, 1, (GameManager.instance.levelNumber + levelProgress) / GameManager.instance.maxLevelNumber);
        }
    }

    public void ChangeTrack()
    {
        currentClipIndex = (currentClipIndex + 1) % clips.Count;
        audioSource.clip = clips[currentClipIndex];
        audioSource.Play();
    }
}
