using UnityEngine;

public class Rewind : MonoBehaviour
{
    private RayFromCamera ray;
    private PlayerState playerState;
    private InputManager input;
    private Rewindable rewindedObject;

    private void Awake()
    {
        ray = GetComponent<RayFromCamera>();
        playerState = GetComponent<PlayerState>();
        input = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (input.PressedRewind)
        {
            if (input.IsPressingRun)
            {
                rewindedObject = GetComponent<RewindablePlayer>();
                // unfreeze player
                if (playerState.IsDead)
                {
                    Time.timeScale = 1f;
                }
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
