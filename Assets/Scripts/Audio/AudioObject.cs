using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioObject
{
    public AudioType name;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    [Range(0f, 1f)]
    public float volume = 1;
    [Range(-3f, 3f)]
    public float pitch = 1;
    [Range(0f, 1f)]
    public float spatialBlend;
    public bool loop;

    public void InitAudioSource()
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.spatialBlend = spatialBlend;
    }
}
