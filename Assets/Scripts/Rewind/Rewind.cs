using UnityEngine;

public class Rewind : MonoBehaviour
{
    private RayFromCamera ray;
    private PlayerState playerState;
    private InputManager input;
    private Rewindable rewindedObject;
    private RewindablePlayer player;

    private void Awake()
    {
        ray = GetComponent<RayFromCamera>();
        playerState = GetComponent<PlayerState>();
        input = FindObjectOfType<InputManager>();
        player = GetComponent<RewindablePlayer>();
    }

    private void Update()
    {
        if (input.PressedRewind)
        {
            if (input.isPressingShift)
            {
                rewindedObject = player;
                rewindedObject.StartRewinding();
            }
            else
            {
                if (ray.hitSomething)
                {
                    Rewindable rewindable = ray.hitInfo.collider.GetComponent<Rewindable>();
                    if (rewindable != null)
                    {
                        rewindedObject = rewindable;
                        rewindedObject.StartRewinding();
                    }
                }
            }
        }
        if (input.ReleasedRewind && rewindedObject != null)
        {
            rewindedObject.StopRewinding();
            rewindedObject = null;
        }
    }
}
