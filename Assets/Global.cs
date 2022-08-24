using UnityEngine;

public struct Layers
{
    public static readonly int Weapon = LayerMask.NameToLayer("Weapon");
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
}

public struct Tags
{
    public const string Attachable = "Attachable";
    public const string Camera = "Camera";
}

public struct Levels
{
    public const int MainMenu = 0;
    public const int Main = 1;
}
