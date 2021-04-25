using UnityEngine;

public class Rewinder : MonoBehaviour
{
    private RayFromCamera ray;
    private InputManager input;
    private Rewindable rewindedObject;
    private RewindablePlayer player;
    public bool isRewinding => rewindedObject != null;

    private void Awake()
    {
        ray = GetComponent<RayFromCamera>();
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
        if (input.ReleasedRewind && isRewinding)
        {
            rewindedObject.StopRewinding();
            rewindedObject = null;
        }
    }
}
