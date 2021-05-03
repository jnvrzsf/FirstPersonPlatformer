using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private PauseMenu menu;
    private PlayerState player;
    private bool isPlayerDead => player.isDead;
    public bool isGamePaused => menu.isPaused;

    private bool isTimePaused;
    public event Action TimePaused;
    public event Action TimeResumed;

    private void Awake()
    {
        menu = FindObjectOfType<PauseMenu>();
        player = FindObjectOfType<PlayerState>();
    }

    private void Update()
    {
        if (!isPlayerDead && !isGamePaused)
        {
            if (isTimePaused) TimeResumed?.Invoke();
            isTimePaused = false;
        }
        else
        {
            if (!isTimePaused) TimePaused?.Invoke();
            isTimePaused = true;
        }

        Time.timeScale = isTimePaused ? 0f : 1f;
    }
}
