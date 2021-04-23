using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private InputManager input;
    [SerializeField] private Transform orientation;

    private Vector3 horizontalDirection;
    private Vector3 projectedDirection;
    private float x;
    private float y;
    private float z;
    [SerializeField]
    private LayerMask groundMask;
    private RaycastHit raycastHit;
    private RaycastHit[] sphereCastHits;
    private bool canJump;
    private bool jump;
    private bool isOnSlope;
    private bool isSlopeTooSteep;
    [HideInInspector]
    public List<GameObject> pickupablesUnderPlayer;
    private bool isOnRideableObject;
    private Transform rideableObjectUnderPlayer;
    private const float walkSpeed = 5f;
    private const float jumpForce = 9.5f;
    private const float largeGroundOffset = 0.8f; // can we jump
    private const float smallGroundOffset = 0.2f; // are we on a slope, are we standing on a cube
    private const float maxSlopeAngle = 45;

    private bool isFrozen;
    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        input = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (!isFrozen)
        {
            if (input.PressedJump)
            {
                jump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            CheckGround();

            if (isOnRideableObject)
            {
                transform.SetParent(rideableObjectUnderPlayer);
            }
            else
            {
                transform.SetParent(null);
            }

            horizontalDirection = (orientation.transform.forward * input.Vertical + orientation.transform.right * input.Horizontal).normalized;
            projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, raycastHit.normal).normalized;

            x = 0;
            y = 0;
            z = 0;
            rb.useGravity = true;

            // we are on a slope and we settled on the ground
            if (isOnSlope) 
            {
                if (!isSlopeTooSteep)
                {
                    // turn off gravity if there is no input
                    if (input.Horizontal == 0 && input.Vertical == 0)
                    {
                        rb.useGravity = false;
                    }
                    // walk according to the direction projected on the slope
                    else
                    {
                        x = projectedDirection.x * walkSpeed;
                        y = projectedDirection.y * walkSpeed;
                        z = projectedDirection.z * walkSpeed;
                    }
                }
                else
                {
                    // don't turn off gravity, add a little, let it slide down
                    x = projectedDirection.x * walkSpeed;
                    y = -2f;
                    z = projectedDirection.z * walkSpeed;
                    jump = false;
                }
            }
            // walking on normal ground / falling - applies gravity
            else
            {
                x = projectedDirection.x * walkSpeed;
                y = rb.velocity.y;
                z = projectedDirection.z * walkSpeed;
            }

            if (jump && canJump)
            {
                y = jumpForce;
            }
            jump = false;

            rb.velocity = new Vector3(x, y, z);
        }
    }

    private void CheckGround()
    {
        // check if we can jump, if we are standing on cubes or a moving object
        float radius = col.bounds.extents.x - 0.01f; // little smaller than the capsule's, so that we can't jump off walls
        Vector3 position = new Vector3(transform.position.x, transform.position.y - col.bounds.extents.y + col.bounds.extents.x, transform.position.z);
        sphereCastHits = Physics.SphereCastAll(position, radius, Vector3.down, largeGroundOffset, groundMask, QueryTriggerInteraction.Ignore);

        isOnRideableObject = false;
        List<GameObject> pickupables = new List<GameObject>();
        if (sphereCastHits.Length > 0)
        {
            canJump = true;
            foreach (RaycastHit hit in sphereCastHits)
            {
                if (hit.distance < smallGroundOffset)
                {
                    if (hit.collider.gameObject.layer == 10)
                    {
                        pickupables.Add(hit.collider.gameObject);
                    }

                    if (hit.collider.CompareTag("RideableObject"))
                    {
                        isOnRideableObject = true;
                        rideableObjectUnderPlayer = hit.collider.gameObject.transform; // ? hit.transform
                    }
                }
            }
        }
        else
        {
            canJump = false;
        }
        pickupablesUnderPlayer = pickupables;

        // check if we are on a slope and the slope angle
        if (canJump) // only if we are no farther than jump distance from the ground
        {
            Vector3 feetPosition = new Vector3(transform.position.x, transform.position.y - col.bounds.extents.y, transform.position.z);
            if (Physics.Raycast(feetPosition, Vector3.down, out raycastHit, smallGroundOffset, groundMask, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(Vector3.up, raycastHit.normal);
                isOnSlope = groundAngle > 0f ? true : false;
                isSlopeTooSteep = groundAngle > maxSlopeAngle ? true : false;
            }
            else
            {
                isOnSlope = false;
            }
        }
    }
}
