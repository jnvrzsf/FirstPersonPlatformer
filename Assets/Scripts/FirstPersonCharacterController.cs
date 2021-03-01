using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;

    private Vector2 keyboardInput;
    private float walkSpeed = 5f;
    private float jumpHeight = 2f;
    private float gravity = -20f;
    private float verticalVelocity;
    private bool isGrounded; // for testing
    private bool isXLocked;
    private bool isZLocked;
    private Vector3 moveVector;
    private RaycastHit hit;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();

        // cast ray down
        Physics.Raycast(transform.position, Vector3.down, out hit);

        // check if grounded
        isGrounded = controller.isGrounded;

        // on ground, slope
        if (isGrounded)
        {
            // unlock horizontal movement
            isXLocked = false;
            isZLocked = false;

            // calculate move vector
            Vector3 horizontalDirection = (transform.forward * keyboardInput.x + transform.right * keyboardInput.y).normalized;
            Vector3 projectedDirection = Vector3.ProjectOnPlane(horizontalDirection, hit.normal).normalized;
            moveVector = projectedDirection * walkSpeed;

            // add gravity to move vector
            verticalVelocity = -4; // to make isGrounded work
            moveVector.y += verticalVelocity; 

            // jump
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                moveVector.y = verticalVelocity;
            }
        }
        // jumping, falling
        else
        {
            // lock horizontal movement if it halted mid air
            if (keyboardInput.x == 0) isXLocked = true;
            if (keyboardInput.y == 0) isZLocked = true;

            // calculate move vector
            float x = isXLocked ? 0 : keyboardInput.x;
            float z = isZLocked ? 0 : keyboardInput.y;
            Vector3 horizontalDirection = (transform.forward * x + transform.right * z).normalized;
            moveVector = horizontalDirection * walkSpeed;

            // increase downward velocity
            verticalVelocity += gravity * Time.deltaTime;
            moveVector.y = verticalVelocity;
        }

        // move the player
        controller.Move(moveVector * Time.deltaTime);
    }

    private void GetInput()
    {
        keyboardInput.x = Input.GetAxisRaw("Vertical");
        keyboardInput.y = Input.GetAxisRaw("Horizontal");
    }
}
