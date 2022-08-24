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
}