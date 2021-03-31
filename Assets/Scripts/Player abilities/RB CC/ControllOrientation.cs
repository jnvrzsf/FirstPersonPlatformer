using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllOrientation : MonoBehaviour
{
    [SerializeField] private Transform playerSight;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private PlayerInput playerInput;
    private float xRotation;
    private float yRotation;
    private float mouseSensitivityX = 250f;
    private float mouseSensitivityY = 400f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yRotation = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        xRotation -= playerInput.MouseY * mouseSensitivityY * Time.deltaTime;
        yRotation += playerInput.MouseX * mouseSensitivityX * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerSight.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0); 
    }
}
