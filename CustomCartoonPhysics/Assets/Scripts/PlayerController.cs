using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 10.0f;

	private Transform tf;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	private void Start()
	{
		tf = this.transform;
		rb = this.GetComponent<Rigidbody2D>();
	}

	// movement, physics, etc.
	private void FixedUpdate()
	{
		Vector2 direction = CalculateMovementDirection();
		rb.velocity = direction * speed;

		/*
		if (direction != Vector2.zero)
			tf.Translate(direction * speed * Time.deltaTime);
		*/
	}

	private Vector2 CalculateMovementDirection()
	{
		Vector2 direction = new Vector2(0, 0);

		if (Input.GetKey(KeyCode.W))
		{
			direction.y += 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			direction.y -= 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			direction.x -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			direction.x += 1;
		}

		return direction.normalized;
	}

}
