using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool PressedJump { get; private set; }
    public bool IsPressingRun { get; private set; }
    public bool PressedAction { get; private set; }
    public bool PressedRewind { get; private set; }
    public bool ReleasedRewind { get; private set; }
    public bool PressedEscape { get; private set; }

    void Update()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        PressedJump = Input.GetKeyDown(KeyCode.Space);
        IsPressingRun = Input.GetKey(KeyCode.LeftShift);
        PressedAction = Input.GetKeyDown(KeyCode.E);
        PressedRewind = Input.GetKeyDown(KeyCode.R);
        ReleasedRewind = Input.GetKeyUp(KeyCode.R);
        PressedEscape = Input.GetKeyDown(KeyCode.Escape);
    }
}
