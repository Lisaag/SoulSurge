using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagram : MonoBehaviour
{

	public static Vector2 ActivePosition;
	public static string ActiveRoomName;

	public Sprite defaultSprite;
	public Sprite activatedSprite;

	public bool isActiveByDefault = false;

	SpriteRenderer sr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		if(isActiveByDefault)
		{
			Activate();
		}
	}

	private void Update()
	{
		if(ActiveRoomName == transform.parent.name)
		{
			sr.sprite = activatedSprite;
		} else
		{
			sr.sprite = defaultSprite;
		}
	}

	void Activate()
	{
		ActiveRoomName = transform.parent.name;
		ActivePosition = transform.position;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.CompareTag("Player") && ActiveRoomName != transform.parent.name)
		{
			Activate();
			collider.GetComponent<Player>().TakeDamage(9999);
		}
	}

}
