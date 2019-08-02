using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics
{
	public enum DropoffType { Constant, Linear, Slide, Impulse }; //need this?
	
	public class ModelLine
	{
		private float _slope;
		private float _yIntercept;
		
		public float Slope
		{
			get { return _slope; }
		}
		
		public float YIntercept
		{
			get { return _yIntercept; }
		}
		
		public ModelLine(Vector2 p1, Vector2 p2)
		{
			RecalculateLine(p1, p2);
		}
		
		
		public void RecalculateLine(Vector2 p1, Vector2 p2)
		{
			_slope = (p2.y - p1.y) / (p2.x - p1.x);
			_yIntercept = p2.y - _slope * p2.x;
		}
		
		public float GetLinePoint(float x)
		{
			return _slope * x + _yIntercept;
		}
	}
	
	/// <summary>
	/// the domain of a <c>LineModel</c>, inclusive start, exclusive end
	/// </summary>
	public struct ModelTimeDomain
	{
		private float start;
		private float end;
		
		public ModelTimeDomain(float t1, float t2)
		{
			if (t1 < 0 || t2 < 0)
				Debug.LogError("Model range times cannot be negative");
			if (t2 <= t1)
				Debug.LogError("Model's end time must be greater than its start time");
			
			start = t1;
			end = t2;
		}
		
		// overload comparison operators for easy equality check
		public static bool operator ==(ModelTimeDomain r1, ModelTimeDomain r2)
		{
			return (r1.start == r2.start && r1.end == r2.end);
		}
		public static bool operator !=(ModelTimeDomain r1, ModelTimeDomain r2)
		{
			return (r1.start != r2.start || r1.end != r2.end);
		}
		
		public bool Contains(float t)
		{
			return t >= start && t < end;
		}
		
	}
	
	
	
}
