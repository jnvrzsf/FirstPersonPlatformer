using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    [SerializeField] private AudioObject[] sounds; // for serialization
    private Dictionary<AudioType, AudioObject> audioObjects; // for faster lookup
    private TimeManager timeManager;

    [SerializeField] private AudioClip[] footstepClips;
    private AudioSource footstepsSource;
    private int lastPlayedIndex;
    private float lastPlayedTime;
    private const float footstepsVolume = 0.8f;
    private const float delayBetweenSteps = 0.4f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitDictionary();

        footstepsSource = gameObject.AddComponent<AudioSource>();
        footstepsSource.volume = footstepsVolume;
    }

    private void InitDictionary()
    {
        audioObjects = new Dictionary<AudioType, AudioObject>();
        foreach (AudioObject audio in sounds)
        {
            if (!audio.Is3DSound)
            {
                audio.InitAudioSource(gameObject);
            }
            audioObjects.Add(audio.Name, audio);
        }
    }

    private bool TryGetAudioObject(AudioType audioName, out AudioObject audioObject)
    {
        AudioObject existingAudio;
        audioObject = null;
        if (audioObjects.TryGetValue(audioName, out existingAudio))
        {
            audioObject = existingAudio;
            return true;
        }
        else
        {
            Debug.LogWarning($"Audio \"{audioName}\" not found.");
            return false;
        }
    }

    /// <summary>
    /// For audio that won't be interrupted,
    /// for looping audio,
    /// for audio that can be stopped.
    /// <summary>
    /// <param name="audioName"></param>
    public void Play(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio)) // if 2d sound
        {
            audio.Play();
        }
    }

    public void Stop(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            audio.Stop();
        }
    }

    /// <summary>
    /// For audio that fires a lot of times, 
    /// for audio that cannot be stopped.
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayOneShot(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            audio.PlayOneShot();
        }
    }

    /// <summary>
    /// Plays a clip at a point in world space.
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="point"></param>
    public void PlayAtPoint(AudioType audioName, Vector3 point)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            AudioSource.PlayClipAtPoint(audio.Clip, point);
        }
    }

    /// <summary>
    /// For 3D sounds. Adds a new AudioSource to the gameobject, plays its clip.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="gameObject"></param>
    public void PlayOnGameObject(AudioType audioName, GameObject gameObject)
    {
        AudioObject newAudio = AddAudioToGameObject(audioName, gameObject);
        newAudio.Play();
    }

    /// <summary>
    /// For 3D sounds.
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private AudioObject AddAudioToGameObject(AudioType audioName, GameObject gameObject)
    {
        AudioObject originalAudio;
        AudioObject newAudio = new AudioObject();
        if (TryGetAudioObject(audioName, out originalAudio))
        {
            newAudio = new AudioObject(originalAudio, gameObject);
        }
        return newAudio;
    }

    public float GetAudioLength(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            return audio.Clip.length;
        }
        else
        {
            return 0f;
        }
    }

    public void PlayWalkingSound()
    {
        if (!footstepsSource.isPlaying && Time.time > lastPlayedTime + delayBetweenSteps)
        {
            int index = Random.Range(0, footstepClips.Length - 1);
            while (index == lastPlayedIndex)
            {
                index = Random.Range(0, footstepClips.Length - 1);
            }
            footstepsSource.PlayOneShot(footstepClips[index]);
            lastPlayedTime = Time.time;
            lastPlayedIndex = index;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Stops AudioSources from playing.
    /// Unsubscribes from old, resubscribes to new on scene change.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllSounds();
        ResumeInGameSounds();
        UnsubscribeFromTimeChanges();
        SubScribeToTimeChanges();
    }

    private void OnDestroy()
    {
        UnsubscribeFromTimeChanges();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SubScribeToTimeChanges()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (timeManager)
        {
            timeManager.TimePaused += PauseInGameSounds;
            timeManager.TimeResumed += ResumeInGameSounds;
        }
    }

    private void UnsubscribeFromTimeChanges()
    {
        if (timeManager)
        {
            timeManager.TimePaused -= PauseInGameSounds;
            timeManager.TimeResumed -= ResumeInGameSounds;
        }
    }

    private void PauseInGameSounds()
    {
        AudioListener.pause = true;
    }

    private void ResumeInGameSounds()
    {
        AudioListener.pause = false;
    }

    /// <summary>
    /// On scene changes, stops all playing sounds on this gameobject, as it can persist between scenes.
    /// </summary>
    private void StopAllSounds()
    {
        foreach (AudioObject audio in audioObjects.Values)
        {
            audio.Stop();
        }
    }
}
