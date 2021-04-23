﻿using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// In the Start instead of the Awake, so DataManager can initialize before.
    /// </summary>
    private void Start()
    {
        UpdateLevelCounts();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
