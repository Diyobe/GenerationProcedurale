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
            }
            else
            {
                node.Type = RoomType.Other;
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

        return true;
    }

    private Connection ChooseRandomConnection()
    {
        List<int> list = new List<int> { 1, 2, 3, 4 };
        int i = 0;

        while(i < 4)
        {
            int randomInt = Random.Range(0, list.Count);

            Direction dir = (Direction)list[randomInt];

            Connection connection = new Connection();

            Position position;

            Node node = new Node();

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
                return connection;
            }
            else
            {
                list.RemoveAt(randomInt);
            }

            i++;
        }

        return null;
    }

}
