using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnitudeDropoffModel
{
	private struct ModelLine
	{
		float slope;
		float y_intercept;
	}


	private Vector2 _start;
	private Vector2 _end;

	private List<Vector2> points;

	public float InitialMagnitude
	{
		get
		{
			return _start.y;
		}
		set
		{
			if (value >= 0)
				_start = new Vector2(0f, value);
			else
			{
				Debug.LogError("Force cannot have a negative magnitude");
				_start = new Vector2(0f, 0f);
			}
		}
	}

	public float ForceLifetime
	{
		get
		{
			return _end.x;
		}
		set
		{
			if (value > 0)
				_end = new Vector2(value, 0f);
			else
			{
				Debug.LogError("Force cannot have a non-positive lifetime");
				_end = new Vector2(0f, 0.1f);
			}
		}
	}

	public MagnitudeDropoffModel(float init, float lifetime)
	{
		InitialMagnitude = init;
		ForceLifetime = lifetime;

		points = new List<Vector2>();
	}

	private void CalculateModel()
	{
		if (points.Count <= 0)
		{

		}
	}

	private void CalculateLine(Vector2 p1, Vector2 p2)
	{

	}

	public float GetMagnitudeAtTime(float t)
	{

		return 0.0f;
	}

}
