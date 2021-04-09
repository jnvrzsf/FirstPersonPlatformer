using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirstPersonRBCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private PlayerInput playerInput;
    [SerializeField] private Transform orientation;

    private float walkSpeed = 5f;
    private Vector3 moveVector;
    private float jumpForce = 9.5f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundMask;
    private RaycastHit raycastHit;
    private RaycastHit[] sphereCastHits;
    private float groundDistance = 0.2f;
    private float jumpDistance = 0.8f;
    private bool isGrounded;
    private bool canJump;
    private bool jump;
    private bool isOnSlope;
    [HideInInspector] public List<GameObject> pickupablesUnderPlayer;

    private Vector3 horizontalDirection;
    private Vector3 projectedDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.IsJumpPressed && canJump)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        horizontalDirection = (orientation.transform.forward * playerInput.Vertical + orientation.transform.right * playerInput.Horizontal).normalized;
        projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, raycastHit.normal).normalized;

        float x = 0;
        float y = 0;
        float z = 0;
        rb.useGravity = true;

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

    private void CheckGround()
    {
        float sphereRadius = col.bounds.extents.x;
        Vector3 position = new Vector3(transform.position.x, transform.position.y - col.bounds.extents.x, transform.position.z);
        sphereCastHits = Physics.SphereCastAll(position, sphereRadius, Vector3.down, jumpDistance, groundMask, QueryTriggerInteraction.Ignore);
        List<GameObject> pickupables = new List<GameObject>();
        if (sphereCastHits.Length > 0)
        {
            canJump = true;
            foreach (RaycastHit hit in sphereCastHits)
            {
                if (hit.distance < groundDistance)
                {
                    isGrounded = true;
                    if (hit.collider.gameObject.layer == 10)
                    {
                        pickupables.Add(hit.collider.gameObject);
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
            isGrounded = false;
        }
        pickupablesUnderPlayer = pickupables;

        if (isGrounded)
        {
            Physics.Raycast(transform.position, Vector3.down, out raycastHit); // TODO: set max length, ground mask, turn off trigger interaction, set position to be at feet (create a transform and feed it to every raycast?)
            isOnSlope = raycastHit.normal != Vector3.up ? true : false;
        }
        else
        {
            isOnSlope = false;
        }

    }
}
