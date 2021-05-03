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
        AudioManager.instance.PlayOneShot(AudioType.Pause);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
        AudioManager.instance.PlayOneShot(AudioType.Popup);
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

    public void OnPointerEnter() => AudioManager.instance.PlayOneShot(AudioType.Hover);
    public void OnPointerClick() =>  AudioManager.instance.PlayOneShot(AudioType.Click);
}
