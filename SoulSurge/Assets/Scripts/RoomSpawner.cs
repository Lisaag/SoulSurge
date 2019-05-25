using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum openingDirection
    {
        BOTTOM, TOP, LEFT, RIGHT
    };

    public openingDirection hasToSpawn;
    private int rand;
    public bool spawned;

    public Vector3 offset;

    private RoomTemplates roomTemplates;

    // Start is called before the first frame update
    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (!spawned)
        {
            switch (hasToSpawn)
            {
                case openingDirection.BOTTOM:
                    // Needs to spawn a room with a BOTTOM door
                    rand = Random.Range(0, roomTemplates.bottomRooms.Length);
                    Instantiate(roomTemplates.bottomRooms[rand], transform.position + offset, Quaternion.identity);
                    spawned = true;
                    break;
                case openingDirection.TOP:
                    // Needs to spawn a room with a TOP door
                    rand = Random.Range(0, roomTemplates.topRooms.Length);
                    Instantiate(roomTemplates.topRooms[rand], transform.position + offset, Quaternion.identity);
                    spawned = true;
                    break;
                case openingDirection.LEFT:
                    // Needs to spawn a room with a LEFT door
                    rand = Random.Range(0, roomTemplates.leftRooms.Length);
                    Instantiate(roomTemplates.leftRooms[rand], transform.position + offset, Quaternion.identity);
                    spawned = true;
                    break;
                case openingDirection.RIGHT:
                    // Needs to spawn a room with a RIGHT door
                    rand = Random.Range(0, roomTemplates.rightRooms.Length);
                    Instantiate(roomTemplates.rightRooms[rand], transform.position + offset, Quaternion.identity);
                    spawned = true;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spawnpoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //  Instantiate(roomTemplates.closedRoom, transform.position + offset, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
