using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleAI : MonoBehaviour
{

	public Rigidbody2D target;
	public float moveSpeed = 4f;
	public float turnSpeed = .1f;

	public float chaseRange = 999f;

	public float attackRange = .2f;
	public float attackRate = 1f;
	private float nextAttackTime = 0f;

	public Vector2 attackOffset;

	public bool isRanged = false;
	public GameObject bulletPrefab;
	public float bulletSpeed = 5f;

	public Sprite spriteRight;
	public Sprite spriteLeft;
	public Sprite spriteUp;
	public Sprite spriteDown;

	public SpriteRenderer spriteRenderer;

	Rigidbody2D rb;

	float rotAngle = 0f;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}

	void Attack()
	{
		target.GetComponent<Player>().TakeDamage(1);
	}

	void Shoot()
	{
		Vector2 dir = target.position - (rb.position + attackOffset);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
		GameObject go = Instantiate(bulletPrefab, rb.position + attackOffset,
									Quaternion.AngleAxis(angle, Vector3.forward),
									transform.parent) as GameObject;
		go.GetComponent<Rigidbody2D>().AddForce(dir.normalized * bulletSpeed, ForceMode2D.Impulse);
	}

	private void FixedUpdate()
	{
		if (target == null)
			return;

		float dist = Vector2.Distance(target.position, rb.position);

		if (dist > chaseRange)
		{
			return;
		}

		if (dist <= attackRange)
		{
			if (Time.time >= nextAttackTime)
			{
				if (nextAttackTime == 0f)
				{
					nextAttackTime = Time.time + .3f;
					return;
				}

				if (isRanged)
					Shoot();
				else
					Attack();

				nextAttackTime = Time.time + 1f / attackRate;
			}
			return;
		}

		Vector2 dir = (target.position - (rb.position + attackOffset)).normalized;

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
		rotAngle = Mathf.LerpAngle(rotAngle, angle, turnSpeed);

		Vector2 newDir = Quaternion.AngleAxis(rotAngle + 90, Vector3.forward) * Vector3.right;

		Vector2 force = newDir * Time.fixedDeltaTime * moveSpeed;

		rb.AddForce(force);

		float absX = Mathf.Abs(rb.velocity.x);
		float absY = Mathf.Abs(rb.velocity.y);

		if (absX > absY)
		{
			if (rb.velocity.x > .01f)
			{
				spriteRenderer.sprite = spriteRight;
			}
			if (rb.velocity.x < -0.01f)
			{
				spriteRenderer.sprite = spriteLeft;
			}
		} else if (absX < absY)
		{
			if (rb.velocity.y > .01f)
			{
				spriteRenderer.sprite = spriteUp;
			}
			if (rb.velocity.y < -0.01f)
			{
				spriteRenderer.sprite = spriteDown;
			}
		}

		spriteRenderer.transform.rotation = Quaternion.identity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			//collision.collider.GetComponent<Player>().TakeDamage(1);
			//GetComponent<Enemy>().TakeDamage(9999);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, attackRange);
	}

}
