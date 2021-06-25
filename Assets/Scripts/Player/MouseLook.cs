using UnityEngine;
using UnityEngine.Assertions;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform playerOrientation;
    private InputManager input;
    private float xRotation;
    private float yRotation;
    private float mouseSensitivityX = 250f;
    private float mouseSensitivityY = 400f;
    private bool isFrozen;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        Assert.IsNotNull(input, "InputManager not found.");
        yRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!isFrozen)
        {
            xRotation -= input.MouseY * mouseSensitivityY * Time.deltaTime;
            yRotation += input.MouseX * mouseSensitivityX * Time.deltaTime;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;

    /// <summary>
    /// Updates the stored rotational values when we finish rewinding,
    /// because the camera's rotation might have been altered during rewind.
    /// </summary>
    public void UpdateRotation()
    {
        xRotation = cam.rotation.eulerAngles.x;
        yRotation = cam.rotation.eulerAngles.y;
    }
}
