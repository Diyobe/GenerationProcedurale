using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Position(Position pos)
    {
        x = pos.x;
        y = pos.y;
    }

    public bool Equals(Position pos)
    {
        if (x == pos.x && y == pos.y)
            return true;
        return false;
    }
}
