using System;
using UnityEngine;

[Serializable]
public class AudioSourceSettings
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1;
    public bool loop;
    public bool is3DSound;
    public bool isUISound;
}
