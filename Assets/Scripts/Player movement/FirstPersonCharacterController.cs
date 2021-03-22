﻿using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;

    private bool isXLocked;
    private bool isZLocked;
    private bool isSpeedingLocked;

    private float moveSpeed;
    private float walkSpeed = 5f;
    private float runSpeed = 10f;
    private float jumpHeight = 2f;
    private float gravity = -20f;
    private float verticalVelocity;
    private bool isGrounded; // for testing

    private Vector3 moveVector;
    private RaycastHit raycastHit;

    private bool isOnMovingObject;
    private Transform hitTransform;

    private float pushPower = 1.0f;

    public bool isFrozen;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!isFrozen)
        {
            // cast ray down
            Physics.Raycast(transform.position, Vector3.down, out raycastHit);

            // check if grounded
            isGrounded = controller.isGrounded;

            // on ground, slope
            if (isGrounded)
            {
                // unlock horizontal movement and speed
                isXLocked = false;
                isZLocked = false;
                isSpeedingLocked = false;

                // calculate move vector
                Vector3 horizontalDirection = (transform.forward * playerInput.Vertical + transform.right * playerInput.Horizontal).normalized;
                Vector3 projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, raycastHit.normal).normalized;
                moveSpeed = playerInput.isRunPressed ? runSpeed : walkSpeed;
                moveVector = projectedDirection * moveSpeed;

                // add gravity to move vector
                verticalVelocity = -4; // to make isGrounded work
                moveVector.y += verticalVelocity;

                // jump
                if (playerInput.isJumpPressed)
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    moveVector.y = verticalVelocity;
                }
            }
            // jumping, falling
            else
            {
                // lock horizontal movement if it halted mid air
                if (playerInput.Vertical == 0) isXLocked = true;
                if (playerInput.Horizontal == 0) isZLocked = true;
                // lock speed
                if (playerInput.isRunPressed) isSpeedingLocked = true;

                // calculate move vector
                float x = isXLocked ? 0 : playerInput.Vertical;
                float z = isZLocked ? 0 : playerInput.Horizontal;
                Vector3 horizontalDirection = (transform.forward * x + transform.right * z).normalized;
                moveSpeed = isSpeedingLocked ? runSpeed : walkSpeed;
                moveVector = horizontalDirection * moveSpeed;

                // increase downward velocity
                verticalVelocity += gravity * Time.deltaTime;
                moveVector.y = verticalVelocity;
            }

            // move the player
            controller.Move(moveVector * Time.deltaTime);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitTransform = hit.transform;
        if (hit.gameObject.CompareTag("MovingObject") && controller.isGrounded)
        {
            isOnMovingObject = true;
        }

        // push Rigidbody
        Rigidbody rb = hit.rigidbody;
        if (rb == null || rb.isKinematic)
            return;
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.velocity = pushDirection * pushPower;
    }

    private void FixedUpdate()
    {
        if (isOnMovingObject)
        {
            if (transform.parent == null)
            {
                transform.SetParent(hitTransform);
                Debug.Log("ON");
            }
        }
        else if (transform.parent != null)
        {
            transform.SetParent(null);
            Debug.Log("OFF");
        }
        isOnMovingObject = false;
    }

    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;
}
