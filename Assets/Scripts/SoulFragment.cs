using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFragment : MonoBehaviour
{

	public static List<string> roomsPickedUp;

	public GameObject effect;

	private void Start()
	{
		if (roomsPickedUp == null)
			roomsPickedUp = new List<string>();

		for (int i = 0; i < roomsPickedUp.Count; i++)
		{
			if (transform.parent.name == roomsPickedUp[i])
				Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			Instantiate(effect, transform.position, Quaternion.identity);
			LifeManager.instance.GainLife();
			Destroy(gameObject);
		}
	}

}
