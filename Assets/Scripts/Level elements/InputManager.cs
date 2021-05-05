using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsIdle => Mathf.Approximately(Horizontal, 0) && Mathf.Approximately(Vertical, 0);
    public bool PressedJump { get; private set; }
    public bool isPressingShift { get; private set; }
    public bool PressedAction { get; private set; }
    public bool isPressingRewind { get; private set; }
    public bool PressedRewind { get; private set; }
    public bool PressedEscape { get; private set; }

    void Update()
    {
        MouseX = Input.GetAxis(Constants.MouseX);
        MouseY = Input.GetAxis(Constants.MouseY);
        Horizontal = Input.GetAxisRaw(Constants.Horizontal);
        Vertical = Input.GetAxisRaw(Constants.Vertical);
        PressedJump = Input.GetKeyDown(KeyCode.Space);
        isPressingShift = Input.GetKey(KeyCode.LeftShift);
        PressedAction = Input.GetMouseButtonDown(0);
        PressedRewind = Input.GetMouseButtonDown(1);
        isPressingRewind = Input.GetMouseButton(1);
        PressedEscape = Input.GetKeyDown(KeyCode.Escape);
    }
}
