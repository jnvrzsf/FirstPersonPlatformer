using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

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
    [SerializeField] private GameObject rewindCountdown;
    private TextMeshProUGUI rewindCountdownText;
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
        rewindCountdownText = rewindCountdown.GetComponentInChildren<TextMeshProUGUI>();
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
            else if (isRewinding)
            {
                SetCountdownText();

                if (!input.isPressingRewind)
                {
                    StopRewinding();
                }
            }
        }
    }

    private void SetCountdownText()
    {
        int secondsLeft = Mathf.CeilToInt(rewindedObject.currentRecordCount * Time.fixedDeltaTime);
        rewindCountdownText.text = secondsLeft.ToString();
    }

    private void StartRewinding(Rewindable rewindable)
    {
        rewindedObject = rewindable;
        rewindedObject.OutOfRecords += StopRewinding;
        rewindedObject.StartRewinding();
        rewindAudio?.source.Play();
        chromaticAberration.active = true;
        SetCountdownText();
        rewindCountdown.SetActive(true);
    }

    private void StopRewinding()
    {
        rewindedObject.OutOfRecords -= StopRewinding;
        rewindedObject.StopRewinding();
        rewindedObject = null;
        rewindAudio?.source.Stop();
        chromaticAberration.active = false;
        rewindCountdown.SetActive(false);
    }
}
