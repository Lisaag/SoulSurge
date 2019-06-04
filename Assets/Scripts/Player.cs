using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public int maxHealth = 3;
	int health;

	public GameObject bloodEffect;

	bool isDead = false;
	bool isInvincible = false;

	SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		health = maxHealth;
	}

	public void TakeDamage (int dmg)
	{
		if (dmg < 10 && isInvincible)
		{
			return;
		}

		health -= dmg;

		if (health <= 0)
		{
			Die();
		} else
		{
			StartCoroutine(DamageAnimation());
		}
	}

	IEnumerator DamageAnimation()
	{
		isInvincible = true;

		Color c = sr.color;
		c.a = .1f;
		sr.color = c;

		yield return new WaitForSeconds(.1f);

		c.a = 1f;
		sr.color = c;

		yield return new WaitForSeconds(.1f);

		c.a = .1f;
		sr.color = c;

		yield return new WaitForSeconds(.1f);

		c.a = 1f;
		sr.color = c;

		yield return new WaitForSeconds(.1f);

		c.a = .1f;
		sr.color = c;

		yield return new WaitForSeconds(.1f);

		c.a = 1f;
		sr.color = c;

		isInvincible = false;

	}

	IEnumerator DieAnimation()
	{
		Color c = sr.color;
		c.a = 1f;
		sr.color = c;
		while(c.a > 0f)
		{
			c.a -= Time.deltaTime * 2f;
			sr.color = c;
			yield return 0;
		}
		c.a = 0f;
		sr.color = c;
	}

	void Die()
	{
		if (isDead)
			return;

		LifeManager.instance.LooseLife();
		Instantiate(bloodEffect, transform.position, Quaternion.identity);

		StopAllCoroutines();

		StartCoroutine(DieAnimation());

		GetComponent<PlayerMovement>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<PlayerSword>().enabled = false;

		isDead = true;
	}

	public void Revive()
	{
		health = maxHealth;

		GetComponent<PlayerMovement>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
		GetComponent<PlayerSword>().enabled = true;

		StopAllCoroutines();

		Color c = sr.color;
		c.a = 1f;
		sr.color = c;

		isDead = false;

        Debug.Log("dead");
	}

}
