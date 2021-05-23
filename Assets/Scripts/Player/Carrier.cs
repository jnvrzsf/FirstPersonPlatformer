using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(RayFromCamera), typeof(PlayerMovement), typeof(Rewinder))]
public class Carrier : MonoBehaviour
{
    private InputManager input;
    private RayFromCamera ray;
    private PlayerMovement player;
    private NonPlayerTrigger nonPlayerTrigger;
    private Rewinder rewinder;
    public Transform carryPoint;
    private Pickupable pickupObject;
    public bool isCarrying => pickupObject != null;
    private float distance => Vector3.Distance(carryPoint.position, pickupObject.transform.position);
    private const float maxDistance = 5f;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        Assert.IsNotNull(input, "InputManager not found.");
        nonPlayerTrigger = GetComponentInChildren<NonPlayerTrigger>();
        Assert.IsNotNull(input, "NonPlayerTrigger not found.");
        ray = GetComponent<RayFromCamera>();
        player = GetComponent<PlayerMovement>();
        rewinder = GetComponent<Rewinder>();
    }

    private bool carriedObjectIsTooFar => distance > maxDistance;
    private bool canBeDropped => !nonPlayerTrigger.isPickupableOverlapping;

    private void Update()
    {
        if (!rewinder.isRewinding)
        {
            if (isCarrying)
            {
                if ((input.PressedAction || carriedObjectIsTooFar) && canBeDropped)
                {
                    Drop();
                }
            }
            else if (input.PressedAction)
            {
                if (ray.hitSomething && ray.hitInfo.distance < maxDistance)
                {
                    Pickupable p = ray.hitInfo.collider.GetComponent<Pickupable>();
                    if (p != null)
                    {
                        // check whether the object to pick up is under the player, if so, return
                        if (player.pickupablesUnderPlayer.Count > 0)
                        {
                            foreach (Collider col in player.pickupablesUnderPlayer)
                            {
                                if (ReferenceEquals(ray.hitInfo.collider, col))
                                {
                                    return;
                                }
                            }
                        }

                        if (p.canBePickedUp)
                        {
                            PickUp(p);
                        }
                    }
                }
            }
        }
    }

    private void PickUp(Pickupable pickupable)
    {
        pickupObject = pickupable;
        pickupObject.SetToPickedUp(this);
        AudioManager.instance.PlayOneShot(AudioType.ObjectPickUp);
    }

    public void Drop()
    {
        pickupObject.SetToDropped();
        pickupObject = null;
        AudioManager.instance.PlayOneShot(AudioType.ObjectDrop);
    }

    private void FixedUpdate()
    {
        if (isCarrying)
        {
            pickupObject.FollowCarrier(maxDistance);
        }
    }
}
