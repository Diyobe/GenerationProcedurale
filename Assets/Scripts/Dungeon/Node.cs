using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    End,
    Other,
}
public enum Difficulty
{
    Easy,
    Hard,
}

public class Node : MonoBehaviour
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

    Difficulty difficulty;
    public Difficulty Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    public List<Connection> connections = new List<Connection>();
}
