using CustomPhysics;
using UnityEngine;

public class CPDummy : CustomPhysicsObject
{
	public float moveSpeed = 10.0f;

	private MagnitudeDropoffModel _dashDropoffModel;

	protected override void Start()
	{
		base.Start();

		this.debugMode = true;

		ModelPoint mp1 = new ModelPoint(0.175f, 50.0f);
		ModelPoint mp2 = new ModelPoint(0.47f, 10.0f);
		ModelPointList pointList = new ModelPointList(200.0f, 0.7f, new ModelPoint[]{mp1, mp2});
		_dashDropoffModel = new MagnitudeDropoffModel(pointList);
	}

	/// <summary>
	/// Called from parent's Update()
	/// Calculates player movement and adds any dash forces
	/// </summary>
	protected override void HandleAdditionalMovement()
	{
		Vector2 moveDirection = CalculateMovementDirection();
		Vector2 dashDirection = CalculateDashDirection();

		if (moveDirection != Vector2.zero)
		{
			totalMovement += moveSpeed * moveDirection * Time.deltaTime;
		}

		if (dashDirection != Vector2.zero)
		{
			CustomForce force = new CustomForce(_dashDropoffModel, dashDirection);
			this.AddCustomForce(force);
		}
	}

	private Vector2 CalculateDashDirection()
	{
		Vector2 direction = Vector2.zero;

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			direction.y += 1;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			direction.y -= 1;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			direction.x -= 1;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			direction.x += 1;
		}

		return direction.normalized;
	}

	private Vector2 CalculateMovementDirection()
	{
		Vector2 direction = Vector2.zero;

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
