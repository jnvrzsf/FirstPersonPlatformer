using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -45f;
    [SerializeField] public float jumpHeight = 3f;
    public Vector3 velocity;
    public bool isGrounded = false;
    private bool isXLocked = false;
    private bool isZLocked = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // get move direction
        float x = isXLocked ? 0 : Input.GetAxis("Horizontal");
        float z = isZLocked ? 0 : Input.GetAxis("Vertical");
        // inputDirection, h v
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

        // move the player (final step)
        controller.Move((moveDirection * speed + velocity) * Time.deltaTime);
    }
}
