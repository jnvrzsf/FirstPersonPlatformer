using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioObject[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (AudioObject audio in sounds)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.InitAudioSource();
        }
    }
    
    public void Play(AudioType name)
    {
        AudioObject audio = GetAudioObject(name);
        if (audio != null)
        {
            audio.source.PlayOneShot(audio.source.clip);
        }
    }

    public void PlayOnGameObject(AudioType name, GameObject gameObject)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        AudioObject audio = GetAudioObject(name);
        if (audio != null)
        {
            newAudioSource.PlayOneShot(audio.source.clip);
        }
    }

    private AudioObject GetAudioObject(AudioType name)
    {
        AudioObject audio = Array.Find(sounds, a => a.name == name);
        if (audio == null)
        {
            Debug.LogWarning($"Audio \"{name}\" not found.");
        }
        return audio;
    }

    public AudioClip GetAudioClip(AudioType name)
    {
        AudioObject audio = Array.Find(sounds, a => a.name == name);
        if (audio == null)
        {
            Debug.LogWarning($"Audio \"{name}\" not found.");
        }
        return audio.clip;
    }
}
