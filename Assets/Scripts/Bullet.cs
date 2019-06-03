using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public string killTag = "Player";
	public int damage = 1;

	public GameObject effect;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag(killTag))
		{
			collision.collider.GetComponent<Player>().TakeDamage(damage);
		}

		Instantiate(effect, transform.position, Quaternion.identity, transform.parent);
		Destroy(gameObject);
	}

}
