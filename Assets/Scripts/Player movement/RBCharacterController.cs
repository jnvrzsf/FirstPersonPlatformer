using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Vector2 keyboardInput;
    private float walkSpeed = 5f;
    private Vector3 moveVector;
    private float jumpForce = 7f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float jumpDistance = 0.8f;
    private bool isGrounded => hitInfo.distance < col.bounds.extents.y + groundDistance;
    private bool canJump => hitInfo.distance < col.bounds.extents.y + jumpDistance;
    private bool isOnSlope => hitInfo.normal != Vector3.up ? true : false;
    RaycastHit hitInfo;

    private Vector3 horizontalDirection;
    private Vector3 projectedDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        GetInput();
        CastRayDown();

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        horizontalDirection = (transform.forward * keyboardInput.y + transform.right * keyboardInput.x).normalized;
        projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, hitInfo.normal).normalized;
        float x = projectedDirection.x * walkSpeed;
        float y = rb.velocity.y;
        float z = projectedDirection.z * walkSpeed;
        moveVector = new Vector3(x, y, z);
    }

    private void GetInput()
    {
        keyboardInput.x = Input.GetAxisRaw("Horizontal");
        keyboardInput.y = Input.GetAxisRaw("Vertical");
    }

    private void CastRayDown()
    {
        Physics.Raycast(transform.position, Vector3.down, out hitInfo);
    }

    private void FixedUpdate()
    {
        if (keyboardInput.x == 0 && keyboardInput.y == 0 && isGrounded)
        {
            rb.velocity =  new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            if (isOnSlope)
            {
                // y = 0 if halted
                rb.velocity = projectedDirection * walkSpeed;
            }
            else
            {
                rb.velocity = moveVector;
            }
        }
    }
}
