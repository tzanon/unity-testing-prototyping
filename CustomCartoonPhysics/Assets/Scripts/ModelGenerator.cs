using CustomPhysics;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ModelGenerator
{
	// this list MUST ALWAYS be sorted by x (time)!!
	private ModelPointList _pointList;

	private Dictionary<ModelTimeDomain, ModelLine> _modelLines;

	public float InitialMagnitude
	{
		get
		{
			return _pointList.Start.Strength;
		}
		set
		{
			if (value >= 0)
			{
				_pointList.SetStartValue(value);

				// TODO: recalc line to inter[0]
				
			}
			else
			{
				// do nothing, keep old value
				Debug.LogError("Force cannot have a negative magnitude");
			}
		}
	}

	public float ForceLifetime
	{
		get
		{
			return _pointList.End.Time;
		}
		set
		{
			if (value > 0)
			{
				_pointList.SetEndValue(value);

				// TODO: recalc line from inter end
				
			}
			else
			{
				// keep old value
				Debug.LogError("Force cannot have a non-positive lifetime");
			}
		}
	}

	public ModelGenerator(float init, float lifetime, ModelPoint[] points = null)
	{
		_pointList = new ModelPointList(init, lifetime, points);
		_modelLines = new Dictionary<ModelTimeDomain, ModelLine>();

		this.CalculateModel();
	}

	/// <summary>
	/// 
	/// </summary>
	public void AddPoint(ModelPoint point)
	{
		// if point happens "before" start, don't add it
		if (!_pointList.PointInBounds(point))
		{
			Debug.LogError("Cannot add out of bounds point " + point.ToString());
			return;
		}
		
		// from the current model, remove the line with domain that contains point
		ModelTimeDomain currentDomain = GetDomainOfPoint(point);
		if (currentDomain != ModelTimeDomain.Default)
			_modelLines.Remove(currentDomain);

		// add to point list
		_pointList.Add(point);

		// create the two new lines and domains the new point ends and starts with
		int idx = _pointList.IndexOf(point);
		ModelPoint prevPoint = _pointList[idx - 1];
		ModelPoint nextPoint = _pointList[idx + 1];
		AddLineToModel(prevPoint, point);
		AddLineToModel(point, nextPoint);
	}

	/// <summary>
	/// 
	/// </summary>
	public void RemovePoint(ModelPoint point)
	{
		// can't remove start or end
		if (point == _pointList.Start || point == _pointList.End)
		{
			return;
		}

		// from the current model, remove the lines the point is part of
		ModelTimeDomain prevDomain = GetDomainWithMax(point);
		ModelTimeDomain nextDomain = GetDomainWithMin(point);
		if (prevDomain != ModelTimeDomain.Default)
			_modelLines.Remove(prevDomain);
		if (prevDomain != ModelTimeDomain.Default)
			_modelLines.Remove(nextDomain);

		// add line connecting the endpoints of the point's lines
		int idx = _pointList.IndexOf(point);
		ModelPoint prevPoint = _pointList[idx - 1];
		ModelPoint nextPoint = _pointList[idx + 1];
		AddLineToModel(prevPoint, nextPoint);

		// remove the point and recalculate lists
		if (!_pointList.Remove(point))
		{
			Debug.LogError("Could not remove point " + point.ToString());
		}
	}

	/// <summary>
	/// return the domain that contains point
	/// </summary>
	public ModelTimeDomain GetDomainOfPoint(ModelPoint point)
	{
		foreach (ModelTimeDomain domain in _modelLines.Keys)
		{
			if (domain.Contains(point.Time))
			{
				return domain;
			}
		}
		return ModelTimeDomain.Default;
	}

	/// <summary>
	/// return the domain for which point is the min
	/// </summary>
	public ModelTimeDomain GetDomainWithMin(ModelPoint point)
	{
		foreach (ModelTimeDomain domain in _modelLines.Keys)
		{
			if (domain.min == point.Time)
			{
				return domain;
			}
		}
		return ModelTimeDomain.Default;
	}
	
	/// <summary>
	/// return the domain for which point is the max
	/// </summary>
	public ModelTimeDomain GetDomainWithMax(ModelPoint point)
	{
		foreach (ModelTimeDomain domain in _modelLines.Keys)
		{
			if (domain.max == point.Time)
			{
				return domain;
			}
		}
		return ModelTimeDomain.Default;
	}

	/// <summary>
	/// based on the model's points, construct the lines that comprise it
	/// </summary>
	private void CalculateModel()
	{
		_modelLines.Clear(); // this is inefficient, change later?
		ModelPoint[] totalPoints = _pointList.ToArray();
		
		// calculate model's lines and their corresponding time domains
		for (int i = 0; i < totalPoints.Length - 1; i++)
		{
			AddLineToModel(totalPoints[i], totalPoints[i+1]);
		}
	}

	/// <summary>
	/// Constructs a domain and line based on p1 and p2 and adds them to the model
	/// </summary>
	private void AddLineToModel(ModelPoint p1, ModelPoint p2)
	{
		ModelLine line = new ModelLine(p1, p2);
		ModelTimeDomain domain = new ModelTimeDomain(p1.Time, p2.Time);
		
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
	
	public void WriteToFile()
	{
		// TODO: export the model as a JSON or some other appropriate file type
	}

	/// <summary>
	/// return string representation of the model:
	/// list of domains and their corresponding lines
	/// </summary>
	public override string ToString()
	{
		// TODO
		return "";
	}
	
}
