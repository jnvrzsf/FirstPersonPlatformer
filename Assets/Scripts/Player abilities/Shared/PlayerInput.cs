using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsRunPressed { get; private set; }
    public bool IsActionPressed { get; private set; }
    public float MouseX { get; set; }
    public float MouseY { get; set; }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        IsJumpPressed = Input.GetKeyDown(KeyCode.Space);
        IsRunPressed = Input.GetKey(KeyCode.LeftShift);
        IsActionPressed = Input.GetKeyDown(KeyCode.E);
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
    }
}
