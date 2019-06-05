using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTimer;
    private bool canSpawnBoss = false;
    private bool spawnedBoss = false;
    private int counter;
    public GameObject boss;

    // Update is called once per frame
    void Update()
    {
        if (waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;
        }
        else
        {
            if (!canSpawnBoss)
            {
                canSpawnBoss = true;
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] == null)
            {
                rooms.RemoveAt(i);
            }
        }

        if (canSpawnBoss)
        {
            if (!spawnedBoss)
            {
                Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
                spawnedBoss = true;
                Debug.Log("Spawned boss at " + rooms[rooms.Count - 1].gameObject.name);
                rooms[rooms.Count - 1].tag = "EndingRoom" ;
            }
        }
    }
}
