using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics
{
	public enum DropoffType { Constant, Linear, Slide, Impulse }; //need this?

	/// <summary>
	/// Similar to a Vector2 but disallows negative x-values, since x represents time
	/// </summary>
	public struct ModelPoint : IComparer<ModelPoint>
	{
		private float time;
		private float strength;

		public float Time
		{
			get { return time; }
			set
			{
				if (value < 0.0f)
				{
					Debug.LogError("Model point cannot have a negative time value!");
					time = 0.0f;
				}
				else
				{
					time = value;
				}
			}
		}

		public float Strength
		{
			get { return strength; }
			set { strength = value; }
		}

		public ModelPoint(float t, float s)
		{
			if (t < 0.0f)
			{
				Debug.LogError("Model point cannot have a negative time value!");
				time = 0.0f;
			}
			else
			{
				time = t;
			}

			strength = s;
		}

		/// <summary>
		/// In case this point needs to be used with Vector2s beyond equality checking
		/// (No, I'm not overloading all the basic operators, especially when commutativity isn't supported)
		/// </summary>
		public Vector2 AsVector2()
		{
			return new Vector2(time, strength);
		}

		public static bool operator ==(ModelPoint a, ModelPoint b)
		{
			return (a.time == b.time && a.strength == b.strength);
		}
		public static bool operator !=(ModelPoint a, ModelPoint b)
		{
			return (a.time != b.time || a.strength != b.strength);
		}

		// compare with Vector2s as well
		public static bool operator ==(ModelPoint mp, Vector2 v2)
		{
			return (mp.time == v2.x && mp.strength == v2.y);
		}
		public static bool operator !=(ModelPoint mp, Vector2 v2)
		{
			return (mp.time != v2.x || mp.strength != v2.y);
		}
		public static bool operator ==(Vector2 v2, ModelPoint mp)
		{
			return (mp.time == v2.x && mp.strength == v2.y);
		}
		public static bool operator !=(Vector2 v2, ModelPoint mp)
		{
			return (mp.time != v2.x || mp.strength != v2.y);
		}

		/// <summary>
		/// Points are compared just by their time value.
		/// Intended for use in sorting lists.
		/// </summary>
		public int Compare(ModelPoint a, ModelPoint b)
		{
			if (a.time > b.time)
				return 1;
			if (a.time < b.time)
				return -1;
			else
				return 0;
		}
	}

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
	/// The domain of a <c>LineModel</c>, inclusive min, exclusive max
	/// </summary>
	public struct ModelTimeDomain
	{
		private readonly float min;
		private readonly float max;
		
		public ModelTimeDomain(float t1, float t2)
		{
			if (t1 < 0 || t2 < 0)
				Debug.LogError("Model range times cannot be negative");
			if (t2 <= t1)
				Debug.LogError("Model's end time must be greater than its start time");
			
			min = t1;
			max = t2;
		}
		
		// overload comparison operators for easy equality check
		public static bool operator ==(ModelTimeDomain r1, ModelTimeDomain r2)
		{
			return (r1.min == r2.min && r1.max == r2.max);
		}
		public static bool operator !=(ModelTimeDomain r1, ModelTimeDomain r2)
		{
			return (r1.min != r2.min || r1.max != r2.max);
		}
		
		public bool Contains(float t)
		{
			return t >= min && t < max;
		}
		
	}
	
	
	
}
