using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeFloor : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Door> doors;

    bool challengeCompleted = false;

    //Start is called before the first frame update
    void Start()
    {
        Enemy.onEnemyDead += RemoveEnemyFromList;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count <= 0)
        {
            if (!challengeCompleted)
            {
                OpenDoors();
                challengeCompleted = true;
            }
        }
        //else
        //{
        //    foreach (enemy enemy in enemies)
        //    {
        //        if (enemy == null)
        //            enemies.remove(enemy);
        //    }
        //}
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        Debug.Log("Enemy Removed");
        if (enemy != null)
        {
            if (enemies.Count <= 0)
            {
                OpenDoors();
                challengeCompleted = true;
            }
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
}
