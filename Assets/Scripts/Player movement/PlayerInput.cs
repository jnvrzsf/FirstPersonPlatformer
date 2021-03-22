using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool isJumpPressed { get; private set; }
    public bool isRunPressed { get; private set; }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        isJumpPressed = Input.GetKeyDown(KeyCode.Space);
        isRunPressed = Input.GetKey(KeyCode.LeftShift);
    }
}
