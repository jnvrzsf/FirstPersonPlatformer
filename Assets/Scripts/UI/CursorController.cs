using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] GameObject cursor;

    private void Awake()
    {
        Lock();
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(true);
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        cursor.SetActive(false);
    }
}
