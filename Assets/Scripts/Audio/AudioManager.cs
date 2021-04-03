using System;
using System.Collections;
using UnityEngine;

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

        foreach (AudioObject sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.InitAudioSource();
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

    public void Play(AudioType name, Vector3 position)
    {
        AudioObject audio = GetAudioObject(name);
        if (audio != null)
        {
            AudioSource.PlayClipAtPoint(audio.source.clip, position);
        }
    }

    private AudioObject GetAudioObject(AudioType name)
    {
        AudioObject sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Audio \"{name}\" not found.");
        }
        return sound;
    }
}
