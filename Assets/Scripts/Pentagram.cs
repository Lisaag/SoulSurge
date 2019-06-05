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
    private bool isActive = true;

	SpriteRenderer sr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;
    }

	private void Update()
	{
        if (isActive)
            sr.sprite = activatedSprite;
        else
        {
            sr.sprite = defaultSprite;
        }
    }

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.CompareTag("Player") && isActive)
		{
            isActive = false;
            LifeManager.instance.lastCheckpoint = gameObject;
            collider.GetComponent<Player>().TakeDamage(9999);
        }
	}

}
