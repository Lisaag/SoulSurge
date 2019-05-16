using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed = 2f;

	public Transform swordRotation;

	public Sprite spriteRight;
	public Sprite spriteLeft;
	public Sprite spriteUp;
	public Sprite spriteDown;

	public float knockBack = 1f;

	Vector2 movement;

    Rigidbody2D rb;
	SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
    }

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

		if (movement.x > 0.01f)
		{
			swordRotation.eulerAngles = new Vector3(0f, 0f, 270f);
			sr.sprite = spriteRight;
		} else if (movement.x < -0.01f)
		{
			swordRotation.eulerAngles = new Vector3(0f, 0f, 90f);
			sr.sprite = spriteLeft;
		} else if (movement.y > 0.01f)
		{
			swordRotation.eulerAngles = new Vector3(0f, 0f, 0f);
			sr.sprite = spriteUp;
		} else if (movement.y < -0.01f)
		{
			swordRotation.eulerAngles = new Vector3(0f, 0f, 180f);
			sr.sprite = spriteDown;
		}
	}

}
