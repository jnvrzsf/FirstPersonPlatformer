using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject crosshair;
    private bool isCarrying;

    private void Awake()
    {
        Lock();
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!isCarrying) ShowCrosshair();
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!isCarrying) HideCrosshair();
    }

    public void HideOnCarry()
    {
        HideCrosshair();
        isCarrying = true;
    }

    public void ShowOnDrop()
    {
        ShowCrosshair();
        isCarrying = false;
    }

    private void HideCrosshair() => crosshair.SetActive(false);
    private void ShowCrosshair() => crosshair.SetActive(true);
}
