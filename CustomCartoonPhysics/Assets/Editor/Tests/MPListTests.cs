using CustomPhysics;
using System;
using NUnit.Framework;

namespace Tests
{
	public class MPListTests
	{
		private const float initStrength = 10.0f;
		private const float lifetime = 5.0f;

		private static readonly ModelPoint
			// list points, in sorted order
			mp1 = new ModelPoint(1.0f, 8.0f),
			mp3 = new ModelPoint(2.0f, 5.0f),
			mp2 = new ModelPoint(3.5f, 1.5f),

			// these ones don't go in the list
			mp4 = new ModelPoint(1.5f, 3.0f),
			mp5 = new ModelPoint(7.0f, 30.0f);

		private static readonly ModelPoint[] initPoints = new ModelPoint[] { mp1, mp2, mp3 };

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
		public void MPL_IdxOutOfRangeTest()
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
			Assert.IsTrue(listWithInterPoints.Contains(mp3));
		}

		[Test]
		public void MPL_DoesNotContainTest()
		{
			Assert.IsFalse(listWithInterPoints.Contains(mp4));
		}

		[Test]
		public void MPL_HasIndexTest()
		{
			int expectedIdx = 2;
			int actualIdx = listWithInterPoints.IndexOf(mp3);
			Assert.AreEqual(expectedIdx, actualIdx);
		}

		[Test]
		public void MPL_HasNoIndexTest()
		{
			int expectedIdx = -1;
			int actualIdx = listWithInterPoints.IndexOf(mp4);
			Assert.AreEqual(expectedIdx, actualIdx);
		}

		[Test]
		public void MPL_PointInBoundsTest()
		{
			Assert.IsTrue(listWithInterPoints.PointInBounds(mp4));
		}

		[Test]
		public void MPL_PointNotInBoundsTest()
		{
			Assert.IsFalse(listWithInterPoints.PointInBounds(mp5));
		}

		[Test]
		public void MPL_ToArrayTest()
		{
			ModelPoint[] expectedArray = new ModelPoint[] {
				new ModelPoint(0.0f, initStrength),
				mp1, mp3, mp2,
				new ModelPoint(lifetime, 0.0f)
			};
			ModelPoint[] actualArray = listWithInterPoints.ToArray();

			Assert.AreEqual(expectedArray, actualArray);
		}

		//[Test]
		public void MPL_OrderTest()
		{
			
		}

		public bool AddToList(ModelPoint point)
		{
			ModelPointList pointList = new ModelPointList(initStrength, lifetime);
			bool status = pointList.Add(point);
			return status;
		}

		[Test]
		public void MPL_AddPointTest()
		{
			ModelPointList pointList = new ModelPointList(initStrength, lifetime);
			ModelPoint mp = new ModelPoint(2.0f, 3.5f);
			bool status = pointList.Add(mp);

			Assert.IsTrue(status && mp == pointList[1]);
		}

		[Test]
		public void MPL_AddExistingPointTest()
		{
			bool status = listWithInterPoints.Add(mp1);
			Assert.IsFalse(status);
		}

		[Test]
		public void MPL_AddOutBoundPointTest()
		{
			Assert.IsFalse(AddToList(mp5));
		}

		[Test]
		public void MPL_RemovePointTest()
		{
			ModelPointList pointList = new ModelPointList(initStrength, lifetime);

		}

	}
}
