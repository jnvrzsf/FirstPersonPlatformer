using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Carrier carrier;
    private Rewinder rewinder;
    private PauseMenu menu;
    public GameObject crosshair;
    private bool isCarrying => carrier.isCarrying;
    private bool isRewinding => rewinder.isRewinding;
    private bool isUIDisplaying => menu.isDisplaying;

    private void Awake()
    {
        carrier = FindObjectOfType<Carrier>();
        rewinder = FindObjectOfType<Rewinder>();
        menu = GetComponent<PauseMenu>();
        Lock();
    }

    private void Update()
    {
        if (!isCarrying && !isRewinding && !isUIDisplaying)
        {
            ShowCrosshair();
        }
        else
        {
            HideCrosshair();
        }
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCrosshair() => crosshair.SetActive(false);
    public void ShowCrosshair() => crosshair.SetActive(true);
}
