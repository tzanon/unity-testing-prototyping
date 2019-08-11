using CustomPhysics;
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
		private const float lifetime = 3.0f;
		ModelPointList basicList = new ModelPointList(initStrength, lifetime);

		[Test]
		public void InitialMPL_InitStrengthTest()
		{
			Assert.AreEqual(initStrength, basicList.Start.Strength);
		}
		
		[Test]
		public void InitialMPL_LifetimeTest()
		{
			Assert.AreEqual(lifetime, basicList.End.Time);
		}

		[Test]
		public void InitialMPL_CountTest()
		{
			int expectedCount = 2;
			Assert.AreEqual(expectedCount, basicList.Count);
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

	}
}
