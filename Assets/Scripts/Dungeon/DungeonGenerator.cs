using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomTest;

    List<Node> dungeon = new List<Node>();

    List<Node> lockedNodes = new List<Node>();

    Vector2 startPosition = new Vector2(0, 0);

    [SerializeField]
    private List<Room> roomList = new List<Room>();

    [SerializeField]
    private int mainSizeMin = 10, mainSizeMax = 14;
    private int mainSize = 0;

    [SerializeField]
    private int secondarySizeMin = 4, secondarySizeMax = 6;

    [SerializeField]
    private float roomDimensionX = 2, roomDimensionY = 2;

    [SerializeField]
    private int minimumForLock = 5;

    private int nbLock = 2;
    private List<int> lockedIndex = new List<int>();

    private bool creationFailed = false;


    private int nbTry = 0, nbTryMax = 10;


    private void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        nbTry++;

        dungeon.Clear();
        lockedNodes.Clear();

        mainSize = Random.Range(mainSizeMin, mainSizeMax);
        nbLock = Random.Range(2, 4);

        SelectLockedIndexes(mainSize);

        GenerateDungeon(mainSize, null, true);

        if(!creationFailed)
        {
            foreach (Node node in lockedNodes)
            {
                int sizeSecondary = Random.Range(4, 6);
                GenerateDungeon(sizeSecondary, node, false);
                if (creationFailed)
                    break;
            }
        }

        if(creationFailed)
        {
            creationFailed = false;
            if(nbTry <= nbTryMax)
                CreateDungeon();
            else
            {
                Debug.LogError("ECHEC DE LA GENERATION");
            }
        }
        else
        {
            DrawDungeonGraph();
            DrawDungeonRooms();
            Debug.LogWarning("DONJON GENERE AVEC SUCCES :)");
        }
    }

    private void GenerateDungeon(int size, Node currentNode, bool isMain)
    {
        int start = 0;
        if (!isMain)
            start = 1;

        for (int i = start; i < size; i++)
        {
            Node node = new Node();
            bool isLock = false;

            if (i == 0)
            {
                node.Type = RoomType.Start;
                node.Position = startPosition;
            }
            else if (i == size - 1)
            {
                if(isMain)
                    node.Type = RoomType.End; 
                else
                    node.Type = RoomType.Key;

                node.Position = ChooseRandomDirection(currentNode, node);

            }
            else
            {
                node.Type = RoomType.Normal;

                if (isMain && lockedIndex.Contains(i))
                {
                    currentNode.Type = RoomType.Lock;
                    lockedNodes.Add(currentNode);
                    isLock = true;
                }

                node.Position = ChooseRandomDirection(currentNode, node);

                
            }

            if (i > 0 && node.Position == Vector2.zero)
            {
                creationFailed = true;

                if (!isMain)
                    Debug.LogWarning("ECHEC CHEMIN SECONDAIRE, DESTRUCTION DU DONJON, ON RECOMMENCE !!!");
                else
                    Debug.LogWarning("DESTRUCTION DU DONGEON PRINCIPAL, ON RECOMMENCE !!!!");

                break;
            }
            else
            {
                dungeon.Add(node);

                if (node.Type != RoomType.Start)
                {
                    currentNode.connections.Add(new Connection(currentNode, node, isLock));
                    node.connections.Add(new Connection(node, currentNode, isLock));
                }
            }

            currentNode = node;
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

    private Vector2 ChooseRandomDirection(Node currentNode, Node node)
    {
        List<int> list = new List<int> { 0, 1, 2, 3 };
        int i = 0;

        while(i < 4)
        {
            int randomInt = Random.Range(0, list.Count);

            Direction dir = (Direction)list[randomInt];

            Vector2 position = currentNode.Position;

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
                //currentNode.Position = position;
                currentNode.doorsDirection.Add(dir);
                node.doorsDirection.Add(RevertDirection(dir));

                if(node.Type == RoomType.Lock)
                {
                    node.lockDirection = RevertDirection(dir);
                }

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

    private void SelectLockedIndexes(int size)
    {
        // lock entre minimum & size -1 => En placer nbLock
        lockedIndex.Clear();

        for(int i = 0; i < nbLock; i++)
        {
            int index = Random.Range(minimumForLock, size);
            if (lockedIndex.Contains(index))
                i--;
            else
                lockedIndex.Add(index);
        }
    }

    private Direction RevertDirection(Direction dir)
    {

        if (dir == Direction.Left)
        {
            return Direction.Right;
        }
        else if (dir == Direction.Right)
        {
            return Direction.Left;
        }
        else if (dir == Direction.Up)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Up;
        }
    }


    public void DrawDungeonGraph()
    {
        foreach(Node node in dungeon)
        {
            // Room 

            GameObject go = Instantiate(roomTest, transform);
            go.transform.position = new Vector2(node.Position.x, node.Position.y);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            if (node.Type == RoomType.Start)
                sr.color = Color.green;
            else if (node.Type == RoomType.Normal)
                sr.color = Color.grey;
            else if (node.Type == RoomType.Key)
                sr.color = Color.black;
            else
                sr.color = Color.red;

            // Connections


            foreach(Connection connect in node.connections)
            {
                GameObject lineObject = new GameObject();
                lineObject.transform.parent = go.transform;

                LineRenderer line = lineObject.AddComponent<LineRenderer>();
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                line.material = new Material(Shader.Find("Sprites/Default"));

                if(connect.IsLocked)
                {
                    line.startColor = Color.red;
                    line.endColor = Color.red;
                }
                else
                {
                    line.startColor = Color.green;
                    line.endColor = Color.green;
                }

                line.SetPosition(0, connect.Nodes[0].Position);
                line.SetPosition(1, connect.Nodes[1].Position);

            }
        }
    }

    public void DrawDungeonRooms()
    {
        foreach (Node node in dungeon)
        {
            List<Room> validRooms = new List<Room>();
            foreach (Room room in roomList)
            {
                bool valid = true;

                Debug.Log("NODE TYPE = " + node.Type + " | ROOM TYPE = " + room.Type);
                Debug.Log("NODE CONNECT = " + node.doorsDirection.Count + " | ROOM CONNECT = " + room.doorsDirection.Count);

                if (room.Type == node.Type && room.doorsDirection.Count == node.connections.Count)
                {
                    for (int i = 0; i < room.doorsDirection.Count; i++)
                    {
                        if (!node.doorsDirection.Contains(room.doorsDirection[i]))
                        {
                            for(int j = 0; j < node.doorsDirection.Count; j++)
                            {
                                Debug.Log(j + " = " + node.doorsDirection[j]);
                            }
                            Debug.Log("NE CONTIENT PAS : " + room.doorsDirection[i]);


                            valid = false;
                            break;
                        }
                    }
                }
                else
                {
                    valid = false;
                }

                if (valid)
                {
                    Debug.Log("ADD ROOM");
                    validRooms.Add(room);
                }
                else
                    Debug.Log("C'EST MORT");
            }

            if(validRooms.Count > 0)
            {
                int index = Random.Range(0, validRooms.Count);
                GameObject go = Instantiate(validRooms[index].gameObject);
                go.transform.position = new Vector2(node.Position.x, node.Position.y);
            }
        }
    }
}
