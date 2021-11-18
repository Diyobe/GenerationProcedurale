using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomTest;

    public enum Direction { Left = 1, Right = 2, Up = 3, Down = 4}

    List<Node> dungeon = new List<Node>();

    List<Node> lockedNodes = new List<Node>();

    Vector2 startPosition = new Vector2(0, 0);

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


    private int nbTry = 0, nbTryMax = 100;


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

        foreach (Node node in lockedNodes)
        {
            int sizeSecondary = Random.Range(4, 6);
            GenerateDungeon(sizeSecondary, node, false);
            if (creationFailed)
                break;
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

                node.Position = ChooseRandomDirection(currentNode.Position);
            }
            else
            {
                node.Type = RoomType.Normal;
                node.Position = ChooseRandomDirection(currentNode.Position);

            }

            if (i > 0 && node.Position == Vector2.zero)
            {
                if(!isMain)
                {
                    Debug.LogWarning("ECHEC CHEMIN SECONDAIRE, DESTRUCTION DU DONJON, ON RECOMMENCE !!!");
                    creationFailed = true;
                    break;
                }
                else
                {
                    i = start - 1;
                    dungeon.Clear();
                    Debug.LogWarning("DESTRUCTION DU DONGEON PRINCIPAL, ON RECOMMENCE !!!!");
                }
            }
            else
            {
                dungeon.Add(node);

                if (node.Type != RoomType.Start)
                {
                    bool isLock = false;

                    if(isMain && lockedIndex.Contains(i))
                    {
                        isLock = true;
                        lockedNodes.Add(currentNode);
                    }

                    currentNode.connections.Add(new Connection(currentNode, node, isLock));
                    node.connections.Add(new Connection(node, currentNode, isLock));
                }
                else
                {
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

    private Vector2 ChooseRandomDirection(Vector2 currentPosition)
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
}
