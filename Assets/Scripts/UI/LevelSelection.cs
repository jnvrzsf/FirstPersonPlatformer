using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    private Button[] levelButtons;

    public void UpdateLevelAvailability(int maxLevelNumber)
    {
        if (levelButtons == null)
        {
            levelButtons = GetComponentsInChildren<Button>();
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i <= maxLevelNumber - 1)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
