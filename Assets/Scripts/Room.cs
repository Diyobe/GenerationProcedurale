using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Door> doors;
    public bool isStartRoom = false;
    public bool isChallengeRoom = false;
    public bool challengeFinished = false;
    public Vector2Int position = Vector2Int.zero;

    private TilemapGroup _tilemapGroup;

    public static List<Room> allRooms = new List<Room>();

    void Awake()
    {
        _tilemapGroup = GetComponentInChildren<TilemapGroup>();
        allRooms.Add(this);
        //PropsChild = get
        if (this.isChallengeRoom)
        {
            foreach (Transform child in transform)
            {
                doors.AddRange(child.GetComponentsInChildren<Door>());
                enemies.AddRange(child.GetComponentsInChildren<Enemy>());
            }
        }
    }

    private void OnDestroy()
    {
        allRooms.Remove(this);
    }

    void Start()
    {
        if (isStartRoom)
        {
            OnEnterRoom();
        }

        if (this.isChallengeRoom)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.onEnemyDead += RemoveEnemyFromList;
            }
            //CloseDoors();
        }
    }

    public void OnEnterRoom()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        Bounds cameraBounds = GetWorldRoomBounds();
        cameraFollow.SetBounds(cameraBounds);
        Player.Instance.EnterRoom(this);

    }


    public Bounds GetLocalRoomBounds()
    {
        Bounds roomBounds = new Bounds(Vector3.zero, Vector3.zero);
        if (_tilemapGroup == null)
            return roomBounds;

        foreach (STETilemap tilemap in _tilemapGroup.Tilemaps)
        {
            Bounds bounds = tilemap.MapBounds;
            roomBounds.Encapsulate(bounds);
        }
        return roomBounds;
    }

    public Bounds GetWorldRoomBounds()
    {
        Bounds result = GetLocalRoomBounds();
        result.center += transform.position;
        return result;
    }

    public bool Contains(Vector3 position)
    {
        position.z = 0;
        return (GetWorldRoomBounds().Contains(position));
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        Debug.Log("Enemy Removed");
        if (enemy != null)
        {
            if (enemies.Count <= 1)
                OpenDoors();
            else
            {
                enemy.onEnemyDead -= RemoveEnemyFromList;
                enemies.Remove(enemy);

            }
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            challengeFinished = true;
            door.OpenDoor();
            Debug.Log("ChallengeDone");
        }

    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.CloseDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !challengeFinished)
        {
            Debug.Log("Enter in challenge Room");
            CloseDoors();
        }
    }
}
