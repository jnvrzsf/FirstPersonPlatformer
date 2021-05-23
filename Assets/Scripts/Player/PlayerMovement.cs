using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    private InputManager input;
    private Rigidbody rb;
    private Collider col;

    private Vector3 horizontalDirection;
    private Vector3 projectedDirection;
    private float x;
    private float y;
    private float z;
    private Vector3 movement;
    [SerializeField]
    private LayerMask groundMask;
    private RaycastHit raycastHit;
    private readonly RaycastHit[] sphereCastHitBuffer = new RaycastHit[10];
    private bool isGrounded; // for playing footsteps
    private bool canJump;
    private bool jump;
    private bool isOnSlope;
    private bool isSlopeTooSteep;
    [HideInInspector]
    public List<Collider> pickupablesUnderPlayer;
    private bool isOnMovingObject;
    private Transform movingObjectUnderPlayer;
    private const float walkSpeed = 5f;
    private const float jumpForce = 9.5f;
    private const float largeGroundOffset = 0.8f; // can we jump
    private const float smallGroundOffset = 0.2f; // are we on a slope, are we standing on a cube, are we grounded
    private const float maxSlopeAngle = 45;

    private bool isFrozen;
    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;

    void Awake()
    {
        input = FindObjectOfType<InputManager>();
        Assert.IsNotNull(input, "InputManager not found.");
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isFrozen)
        {
            if (input.PressedJump)
            {
                jump = true;
            }

            if (isGrounded && !input.IsIdle)
            {
                AudioManager.instance.PlayWalkingSound();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            CheckGround();

            CheckMovingObject();

            CalculateDirection();

            CalculateMovement();

            MovePlayer();
        }
    }

    private void CheckGround()
    {
        CastSphere();
        CheckSlope();
    }

    private void CastSphere()
    {
        // check if we can jump, if we are standing on cubes or a moving object
        float radius = col.bounds.extents.x - 0.01f; // little smaller than the capsule's, so that we can't jump off walls
        Vector3 position = new Vector3(transform.position.x, transform.position.y - col.bounds.extents.y + col.bounds.extents.x, transform.position.z);
        int hitCount = Physics.SphereCastNonAlloc(position, radius, Vector3.down, sphereCastHitBuffer, largeGroundOffset, groundMask, QueryTriggerInteraction.Ignore);

        isOnMovingObject = false;
        pickupablesUnderPlayer.Clear();
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = sphereCastHitBuffer[i];

                canJump = true;

                if (hit.distance < smallGroundOffset)
                {
                    if (isGrounded == false)
                    {
                        // landing
                        AudioManager.instance.PlayOneShot(AudioType.Landing);
                    }
                    isGrounded = true;

                    if (hit.collider.gameObject.layer == Layers.Pickupable)
                    {
                        pickupablesUnderPlayer.Add(hit.collider);
                    }

                    if (!isOnMovingObject && hit.collider.CompareTag(Tags.Moving))
                    {
                        isOnMovingObject = true;
                        movingObjectUnderPlayer = hit.transform;
                    }
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckSlope()
    {
        // check if we are on a slope and the slope angle
        if (canJump) // only if we are no farther than jump distance from the ground
        {
            Vector3 feetPosition = new Vector3(transform.position.x, transform.position.y - col.bounds.extents.y, transform.position.z);
            if (Physics.Raycast(feetPosition, Vector3.down, out raycastHit, smallGroundOffset, groundMask, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(Vector3.up, raycastHit.normal);
                isOnSlope = groundAngle > 0f;
                isSlopeTooSteep = groundAngle > maxSlopeAngle ? true : false;
            }
            else
            {
                isOnSlope = false;
            }
        }
    }

    private void CheckMovingObject()
    {
        if (isOnMovingObject)
        {
            if (transform.parent == null)
            {
                transform.SetParent(movingObjectUnderPlayer);
            }
        }
        else
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
        }
    }


    private void CalculateDirection()
    {
        horizontalDirection = (orientation.forward * input.Vertical + orientation.right * input.Horizontal).normalized;
        projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, raycastHit.normal).normalized;
    }

    private void CalculateMovement()
    {
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
                if (input.IsIdle)
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

        CheckJump();

        movement = new Vector3(x, y, z);
    }


    private void CheckJump()
    {
        if (jump && canJump)
        {
            y = jumpForce;
            if (!isGrounded)
            {
                AudioManager.instance.PlayOneShot(AudioType.Landing);
            }
            AudioManager.instance.Play(AudioType.Jump);
        }
        jump = false;
    }

    private void MovePlayer()
    {
        rb.velocity = movement;
    }
}
