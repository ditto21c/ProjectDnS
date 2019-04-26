using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundType
{
    BGM = 0,
    Item,
    Motion,
    UI,
}

public class CSoundMgr : MonoBehaviour
{
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DefaultBGMName = audioSource.clip.name;
    }

    public void PlaySound(string Name, ESoundType SoundType)
    {
        AudioClip LoadedClip = CResourceMgr.LoadSound(Name, SoundType);

        if (SoundType == ESoundType.BGM)
        {
            audioSource.clip = LoadedClip;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(LoadedClip);
        }
    }

    public void RestoreDefaltBGM()
    {
        AudioClip LoadedClip = CResourceMgr.LoadSound(DefaultBGMName, ESoundType.BGM);
        audioSource.clip = LoadedClip;
        audioSource.Play();
    }

    AudioSource audioSource;
    public string DefaultBGMName;
    
}
