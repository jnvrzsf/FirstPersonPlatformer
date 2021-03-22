using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private PlayerInput playerInput;

    private float walkSpeed = 5f;
    private Vector3 moveVector;
    private float jumpForce = 7f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundMask;
    private RaycastHit hitInfo;
    private float groundDistance = 0.2f; // player width: 1, max slope angle: 45 degrees
    private float jumpDistance = 0.8f;
    private bool isGrounded => hitInfo.distance < col.bounds.extents.y + groundDistance;
    private bool isAllowedToJump => hitInfo.distance < col.bounds.extents.y + jumpDistance;
    private bool isOnSlope => isGrounded && hitInfo.normal != Vector3.up ? true : false;

    private Vector3 horizontalDirection;
    private Vector3 projectedDirection;

    public float targetSpeed = 5f;
    private bool jump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.isJumpPressed && isAllowedToJump)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        rb.useGravity = true;

        CastRayDown();

        horizontalDirection = (transform.forward * playerInput.Vertical + transform.right * playerInput.Horizontal).normalized;
        projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, hitInfo.normal).normalized;

        float x = 0;
        float y = 0;
        float z = 0;

        if (isOnSlope)
        {
            if (playerInput.Horizontal == 0 && playerInput.Vertical == 0 && isGrounded)
            {
                rb.useGravity = false;
            }
            else
            {
                x = projectedDirection.x * walkSpeed;
                y = projectedDirection.y * walkSpeed;
                z = projectedDirection.z * walkSpeed;
            }
        }
        else
        {
            x = projectedDirection.x * walkSpeed;
            y = rb.velocity.y;
            z = projectedDirection.z * walkSpeed;
        }

        if (jump)
        {
            y = jumpForce;
            jump = false;
        }

        moveVector = new Vector3(x, y, z);
        rb.velocity = moveVector;
    }

    private void CastRayDown()
    {
        Physics.Raycast(transform.position, Vector3.down, out hitInfo);
    }
}
