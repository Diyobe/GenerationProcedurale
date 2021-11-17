using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public enum Direction { Left = 1, Right = 2, Up = 3, Down = 4}

    List<Node> dungeon = new List<Node>();

    Position currentPosition = new Position(50, 50);

    [SerializeField]
    private int mainSizeMin, mainSizeMax;

    [SerializeField]
    private int secondarySize;


    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        int size = Random.Range(mainSizeMin, mainSizeMax);

        bool flag = false;

        int counter = 0;

        for(int i = 0; i < size; i++)
        {
            Node node = new Node();

            if(i == 0)
            {
                node.Type = RoomType.Start;
                node.Position = currentPosition;
            }
            else if(i == size - 1)
            {
                node.Type = RoomType.End;
                node.Position = ChooseRandomDirection();
            }
            else
            {
                node.Type = RoomType.Other;
                node.Position = ChooseRandomDirection();

            }

            if (node.Position.Equals(new Position(-1, -1)))
            {
                Debug.Log("DESTRUCTION DU DUNGON, ON RECOMMENCE !!!!\n");
                i = 0;
                dungeon.Clear();
            }
            else
            {
                dungeon.Add(node);

                if (node.Type != RoomType.Start)
                {
                    node.connections.Add(new Connection(dungeon[i - 1], dungeon[i]));

                    Debug.Log("CREATION D'UNE SALLE EN POSITION : " + node.Position.x + ", " + node.Position.y 
                                + " CONNECTEE A : " + node.connections[0].Nodes[0].Position.x + ", " + node.connections[0].Nodes[0].Position.y);
                    Debug.Log("TYPE : " + node.Type);
                }
                else
                {
                    Debug.Log("CREATION D'UNE SALLE EN POSITION : " + node.Position.x + ", " + node.Position.y);
                    Debug.Log("TYPE : " + node.Type);
                }
            }



            counter++;
            if (counter > 1000 && !flag)
                break;
        }


        if(!flag)
        {
            Debug.LogWarning("ECHEC DE LA GENERATION");
        }
    }

    private bool CheckPosition(Position pos)
    {
        foreach(Node node in dungeon)
        {
            if (node.Position.Equals(pos))
                return false;
        }

        return true;
    }

    private Position ChooseRandomDirection()
    {
        List<int> list = new List<int> { 1, 2, 3, 4 };
        int i = 0;

        while(i < 4)
        {
            int randomInt = Random.Range(0, list.Count);

            Direction dir = (Direction)list[randomInt];

            Position position;

            if (dir == Direction.Left)
            {
                position = new Position(currentPosition);
                position.x -= 1;
            }
            else if (dir == Direction.Right)
            {
                position = new Position(currentPosition);
                position.x += 1;
            }
            else if (dir == Direction.Up)
            {
                position = new Position(currentPosition);
                position.y += 1;
            }
            else
            {
                position = new Position(currentPosition);
                position.y -= 1;
            }


            if (CheckPosition(position))
            {
                currentPosition = position;
                return position;
            }
            else
            {
                list.RemoveAt(randomInt);
            }

            i++;
        }

        return new Position(-1, -1);
    }
}
