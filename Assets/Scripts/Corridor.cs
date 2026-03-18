using UnityEngine;

public enum CorridorType
{
    Horizontal,
    Vertical
}

public struct Corridor
{
    public Vector3Int start;
    public Vector3Int end;
    public int width;
    public CorridorType type;
    public int roomId;
    public bool isBonus;
}