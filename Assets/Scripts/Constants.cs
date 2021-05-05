using UnityEngine;

public static class Constants
{
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";

    public const string Play = "Play";
    public const string Continue = "Continue";
    public const string Level = "Level";
    public const string MainMenu = "MainMenu";

    public const string DissolvePercentage = "DissolvePercentage";
}

public static class Tags
{
    public const string DestructiveField = "DestructiveField";
    public const string DeathZone = "DeathZone";
    public const string Moving = "Moving";
    public const string Player = "Player";
}

public static class Layers
{
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int Ground = LayerMask.NameToLayer("Ground");
    public static readonly int Pickupable = LayerMask.NameToLayer("Pickupable");
    public static readonly int PickedUp = LayerMask.NameToLayer("PickedUp");
}

public static class Paths
{
    public static readonly string GameStatePath = Application.persistentDataPath + "/GameState.json";
}
