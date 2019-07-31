using CustomPhysics;

using System.Collections.Generic;
using UnityEngine;

public class MagnitudeDropoffModel
{
	private Vector2 _start;
	private Vector2 _end;

	// these two lists MUST ALWAYS be sorted by x (time)!!
	private List<Vector2> _intermediatePoints;
	
	private Vector2[] _totalPoints;
	
	private Dictionary<ModelTimeDomain, ModelLine> _modelLines;

	public float InitialMagnitude
	{
		get
		{
			return _start.y;
		}
		set
		{
			if (value >= 0)
			{
				_start = new Vector2(0f, value);
				this.CalculateModel();
			}
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
			{
				_end = new Vector2(value, 0f);
				this.CalculateModel();
			}
			else
			{
				Debug.LogError("Force cannot have a non-positive lifetime");
				_end = new Vector2(0f, 0.1f);
			}
		}
	}
	
	public MagnitudeDropoffModel(float init, float lifetime)
	{
		_start = new Vector2(0.0f, init);
		_end = new Vector2(lifetime, 0.0f);

		_intermediatePoints = new List<Vector2>();
		_modelLines = new Dictionary<ModelTimeDomain, ModelLine>();
		
		this.CalculateModel();
	}
	
	
	public void AddPoint(Vector2 point)
	{
		
		CalculateModel();
	}
	
	public void RemovePoint(Vector2 point)
	{
		
		CalculateModel();
	}
	
	/// <summary>
	/// based on model's points (start, end, and any intermediate ones),
	/// construct the lines that comprise it
	/// </summary>
	private void CalculateModel()
	{
		_modelLines.Clear(); // this is inefficient, change later?
		
		// calculate array of all points
		int numInterPoints = _intermediatePoints.Count;
		
		_totalPoints = new Vector2[numInterPoints+2];
		_totalPoints[0] = _start;
		_totalPoints[_totalPoints.Length-1] = _end;
		
		for (int i = 0; i < numInterPoints; i++)
		{
			_totalPoints[i+1] = _intermediatePoints[i];
		}
		
		// calculate model's lines and their corresponding time domains
		for (int i = 0; i < _totalPoints.Length - 1; i++)
		{
			AddLineToModel(_totalPoints[i], _totalPoints[i+1]);
		}
	}
	
	private void AddLineToModel(Vector2 p1, Vector2 p2)
	{
		ModelLine line = new ModelLine(p1, p2);
		ModelTimeDomain domain = new ModelTimeDomain(p1.x, p2.x);
		
		_modelLines.Add(domain, line);
	}
	
	/// <summary>
	/// gets the magnitude of the force
	/// </summary>
	public float GetMagnitudeAtTime(float time)
	{
		if (time <= 0.0f)
		{
			return InitialMagnitude;
		}
		else if (time < ForceLifetime)
		{
			foreach (ModelTimeDomain domain in _modelLines.Keys)
			{
				if (domain.Contains(time))
				{
					ModelLine line = _modelLines[domain];
					return line.GetLinePoint(time);
				}
			}
			Debug.LogError("Given time does not exist in any domain");
			return 0.0f;
		}
		else // if time is equal or greater to the lifetime
		{
			return 0.0f;
		}
	}
	
	
}
