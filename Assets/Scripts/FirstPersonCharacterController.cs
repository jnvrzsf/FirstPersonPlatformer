using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float speed = 10f;
    private float gravity = -45f;
    public float jumpHeight = 3f;
    public Vector3 velocity;
    public bool isGrounded = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * z + transform.right * x;
        velocity.y += gravity * Time.deltaTime;
        controller.Move((move.normalized * speed + velocity) * Time.deltaTime);

        isGrounded = controller.isGrounded;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }
}
