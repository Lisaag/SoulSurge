using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

	public Player player;
	public PlayerMovement playerMovement;
	public PlayerSword playerSword;

	public void OnSpeed()
	{
		LifeManager.instance.Revive();
		playerMovement.speed += 1f;
	}

	public void OnDamage()
	{
		LifeManager.instance.Revive();
		playerSword.damage += 5;
	}

	public void OnHealth()
	{
		LifeManager.instance.Revive();
		player.maxHealth += 1;
	}

}
