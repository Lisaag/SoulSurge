using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
	public static LifeManager instance;

	private void Awake()
	{
		instance = this;
	}

	public int lives = 3;

	public GameObject shop;
	public GameObject gameOver;
	public Player player;

	public void LooseLife()
	{
		lives--;

		if (lives <= 0)
		{
			GameOver();
		} else
		{
			//Debug.Log("Thank you for your sacrifice. What power would you like in return?");
			shop.SetActive(true);
		}
	}

	public void Revive()
	{
		// Return to last spawnpoint
		GameManager.instance.ResetAllRooms(Pentagram.ActiveRoomName);
		//GameManager.instance.LoadNewRoom(Pentagram.activePentagram.transform.parent.gameObject, DoorDirection.NONE);
		player.transform.position = Pentagram.ActivePosition;
		player.Revive();
	}

	public void GainLife()
	{
		lives++;
		Debug.Log("Congratulations! You recovered a soul fragment!");
	}

	void GameOver()
	{
		gameOver.SetActive(true);
	}

}
