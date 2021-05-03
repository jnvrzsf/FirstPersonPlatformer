using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioObject[] sounds; // for serialization
    private Dictionary<AudioType, AudioObject> audioObjects; // for faster lookup

    [SerializeField] private AudioClip[] footstepClips;
    private AudioSource footstepsSource;
    private int lastPlayedIndex;
    private float lastPlayedTime;
    private const float footstepsVolume = 0.8f;
    private const float delayBetweenSteps = 0.4f;

    private TimeManager timeManager;

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
            if (!audio.settings.is3DSound)
            {
                audio.InitAudioSource(gameObject);
            }
            audioObjects.Add(audio.name, audio);
        }
    }

    public bool TryGetAudioObject(AudioType audioName, out AudioObject audioObject)
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
    /// Eg. for looping audio, for audio that doesnt rapidly fires.
    /// <summary>
    /// <param name="audioName"></param>
    public void Play(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            audio.source.Play();
        }
    }

    /// <summary>
    /// Eg. for audio that should not be interrupted by playing it again.
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayOneShot(AudioType audioName)
    {
        AudioObject audio;
        if (TryGetAudioObject(audioName, out audio))
        {
            audio.source.PlayOneShot(audio.source.clip);
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
            AudioSource.PlayClipAtPoint(audio.settings.clip, point);
        }
    }

    /// <summary>
    /// For 3D sounds. Requesting script can decide whether to use 
    /// eg. Play or PlayOneShot on the AudioSource.
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public AudioObject AddAudioToGameObject(AudioType audioName, GameObject gameObject)
    {
        AudioObject originalAudio;
        AudioObject newAudio = null; // new empty?
        if (TryGetAudioObject(audioName, out originalAudio))
        {
            newAudio = new AudioObject(originalAudio, gameObject);
        }
        return newAudio;
    }

    /// <summary>
    /// For 3D sounds. Adds a new AudioSource to the gameobject, plays its clip, 
    /// and returns the AudioObject so the requesting script can have control over the play.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="gameObject"></param>
    public AudioObject PlayOnGameObject(AudioType audioName, GameObject gameObject)
    {
        AudioObject newAudio = AddAudioToGameObject(audioName, gameObject);
        newAudio.source.Play();
        return newAudio;
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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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
            if (audio.source != null)
            {
                audio.source.Stop();
            }
        }
    }
}
