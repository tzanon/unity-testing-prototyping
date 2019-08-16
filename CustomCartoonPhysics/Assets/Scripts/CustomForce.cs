using CustomPhysics;
using UnityEngine;

// Custom force for specific, unrealistic behaviour
public class CustomForce
{
	private MagnitudeDropoffModel _dropoffModel;
	private Vector2 _direction;

	public float Lifetime
	{
		get
		{
			return _dropoffModel.Lifetime;
		}
	}

	public Vector2 Direction
	{
		get
		{
			return _direction;
		}
	}

	public CustomForce(MagnitudeDropoffModel model, Vector2 dir)
	{
		_dropoffModel = model;
		_direction = dir.normalized;
	}

	public Vector2 ForceAtTime(float time)
	{
		if (time < 0.0f)
		{
			Debug.LogError("Cannot access force with negative time");
			return Vector2.zero;
		}

		float magnitude = _dropoffModel.GetMagnitudeAtTime(time);
		Vector2 force = magnitude * _direction;

		return force;
	}

	public override string ToString()
	{
		string forceStr = "Force has direction " + _direction + " and " + _dropoffModel.ToString();
		return forceStr;
	}

}
