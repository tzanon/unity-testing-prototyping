using CustomPhysics;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class CustomPhysicsObject : MonoBehaviour
{
	protected class ForceRunner
	{
		public float currentTime = 0.0f;

		private CustomForce _force;
		private readonly float _expiryTime;

		public bool Finished
		{
			get
			{
				return (currentTime >= _expiryTime);
			}
		}

		public ForceRunner(CustomForce f)
		{
			_force = f;
			_expiryTime = _force.Lifetime;
		}

		public ForceRunner(MagnitudeDropoffModel model, Vector2 dir) : this(new CustomForce(model, dir)) {}

		public Vector2 GetCurrentForce()
		{
			if (Finished)
			{
				return Vector2.zero;
			}

			Vector2 currentForce = _force.ForceAtTime(currentTime);
			return currentForce;
		}

		public override string ToString()
		{
			return _force.ToString();
		}
	}

	public bool debugMode = false;

	//public float CP_mass = 1.0f;

	protected Vector2 totalMovement;

	protected Rigidbody2D rb;
	private readonly HashSet<ForceRunner> _forceRunners = new HashSet<ForceRunner>();

	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		totalMovement = Vector2.zero;
	}

	// 
	private void FixedUpdate()
	{
		totalMovement = Vector2.zero;

		if (_forceRunners.Count > 0)
		{
			Vector2 customForceMovement = CalculateCustomForceMovement();
			totalMovement += customForceMovement * Time.fixedDeltaTime;
		}
	}

	// 
	private void Update()
	{
		HandleAdditionalMovement();

		// final movement
		//rb.velocity = totalMovement;
		rb.MovePosition(rb.position + totalMovement);
	}

	// 
	private void LateUpdate()
	{
		totalMovement = Vector2.zero;
	}

	protected abstract void HandleAdditionalMovement();

	public void AddCustomForce(CustomForce force)
	{
		ForceRunner runner = new ForceRunner(force);
		_forceRunners.Add(runner);

		if (debugMode)
		{
			Debug.Log("Added custom force " + force.ToString());
		}
	}

	// Applies the effects of all current forces for the current frame
	private Vector2 CalculateCustomForceMovement()
	{
		Vector2 customForceMovement = Vector2.zero;

		// apply each force
		foreach (ForceRunner runner in _forceRunners)
		{
			Vector2 rawForce = runner.GetCurrentForce();
			Vector2 massScaledForce = rawForce / rb.mass; // maybe use a separate custom "mass" or multiplier?

			customForceMovement += massScaledForce;

			// increase the force's elapsed time
			runner.currentTime += Time.fixedDeltaTime;

			if (debugMode && runner.Finished)
			{
				Debug.Log("Force now finished: " + runner.ToString());
			}
		}

		// remove the expired forces
		_forceRunners.RemoveWhere(runner => runner.Finished);

		return customForceMovement;
	}

}
