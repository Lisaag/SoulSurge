using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DoorDirection { RIGHT, LEFT, UP, DOWN, NONE };

public class Door : MonoBehaviour
{

	public GameObject room;

	public DoorDirection direction;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
				GameManager.instance.LoadNewRoom(room, direction);
		}
	}

}
