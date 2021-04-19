using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    public int CurrentLevel;
    public int HighestLevel;

    public GameState()
    {
        CurrentLevel = 0;
        HighestLevel = 1;
    }
}
