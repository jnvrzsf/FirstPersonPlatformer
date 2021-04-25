using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject rewindPanel;
    [SerializeField] private CursorController cursor;
    private PlayerState player;
    private InputManager input;
    public bool isPaused { get; private set; }
    public bool isDisplaying => menu.activeSelf || rewindPanel.activeSelf;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        player = FindObjectOfType<PlayerState>();
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

        if (player.isDead)
        {
            ShowRewindPanel();
        }
        else
        {
            HideRewindPanel();
        }
    }

    private void Pause()
    {
        isPaused = true;
        menu.SetActive(true);
        cursor.Unlock();
    }

    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        cursor.Lock();
    }

    public void Restart()
    {
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
    }

    public void HidePopup()
    {
        popup.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void ShowRewindPanel()
    {
        rewindPanel.SetActive(true);
    }

    private void HideRewindPanel()
    {
        rewindPanel.SetActive(false);
    }
}
