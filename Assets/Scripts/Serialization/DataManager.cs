﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    private GameState gameState;
    private string path;
    public bool isGameStateSaved => File.Exists(path);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        path = Application.persistentDataPath + "/GameState.json";
        if (isGameStateSaved)
        {
            LoadGameState();
        }
        else
        {
            gameState = new GameState();
        }
    }

    private void LoadGameState()
    {
        string jsonString = File.ReadAllText(path);
        gameState = JsonUtility.FromJson<GameState>(jsonString);
    }

    private void SaveGameState()
    {
        string jsonString = JsonUtility.ToJson(gameState, true);
        File.WriteAllText(path, jsonString);
    }

    public int GetHighestLevel()
    {
        return gameState.HighestLevel;
    }

    public void SetHighestLevel(int level)
    {
        gameState.HighestLevel = level;
    }

    public int GetCurrentLevel()
    {
        return gameState.CurrentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        gameState.CurrentLevel = level;
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }
}
