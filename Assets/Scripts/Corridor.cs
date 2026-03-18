using UnityEngine;

public enum CorridorType { Horizontal, Vertical }

public enum CorridorSize
{
    Small,Large,Boss
}

public struct Corridor
{
    public Vector2Int start;
    public CorridorType type;
    public CorridorSize size;
}