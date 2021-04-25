using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    /// <summary>
    /// In Start, so DataManager can initialize before in Awake.
    /// </summary>
    private void Start()
    {
        UpdateLevelCounts();
    }

    /// <summary>
    /// Updates the current level and the highest reached level number 
    /// if needed from the current scene's name.
    /// </summary>
    private void UpdateLevelCounts()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string number = Regex.Match(currentSceneName, @"\d+").Value;
        int currentLevelNumber = Int32.Parse(number);
        if (DataManager.instance.GetHighestLevel() < currentLevelNumber)
        {
            DataManager.instance.SetHighestLevel(currentLevelNumber);
        }
        DataManager.instance.SetCurrentLevel(currentLevelNumber);
    }

    /// <summary>
    /// Loads the given next level.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
