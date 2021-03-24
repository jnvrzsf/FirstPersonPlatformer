using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector2 mouseInput;
    private float vertical;
    private float horizontal;
    private float mouseSensitivity = 100f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        vertical += mouseInput.y * mouseSensitivity * Time.deltaTime;
        horizontal += mouseInput.x * mouseSensitivity * Time.deltaTime;

        vertical = Mathf.Clamp(vertical, -90f, 90f);

        transform.localRotation = Quaternion.Euler(vertical, 0f, 0f);
        player.Rotate(Vector3.up * horizontal);
    }

    private void GetInput()
    {
        mouseInput.x = Input.GetAxis("Mouse X");
        mouseInput.y = Input.GetAxis("Mouse Y");
    }
}