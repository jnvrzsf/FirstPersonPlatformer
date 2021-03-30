using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerInput playerInput;
    private float xRotation;
    private float yRotation;
    private float mouseSensitivityX = 250f;
    private float mouseSensitivityY = 400f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        xRotation -= playerInput.MouseY * mouseSensitivityY * Time.deltaTime;
        yRotation += playerInput.MouseX * mouseSensitivityX * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); 
    }
}
