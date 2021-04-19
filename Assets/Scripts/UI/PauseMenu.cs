using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject popup;
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

    private void PauseTime() => Time.timeScale = 0f;
    private void ResumeTime() => Time.timeScale = 1f;

    private void Pause()
    {
        isPaused = true;
        cursor.Unlock();
        menu.SetActive(true);
        PauseTime();
    }

    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        cursor.Lock();
        ResumeTime();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeTime();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ResumeTime();
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
}
