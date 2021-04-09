using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject menu;
    [SerializeField] private CursorController cursor;
    private InputManager input;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        menu.SetActive(false);
    }

    private void Update()
    {
        if (input.PressedEscape)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            } 
        }
    }

    private void Pause()
    {
        isPaused = true;
        cursor.Unlock();
        menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        cursor.Lock();
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
