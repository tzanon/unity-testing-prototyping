
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CustomPhysics
{
	/// <summary>
	/// Similar to a Vector2 but disallows negative x-values, since x represents time
	/// </summary>
	public struct ModelPoint
	{
		private float _time;
		private float _strength;
		
		public float Time
		{
			get { return _time; }
			set
			{
				if (value < 0.0f)
				{
					Debug.LogError("Model point cannot have a negative time value!");
					_time = 0.0f;
				}
				else
				{
					_time = value;
				}
			}
		}
		
		public float Strength
		{
			get { return _strength; }
			set { _strength = value; }
		}
		
		public ModelPoint(float t, float s)
		{
			if (t < 0.0f)
			{
				Debug.LogError("Model point cannot have a negative time value!");
				_time = 0.0f;
			}
			else
			{
				_time = t;
			}
			
			_strength = s;
		}

		public ModelPoint(Vector2 v2)
		{
			if (v2.x < 0.0f)
			{
				Debug.LogError("Model point cannot have a negative time value!");
				_time = 0.0f;
			}
			else
			{
				_time = v2.x;
			}

			_strength = v2.y;
		}

		/// <summary>
		/// In case this point needs to be used with Vector2s beyond equality checking
		/// </summary>
		public Vector2 AsVector2()
		{
			return new Vector2(_time, _strength);
		}

		public static bool operator ==(ModelPoint a, ModelPoint b)
		{
			if (a == null && b == null)
				return true;
			if (a == null || b == null)
				return false;
			return (a._time == b._time && a._strength == b._strength);
		}
		public static bool operator !=(ModelPoint a, ModelPoint b)
		{
			if (a == null && b == null)
				return true;
			if (a == null || b == null)
				return false;
			return (a._time != b._time || a._strength != b._strength);
		}
		public static ModelPoint operator *(float a, ModelPoint mp)
		{
			return new ModelPoint(mp.Time, a * mp.Strength);
		}
		public static ModelPoint operator *(ModelPoint mp, float a)
		{
			return a * mp;
		}
		public static ModelPoint MultiplyByTimeAndStrength(float timeFactor, float strengthFactor, ModelPoint mp)
		{
			return new ModelPoint(timeFactor * mp.Time, strengthFactor * mp.Strength);
		}

		// compare with Vector2s as well
		public static bool operator ==(ModelPoint mp, Vector2 v2)
		{
			if (mp == null && v2 == null)
				return true;
			if (mp == null || v2 == null)
				return false;
			return (mp._time == v2.x && mp._strength == v2.y);
		}
		public static bool operator !=(ModelPoint mp, Vector2 v2)
		{
			if (mp == null && v2 == null)
				return false;
			if (mp == null || v2 == null)
				return true;
			return (mp._time != v2.x || mp._strength != v2.y);
		}
		public static bool operator ==(Vector2 v2, ModelPoint mp)
		{
			return mp == v2;
		}
		public static bool operator !=(Vector2 v2, ModelPoint mp)
		{
			return mp != v2;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				ModelPoint point = (ModelPoint)obj;
				return this == point;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return "(" + _time + ", " + _strength + ")";
		}

	}

	/// <summary>
	/// Comparer class to compare two ModelPoints
	/// </summary>
	public class ModelPointComparer : Comparer<ModelPoint>
	{
		/// <summary>
		/// Compare model points according to their time value.
		/// return 1 if a > b,
		/// -1 if a < b,
		/// return 0 if a == b
		/// </summary>
		public override int Compare(ModelPoint a, ModelPoint b)
		{
			if (a.Time - b.Time > 0) // if a > b
			{
				return 1;
			}
			else if (a.Time - b.Time < 0) // if a < b
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
	}

	/// <summary>
	/// Linked list of ModelPoints, ordered by time
	/// The start and end cannot be removed
	/// The start can only have it's strength field changed
	/// The end point can only have its time field changed
	/// </summary>
	public class ModelPointList
	{
		// Node of a ModelPoint linked list
		private class ModelPointNode
		{
			public ModelPointNode next;
			public ModelPointNode previous;
			private ModelPoint _value;

			public ModelPoint Value
			{
				get { return _value; }
			}

			public ModelPointNode(ModelPoint mp)
			{
				_value = mp;
			}

			public ModelPointNode(float t, float s)
			{
				_value = new ModelPoint(t, s);
			}
		}

		private ModelPointNode _start;
		private ModelPointNode _end;
		private readonly ModelPointComparer _comparer;

		public ModelPoint this[int index]
		{
			get
			{
				int idx = 0;
				ModelPointNode current = _start;

				while (current != null && idx < index)
				{
					current = current.next;
					idx++;
				}

				if (current == null)
				{
					throw new IndexOutOfRangeException("Index " + index + " out of range on point list with " + this.Count + " points");
				}
				else
				{
					return current.Value;
				}
			}
		}

		/// <summary>
		/// Number of points in the list
		/// </summary>
		public int Count
		{
			get
			{
				int count = 1;
				ModelPointNode currentNode = _start;
				
				while (currentNode.next != null)
				{
					currentNode = currentNode.next;
					count++;
				}

				return count;
			}
		}

		/// <summary>
		/// Point representing beginning magnitude
		/// </summary>
		public ModelPoint Start
		{
			get { return _start.Value; }
		}

		/// <summary>
		/// Point representing model lifetime
		/// </summary>
		public ModelPoint End
		{
			get { return _end.Value; }
		}

		public ModelPointList(ModelPoint initStrengthPoint, ModelPoint lifetimePoint, ModelPoint[] initIntermediatePoints = null)
		{
			if (initStrengthPoint.Time != 0.0f)
			{
				Debug.LogError("Initial strength point must have a time value of zero!");
				initStrengthPoint.Time = 0.0f;
			}

			if (lifetimePoint.Strength != 0.0f)
			{
				Debug.LogError("Lifetime point must have a strength value of zero!");
				lifetimePoint.Strength = 0.0f;
			}

			_start = new ModelPointNode(initStrengthPoint);
			_end = new ModelPointNode(lifetimePoint);
			_comparer = new ModelPointComparer();

			_start.next = _end;
			_end.previous = _start;

			if (initIntermediatePoints != null)
			{
				foreach (ModelPoint mp in initIntermediatePoints)
				{
					this.Add(mp);
				}
			}
		}

		/// <summary>
		/// Constructor that makes the start and end points based on the given initial magnitude and lifetime
		/// </summary>
		public ModelPointList(float initialStrength, float lifetime, ModelPoint[] initIntermediatePoints = null)
			: this(new ModelPoint(0.0f, initialStrength), new ModelPoint(lifetime, 0.0f), initIntermediatePoints) {}

		/// <summary>
		/// Constructor that makes a list with initial magnitude 1.0
		/// </summary>
		public ModelPointList(float lifetime, ModelPoint[] initIntermediatePoints = null) : this(1.0f, lifetime, initIntermediatePoints) {}

		/// <summary>
		/// Change magnitude of start node
		/// </summary>
		public void SetStartValue(float initMagnitude)
		{
			ModelPointNode newStart = new ModelPointNode(0.0f, initMagnitude);
			ModelPointNode secondNode = _start.next;

			// replace starting node, can't modify as it's read-only
			newStart.next = secondNode;
			secondNode.previous = newStart;
			_start = newStart;
		}

		/// <summary>
		/// Change time of end node
		/// </summary>
		public bool SetEndValue(float lifetime)
		{
			ModelPointNode secondLastNode = _end.previous;

			if (lifetime <= secondLastNode.Value.Time)
			{
				Debug.LogError("Cannot set a lifetime shorter than the previous point's timestamp");
				return false;
			}

			ModelPointNode newEnd = new ModelPointNode(lifetime, 0.0f)
			{
				previous = secondLastNode
			};
			secondLastNode.next = newEnd;
			_end = newEnd;

			return true;
		}

		/// <summary>
		/// Adds point to list, keeping order with the other nodes
		/// Returns true if addition successful, false otherwise
		/// </summary>
		public bool Add(ModelPoint point)
		{
			// check that point is within the model's bounds
			if (!PointInBounds(point))
			{
				Debug.LogWarning("Point " + point.ToString() + " is not in bounds");
				return false;
			}

			// don't add if point already in the list
			if (Contains(point))
			{
				Debug.LogWarning("List already contains point " + point.ToString());
				return false;
			}

			ModelPointNode currentNode = _start;

			while (currentNode.next != null &&
				!(_comparer.Compare(point, currentNode.Value) == 1 &&
				_comparer.Compare(point, currentNode.next.Value) == -1 ))
			{
				currentNode = currentNode.next;
			}

			if (currentNode.next == null)
			{

				return false;
			}
			else
			{
				ModelPointNode newNode = new ModelPointNode(point);
				ModelPointNode nextNode = currentNode.next;

				currentNode.next = newNode;
				newNode.next = nextNode;
				newNode.previous = currentNode;
				nextNode.previous = newNode;

				return true;
			}
		}

		/// <summary>
		/// Removes point from the list
		/// </summary>
		public bool Remove(ModelPoint point)
		{
			// can't remove an out of bounds point
			if (!PointInBounds(point))
			{
				Debug.LogWarning("Point " + point.ToString() + " is not in bounds");
				return false;
			}

			// can't remove the start or end points
			if (point == _start.Value || point == _end.Value)
			{
				Debug.LogWarning("Cannot remove the start or end point");
				return false;
			}

			// iterate through intermediate points to find the point
			ModelPointNode currentNode = _start;

			while (currentNode.next != null && currentNode.next.Value != point)
			{
				currentNode = currentNode.next;
			}

			// point not in list
			if (currentNode.next == null)
			{
				return false;
			}
			else
			{
				ModelPointNode newNext = currentNode.next.next;
				currentNode.next = newNext;
				newNext.previous = currentNode;
				return true;
			}
		}

		/// <summary>
		/// Returns the list's modelpoints as an array
		/// </summary>
		public ModelPoint[] ToArray()
		{
			ModelPoint[] pointArray = new ModelPoint[this.Count];
			ModelPointNode current = _start;
			int idx = 0;

			while (current != null)
			{
				pointArray[idx] = current.Value;
				idx++;
				current = current.next;
			}

			return pointArray;
		}

		/// <summary>
		/// get the point's index in this list
		/// returns -1 if it isn't in the list
		/// </summary>
		public int IndexOf(ModelPoint point)
		{
			int idx = 0;
			ModelPointNode currentNode = _start;

			while (currentNode != null)
			{
				if (currentNode.Value == point)
					return idx;
				idx++;
				currentNode = currentNode.next;
			}

			return -1;
		}

		/// <summary>
		/// Checks if the list contains the given point
		/// </summary>
		public bool Contains(ModelPoint point)
		{
			ModelPointNode currentNode = _start;

			while (currentNode != null)
			{
				if (currentNode.Value == point)
					return true;
				currentNode = currentNode.next;
			}

			return false;
		}

		/// <summary>
		/// Checks if the given point is within the list's start and end point's times
		/// </summary>
		public bool PointInBounds(ModelPoint point)
		{
			return (_comparer.Compare(_start.Value, point) < 0 && _comparer.Compare(_end.Value, point) > 0);
		}

		/// <summary>
		/// Returns string of model points in the list
		/// </summary>
		public override string ToString()
		{
			string descr = "ModelPointList with " + this.Count + " points: ";

			ModelPointNode currentNode = _start;
			while (currentNode != null)
			{
				descr += currentNode.Value.ToString();
				if (currentNode != _end)
					descr += ", ";
				currentNode = currentNode.next;
			}

			return descr;
		}
	}

	/// <summary>
	/// A line component of the model
	/// </summary>
	public struct ModelLine
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
		
		public ModelLine(ModelPoint p1, ModelPoint p2)
		{
			_slope = 0.0f;
			_yIntercept = 0.0f;
			RecalculateLine(p1, p2);
		}
		
		public bool RecalculateLine(ModelPoint p1, ModelPoint p2)
		{
			if (p1.Time == p2.Time)
			{
				Debug.LogError("Model line cannot have an infinite slope");
				return false;
			}

			_slope = (float)Math.Round((p2.Strength - p1.Strength) / (p2.Time - p1.Time), 2);
			_yIntercept = (float)Math.Round(p2.Strength - _slope * p2.Time, 2);
			return true;
		}
		
		public float GetLinePoint(float x)
		{
			return _slope * x + _yIntercept;
		}

		/// <summary>
		/// Returns string representation of the line
		/// </summary>
		public override string ToString()
		{
			string lineStr = "S(t) = " + _slope + "t + " + _yIntercept;
			return lineStr;
		}
	}

	/// <summary>
	/// The domain of a LineModel, inclusive min, exclusive max
	/// </summary>
	public struct ModelTimeDomain
	{
		public readonly float min;
		public readonly float max;

		public static ModelTimeDomain Default
		{
			get
			{
				return new ModelTimeDomain(0, Mathf.Infinity);
			}
		}

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
			if (r1 == null || r2 == null)
				return false;

			return (r1.min == r2.min && r1.max == r2.max);
		}
		public static bool operator !=(ModelTimeDomain r1, ModelTimeDomain r2)
		{
			if (r1 == null || r2 == null)
				return true;

			return (r1.min != r2.min || r1.max != r2.max);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				ModelTimeDomain domain = (ModelTimeDomain)obj;
				return this == domain;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Contains(float t)
		{
			return t >= min && t < max;
		}

		public bool Contains(ModelPoint mp)
		{
			return Contains(mp.Time);
		}

		public override string ToString()
		{
			string domainStr = "[" + min + ", " + max +")";
			return domainStr;
		}
	}

	/// <summary>
	/// Model used by custom force; relates time domains to a linear function
	/// </summary>
	public struct MagnitudeDropoffModel
	{
		private readonly Dictionary<ModelTimeDomain, ModelLine> _lines;
		private readonly ModelPoint[] _points;

		public float InitialStrength
		{
			get
			{
				return _points[0].Strength;
			}
		}

		public float Lifetime
		{
			get
			{
				return _points[_points.Length - 1].Time;
			}
		}

		public ModelPoint[] Points
		{
			get
			{
				return _points;
			}
		}

		/// <summary>
		/// Based on given points, construct dictionary relating lines to their domains
		/// </summary>
		public MagnitudeDropoffModel(ModelPointList pointList)
		{
			//_modelPoints = pointList;

			_points = pointList.ToArray();
			_lines = new Dictionary<ModelTimeDomain, ModelLine>();
			
			// calculate model's lines and their corresponding time domains
			for (int i = 0; i < _points.Length - 1; i++)
			{
				ModelPoint p1 = _points[i];
				ModelPoint p2 = _points[i+1];
				
				ModelLine line = new ModelLine(p1, p2);
				ModelTimeDomain domain = new ModelTimeDomain(p1.Time, p2.Time);
				_lines.Add(domain, line);
			}
		}

		/// <summary>
		/// Multiply the model's strength dropoff by the given factor
		/// </summary>
		public static MagnitudeDropoffModel operator *(float strengthFactor, MagnitudeDropoffModel model)
		{
			ModelPointList multipliedList = new ModelPointList(strengthFactor * model.InitialStrength, model.Lifetime);

			for (int i = 1; i < model.Points.Length - 1; i++)
			{
				multipliedList.Add(strengthFactor * model.Points[i]);
			}

			MagnitudeDropoffModel multipliedModel = new MagnitudeDropoffModel(multipliedList);
			return multipliedModel;
		}

		/// <summary>
		/// gets the magnitude of the force
		/// </summary>
		public float GetMagnitudeAtTime(float time)
		{
			if (_lines == null)
			{
				throw new Exception("Model's Domain -> Line dictionary not initialized");
			}

			foreach (ModelTimeDomain domain in _lines.Keys)
			{
				if (domain.Contains(time))
				{
					ModelLine line = _lines[domain];
					return line.GetLinePoint(time);
				}
			}
			
			// not in domain
			return 0.0f;
		}
		
		public void WriteToFile()
		{
			// TODO: export the model as a JSON or some other appropriate file type
		}

		public override string ToString()
		{
			if (_lines == null || _lines.Count <= 0)
			{
				return "Dropoff Model: empty";
			}

			string modelStr = "Dropoff Model has init strength=" + InitialStrength + ", lifetime=" + Lifetime + ", points: ";

			foreach (KeyValuePair<ModelTimeDomain, ModelLine> pair in _lines)
			{
				modelStr += pair.Key.ToString() + " -> " + pair.Value.ToString() + ", ";
			}

			return modelStr;
		}

	}

}
