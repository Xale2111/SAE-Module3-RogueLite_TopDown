using UnityEngine;

public enum CorridorType
{
    Horizontal,
    Vertical
}

public struct Corridor
{
    public Vector2Int start;
    public Vector2Int end;
    public int width;
    public CorridorType type;
    public bool isBonus;
}