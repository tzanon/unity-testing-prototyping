using CustomPhysics;
using UnityEngine;

public class CPDummy : CustomPhysicsObject
{
	private MagnitudeDropoffModel _movementDropoffModel;

	protected override void Start()
	{
		base.Start();

		this.debugMode = true;

		ModelPoint mp1 = new ModelPoint(0.175f, 50.0f);
		ModelPoint mp2 = new ModelPoint(0.47f, 10.0f);
		ModelPointList pointList = new ModelPointList(200.0f, 0.7f, new ModelPoint[]{mp1, mp2});
		_movementDropoffModel = new MagnitudeDropoffModel(pointList);
	}

	protected override void FixedUpdate()
	{
		Vector2 direction = CalculateMovementDirection();

		if (direction != Vector2.zero)
		{
			CustomForce force = new CustomForce(_movementDropoffModel, direction);
			this.AddCustomForce(force);
		}

		base.FixedUpdate();
	}

	private Vector2 CalculateMovementDirection()
	{
		Vector2 direction = Vector2.zero;

		if (Input.GetKeyDown(KeyCode.W))
		{
			direction.y += 1;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			direction.y -= 1;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			direction.x -= 1;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			direction.x += 1;
		}

		return direction;
	}

}
