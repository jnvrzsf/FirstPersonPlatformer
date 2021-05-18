using System;
using UnityEngine;

[Serializable]
public class AudioObject
{
    [SerializeField] private AudioType name;
    [SerializeField] private AudioSourceSettings settings;
    private AudioSource source;

    public AudioType Name => name;
    public AudioClip Clip => settings.clip;
    public bool Is3DSound => settings.is3DSound;

    public AudioObject() { }

    public AudioObject(AudioObject ao, GameObject go)
    {
        name = ao.name;
        settings = ao.settings;
        AddAudioSource(go);
        InitAudioSourceFromSettings();
    }

    public void InitAudioSource(GameObject gameObject)
    {
        AddAudioSource(gameObject);
        InitAudioSourceFromSettings();
    }

    private void AddAudioSource(GameObject gameObject)
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    private void InitAudioSourceFromSettings()
    {
        if (source != null)
        {
            source.clip = settings.clip;
            source.volume = settings.volume;
            source.loop = settings.loop;
            if (settings.is3DSound)
            {
                source.spatialBlend = 1f;
            }
            if (settings.isUISound)
            {
                source.ignoreListenerPause = true;
            }
        }
    }

    public void Play()
    {
        if (source != null)
        {
            source.Play();
        }
    }

    public void PlayOneShot()
    {
        if (source != null)
        {
            source.PlayOneShot(settings.clip);
        }
    }

    public void Stop()
    {
        if (source != null)
        {
            source.Stop();
        }
    }
}
