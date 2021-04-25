using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private PauseMenu menu;
    private PlayerState player;
    private bool isPlayerDead => player.isDead;
    private bool isGamePaused => menu.isPaused;

    private void Awake()
    {
        menu = FindObjectOfType<PauseMenu>();
        player = FindObjectOfType<PlayerState>();
    }

    private void Update()
    {
        if (!isPlayerDead && !isGamePaused)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
