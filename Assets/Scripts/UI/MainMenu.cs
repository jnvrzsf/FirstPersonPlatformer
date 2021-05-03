using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private LevelSelection levels;
    [SerializeField] private TMP_Text playText;

    private void Start()
    {
        UnlockCursor();

        if (DataManager.instance.GetCurrentLevel() == 0)
        {
            playText.text = "Play";
        }
        else
        {
            playText.text = "Continue";
        }
    }

    /// <summary>
    /// Shows cursor in case it was locked and hidden in a previous scene.
    /// </summary>
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPlayButtonClick()
    {
        int currentLevel = DataManager.instance.GetCurrentLevel();
        int levelToLoad = currentLevel == 0 ? 1 : currentLevel;
        string levelName = "Level" + levelToLoad.ToString();
        SceneManager.LoadScene(levelName);
    }

    public void OnLevelsButtonClick()
    {
        if (levels.gameObject.activeSelf)
        {
            HideLevels();
        }
        else
        {
            ShowLevels();
        }
    }

    public void ShowLevels()
    {
        levels.UpdateLevelAvailability(DataManager.instance.GetHighestLevel());
        levels.gameObject.SetActive(true);
    }

    public void HideLevels()
    {
        levels.gameObject.SetActive(false);
    }

    public void OnLevelButtonClick(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
    }

    public void HidePopup()
    {
        popup.SetActive(false);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void PlayClickSound() => AudioManager.instance.PlayOneShot(AudioType.Click);

    public void PlayHoverSound(Button b)
    {
        if (b.IsInteractable())
        {
            AudioManager.instance.PlayOneShot(AudioType.Hover);
        }
    }
}
