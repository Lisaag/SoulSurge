using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

	private void Awake()
	{
		instance = this;
	}

	public GameObject currentRoom;
	public bool isLoading = false;

	public Rigidbody2D playerRB;

	public Vector2 playerPosRight;
	public Vector2 playerPosLeft;
	public Vector2 playerPosUp;
	public Vector2 playerPosDown;

	public GameObject[] rooms;

	public GameObject shop;

	private void Start()
	{
		StartCoroutine(LoadAllRooms());
	}

	IEnumerator LoadAllRooms()
	{
		if (!SceneManager.GetSceneByName("Rooms").isLoaded)
		{
			SceneManager.LoadScene("Rooms", LoadSceneMode.Additive);
		}

		while (Rooms.instance == null)
			yield return 0;

		rooms = Rooms.instance.rooms;
		currentRoom = rooms[0];
		currentRoom.SetActive(true);
	}

	public void ResetAllRooms(string roomName)
	{
		StartCoroutine(ResetAllRoomsCo(roomName));
	}

	IEnumerator ResetAllRoomsCo(string roomName)
	{
		AsyncOperation unload = SceneManager.UnloadSceneAsync("Rooms");

		while (!unload.isDone)
		{
			yield return 0;
		}

		AsyncOperation load = SceneManager.LoadSceneAsync("Rooms", LoadSceneMode.Additive);

		while (!load.isDone)
		{
			yield return 0;
		}

		rooms = Rooms.instance.rooms;

		GameObject room = null;

		foreach (GameObject r in rooms)
		{
			if (r.name == roomName)
			{
				room = r;
				continue;
			}
		}

		LoadNewRoom(room, DoorDirection.NONE);
	}

	public void LoadNewRoom(GameObject room, DoorDirection direction)
	{
		if (isLoading)
			return;

		isLoading = true;
		Debug.Log("Loading " + room.name);

		StartCoroutine(LoadRoom(room, direction));
	}

	IEnumerator LoadRoom(GameObject room, DoorDirection direction)
	{
		Fader.instance.FadeOut();

		yield return new WaitForSeconds(0.3f);

		//AsyncOperation unload = SceneManager.UnloadSceneAsync(currentRoom);

		GameObject[] effects = GameObject.FindGameObjectsWithTag("Effects");
		for (int i = 0; i < effects.Length; i++)
		{
			Destroy(effects[i]);
		}

		if (currentRoom != null)
			currentRoom.SetActive(false);

		foreach(GameObject r in rooms)
		{
			if(r.name == room.name)
			{
				r.SetActive(true);
				currentRoom = r;
			}
		}

		shop.SetActive(false);

		if (direction == DoorDirection.RIGHT)
			playerRB.position = playerPosLeft;
		if (direction == DoorDirection.LEFT)
			playerRB.position = playerPosRight;
		if (direction == DoorDirection.UP)
			playerRB.position = playerPosDown;
		if (direction == DoorDirection.DOWN)
			playerRB.position = playerPosUp;

		Fader.instance.FadeIn();

		yield return new WaitForSeconds(.3f);

		isLoading = false;
	}

}
