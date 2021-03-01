using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;

    private float moveSpeed = 10f;
    private float gravity = -45f;
    private float jumpHeight = 3f;
    private Vector3 velocity;

    private bool isGrounded = false; // for testing
    private bool isXLocked = false;
    private bool isZLocked = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // get input, move direction
        float x = isXLocked ? 0 : Input.GetAxisRaw("Horizontal");
        float z = isZLocked ? 0 : Input.GetAxisRaw("Vertical");
        Vector3 move = transform.forward * z + transform.right * x;
        Vector3 moveDirection = move.normalized;

        // apply gravity (before jump boost)
        velocity.y += gravity * Time.deltaTime;

        if (isGrounded)
        {
            // unlock horizontal movement
            if (isZLocked) isZLocked = false;
            if (isXLocked) isXLocked = false;
            
            // jump boost (will gradually die down)
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // reset building gravity (after applying gravity)
            if (velocity.y < 0f)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            // lock horizontal movement if it halted mid air
            if (x == 0) isXLocked = true;
            if (z == 0) isZLocked = true;
        }

        // finally, move the player
        controller.Move((moveDirection * moveSpeed + velocity) * Time.deltaTime);
    }
}
