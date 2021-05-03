using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Rewinder : MonoBehaviour
{
    private RayFromCamera ray;
    private InputManager input;
    private Rewindable rewindedObject;
    private RewindablePlayer player;
    private Carrier carrier;
    private TimeManager timeManager;
    private AudioObject rewindAudio;
    private ChromaticAberration chromaticAberration;
    public bool isRewinding => rewindedObject != null;

    private void Awake()
    {
        ray = GetComponent<RayFromCamera>();
        input = FindObjectOfType<InputManager>();
        player = GetComponent<RewindablePlayer>();
        carrier = GetComponent<Carrier>();
        timeManager = FindObjectOfType<TimeManager>();
        VolumeProfile volumeProfile = Camera.main.GetComponent<Volume>().profile;
        volumeProfile.TryGet<ChromaticAberration>(out chromaticAberration);
    }

    private void Start()
    {
        AudioManager.instance.TryGetAudioObject(AudioType.Rewind, out rewindAudio);
    }

    private void Update()
    {
        if (!timeManager.isGamePaused)
        {
            if (input.isPressingShift && input.PressedRewind)
            {
                StartRewinding(player);
            }
            else if (input.PressedRewind && !carrier.isCarrying && ray.hitSomething)
            {
                Rewindable rewindable = ray.hitInfo.collider.GetComponent<Rewindable>();
                if (rewindable != null)
                {
                    StartRewinding(rewindable);
                }
            }
            else if (isRewinding && !input.isPressingRewind)
            {
                StopRewinding();
            }
        }
    }

    private void StartRewinding(Rewindable rewindable)
    {
        rewindedObject = rewindable;
        rewindedObject.OutOfRecords += StopRewinding;
        rewindedObject.StartRewinding();
        rewindAudio?.source.Play();
        chromaticAberration.active = true;
    }

    private void StopRewinding()
    {
        rewindedObject.OutOfRecords -= StopRewinding;
        rewindedObject.StopRewinding();
        rewindedObject = null;
        rewindAudio?.source.Stop();
        chromaticAberration.active = false;
    }
}
