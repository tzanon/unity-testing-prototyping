using CustomPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public class MPListTests
	{
		private const float initStrength = 10.0f;
		private const float lifetime = 5.0f;

		private static readonly ModelPoint[] initPoints = new ModelPoint[] {
			new ModelPoint(1.0f, 8.0f),
			new ModelPoint(3.5f, 1.5f),
			new ModelPoint(2.0f, 5.0f)
		};

		private ModelPointList basicList = new ModelPointList(initStrength, lifetime);
		private ModelPointList listWithInterPoints = new ModelPointList(initStrength, lifetime, initPoints);

		[Test]
		public void BasicMPL_InitStrengthTest()
		{
			ModelPoint startPoint = new ModelPoint(0.0f, initStrength);
			bool status = (startPoint == basicList[0]) && (startPoint == basicList.Start);
			Assert.IsTrue(status);
		}
		
		[Test]
		public void BasicMPL_LifetimeTest()
		{
			ModelPoint endPoint = new ModelPoint(lifetime, 0.0f);
			bool status = (endPoint == basicList[1]) && (endPoint == basicList.End);
			Assert.IsTrue(status);
		}

		[Test]
		public void BasicMPL_CountTest()
		{
			int expectedCount = 2;
			Assert.AreEqual(expectedCount, basicList.Count);
		}

		[Test]
		public void MPL_CountTest()
		{
			int expectedCount = 5;
			Assert.AreEqual(expectedCount, listWithInterPoints.Count);
		}

		[Test]
		public void MPLOutOfBoundsTest()
		{
			Assert.Throws<IndexOutOfRangeException>(delegate { ModelPoint nonexistantMP = basicList[2]; });
		}

		[Test]
		public void MPL_ContainsStartTest()
		{
			ModelPoint startPoint = new ModelPoint(0.0f, initStrength);
			Assert.IsTrue(basicList.Contains(startPoint));
		}

		[Test]
		public void MPL_ContainsEndTest()
		{
			ModelPoint endPoint = new ModelPoint(lifetime, 0.0f);
			Assert.IsTrue(basicList.Contains(endPoint));
		}

		[Test]
		public void MPL_ContainsTest()
		{
			ModelPoint point = new ModelPoint(2.0f, 5.0f);
			Assert.IsTrue(listWithInterPoints.Contains(point));
		}

		[Test]
		public void MPL_OrderTest()
		{

		}

		[Test]
		public void MPL_AddPointTest()
		{
			ModelPointList pointList = new ModelPointList(initStrength, lifetime);
			ModelPoint mp = new ModelPoint(2.0f, 3.5f);
			pointList.Add(mp);

			Assert.AreEqual(mp, pointList[1]);
		}


	}
}
