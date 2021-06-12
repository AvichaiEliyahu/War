using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource musicSource;

    [SerializeField] AudioClip backgroundMusic;

    void Start()
    {
        AudioSource musicSource = GetComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
}
