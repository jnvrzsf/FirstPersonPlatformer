using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private LevelSelection levels;
    [SerializeField] private TMP_Text playText;

    private void Awake()
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

    public void Play()
    {
        string levelName = "Level" + DataManager.instance.GetHighestLevel().ToString();
        LoadLevel(levelName);
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

    public void LoadLevel(string levelName)
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

    public void Quit()
    {
        Application.Quit();
    }
}
