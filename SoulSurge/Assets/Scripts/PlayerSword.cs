using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{

	public int damage = 10;
	public float pushBackForce = 1f;

	public Transform swordPoint;
	public float swordRadius;

	public Animator animator;

	public LayerMask swordMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(swordPoint.position, swordRadius, swordMask);
			foreach (Collider2D collider in colliders)
			{
				collider.GetComponent<Enemy>().TakeDamage(damage);
				Vector2 dir = (collider.transform.position - transform.position).normalized;
				collider.GetComponent<Rigidbody2D>().AddForce(dir * pushBackForce, ForceMode2D.Impulse);
			}

			animator.SetTrigger("Attack");
		}
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(swordPoint.position, swordRadius);
	}
}
