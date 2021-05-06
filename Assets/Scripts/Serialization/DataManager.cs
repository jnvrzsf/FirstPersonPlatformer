using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    private GameState gameState;
    public bool isGameStateSaved => File.Exists(Paths.GameStatePath);

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
        string jsonString = File.ReadAllText(Paths.GameStatePath);
        gameState = JsonUtility.FromJson<GameState>(jsonString);
    }

    private void SaveGameState()
    {
        string jsonString = JsonUtility.ToJson(gameState, true);
        File.WriteAllText(Paths.GameStatePath, jsonString);
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
