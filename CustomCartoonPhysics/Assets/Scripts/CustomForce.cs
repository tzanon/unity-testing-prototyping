using UnityEngine;

// Custom force for specific, unrealistic behaviour
public class CustomForce
{
	private MagnitudeDropoffModel _dropoffModel;
	private Vector2 _direction;

	public Vector2 Direction
	{
		get
		{
			return _direction;
		}
	}

	public CustomForce()
	{
		
	}



}
