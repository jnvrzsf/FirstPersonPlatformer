using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Assertions;

[RequireComponent(typeof(RayFromCamera), typeof(RewindablePlayer), typeof(Carrier))]
public class Rewinder : MonoBehaviour
{
    private InputManager input;
    private TimeManager timeManager;

    private RayFromCamera ray;
    private RewindablePlayer player;
    private Carrier carrier;

    private ChromaticAberration chromaticAberration;

    [SerializeField] private GameObject rewindCountdown;
    private TextMeshProUGUI rewindCountdownText;

    private Rewindable rewindedObject;
    public bool isRewinding => rewindedObject != null;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        Assert.IsNotNull(input, "InputManager not found.");
        timeManager = FindObjectOfType<TimeManager>();
        Assert.IsNotNull(input, "TimeManager not found.");

        ray = GetComponent<RayFromCamera>();
        player = GetComponent<RewindablePlayer>();
        carrier = GetComponent<Carrier>();

        VolumeProfile volumeProfile = Camera.main.GetComponent<Volume>().profile;
        volumeProfile.TryGet<ChromaticAberration>(out chromaticAberration);
        rewindCountdownText = rewindCountdown.GetComponentInChildren<TextMeshProUGUI>();
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
        AudioManager.instance.Play(AudioType.Rewind);
        chromaticAberration.active = true;
        SetCountdownText();
        rewindCountdown.SetActive(true);
    }

    private void StopRewinding()
    {
        rewindedObject.OutOfRecords -= StopRewinding;
        rewindedObject.StopRewinding();
        rewindedObject = null;
        AudioManager.instance.Stop(AudioType.Rewind);
        chromaticAberration.active = false;
        rewindCountdown.SetActive(false);
    }
}
