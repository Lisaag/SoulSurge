using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDuplicates : MonoBehaviour
{
    RoomTemplates roomTemplates;
    private bool hasSpawned;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);   
        if (other.gameObject.CompareTag("DuplicateChecker"))
        {
            if (!other.transform.parent.CompareTag("SpawnRoom"))
            {
                //  transform.parent.gameObject.SetActive(false);
                Debug.Log("Removed duplicate");
            }
        }
    }

    private void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void Update()
    {

    }
}
