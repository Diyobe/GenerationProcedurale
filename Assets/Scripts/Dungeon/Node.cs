using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    End,
    Key,
    Lock,
    Special,
    Normal,
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None,
}
public enum Difficulty
{
    Easy,
    Hard,
}

public class Node
{
    Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    RoomType type;
    public RoomType Type
    {
        get { return type; }
        set { type = value; }
    }
    
    public List<Direction> doorsDirection = new List<Direction>();
    public Direction lockDirection;

    Difficulty difficulty;
    public Difficulty Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    public List<Connection> connections = new List<Connection>();
}
