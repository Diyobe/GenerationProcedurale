using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] List<Enemy> enemies;
    [SerializeField] Door[] doors;
    public bool isStartRoom = false;
    public bool isChallengeRoom = false;
    public Vector2Int position = Vector2Int.zero;

    private TilemapGroup _tilemapGroup;

    public static List<Room> allRooms = new List<Room>();

    void Awake()
    {
        _tilemapGroup = GetComponentInChildren<TilemapGroup>();
        allRooms.Add(this);
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
    }

    public void OnEnterRoom()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        Bounds cameraBounds = GetWorldRoomBounds();
        cameraFollow.SetBounds(cameraBounds);
        Player.Instance.EnterRoom(this);
        if (this.isChallengeRoom)
        {
            Enemy.onEnemyDead += RemoveEnemyFromList;
            //CloseDoors();
        }
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
            if (enemies.Count <= 0)
                OpenDoors();
            else
                enemies.Remove(enemy);
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.OpenDoor();
        }
        Enemy.onEnemyDead -= RemoveEnemyFromList;
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.CloseDoor();
        }
    }
}
