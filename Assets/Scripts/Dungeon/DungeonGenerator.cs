using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomTest;

    public enum Direction { Left = 1, Right = 2, Up = 3, Down = 4}

    List<Node> dungeon = new List<Node>();

    Vector2 currentPosition = new Vector2(0, 0);

    [SerializeField]
    private int mainSizeMin, mainSizeMax;

    [SerializeField]
    private float roomDimensionX = 50, roomDimensionY = 50;

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

            if (i > 0 && node.Position == Vector2.zero)
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
                    dungeon[i - 1].connections.Add(new Connection(dungeon[i - 1], dungeon[i]));

                    node.connections.Add(new Connection(dungeon[i], dungeon[i - 1]));

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

            DrawDungeon();
        }


        if(!flag)
        {
            Debug.LogWarning("ECHEC DE LA GENERATION");
        }
    }

    private bool CheckPosition(Vector2 pos)
    {
        foreach(Node node in dungeon)
        {
            if (node.Position == pos)
                return false;
        }

        return true;
    }

    private Vector2 ChooseRandomDirection()
    {
        List<int> list = new List<int> { 1, 2, 3, 4 };
        int i = 0;

        while(i < 4)
        {
            int randomInt = Random.Range(0, list.Count);

            Direction dir = (Direction)list[randomInt];

            Vector2 position = currentPosition; ;

            if (dir == Direction.Left)
            {
                position.x -= roomDimensionX;
            }
            else if (dir == Direction.Right)
            {
                position.x += roomDimensionX;
            }
            else if (dir == Direction.Up)
            {
                position.y += roomDimensionY;
            }
            else
            {
                position.y -= roomDimensionY;
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

        return Vector2.zero;
    }


    public void DrawDungeon()
    {
        foreach(Node node in dungeon)
        {
            // Room 

            GameObject go = Instantiate(roomTest, transform);
            go.transform.position = new Vector2(node.Position.x, node.Position.y);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            if (node.Type == RoomType.Start)
                sr.color = Color.green;
            else if (node.Type == RoomType.Other)
                sr.color = Color.grey;
            else
                sr.color = Color.red;

            // Connections

            LineRenderer line = go.AddComponent<LineRenderer>();
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;

            for (int i = 0; i < node.connections.Count; i++)
            {
                line.SetPosition(0, node.connections[i].Nodes[0].Position);
                line.SetPosition(1, node.connections[i].Nodes[1].Position);
            }

        }
    }
}
