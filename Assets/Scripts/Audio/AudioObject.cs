using System;
using UnityEngine;

[Serializable]
public class AudioObject
{
    public AudioType name;
    public AudioSourceSettings settings;
    [HideInInspector] public AudioSource source;

    public void InitAudioSource(GameObject gameObject)
    {
        AddAudioSource(gameObject);
        InitAudioSourceFromSettings();
    }

    public AudioObject(AudioObject ao, GameObject go)
    {
        name = ao.name;
        settings = ao.settings;
        AddAudioSource(go);
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
}
