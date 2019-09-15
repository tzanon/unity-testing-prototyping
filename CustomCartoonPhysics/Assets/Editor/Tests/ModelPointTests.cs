using CustomPhysics;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public class ModelPointTests
	{
		ModelPoint mp1 = new ModelPoint(0.0f, 10.0f);
		ModelPoint mp2 = new ModelPoint(0.0f, 10.0f);
		ModelPoint mp3 = new ModelPoint(3.0f, 10.0f);
		ModelPoint mp4 = new ModelPoint(6.0f, 2.0f);
		ModelPoint mp5 = new ModelPoint(6.0f, 7.5f);
		ModelPoint mp6 = new ModelPoint(8.0f, 0.0f);
		
		Vector2 vec1 = new Vector2(0.0f, 10.0f);
		Vector2 vec2 = new Vector2(5.0f, 17.0f);
		readonly ModelPointComparer comparer = new ModelPointComparer();
		readonly ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);

		#region Model point tests

		[Test]
		public void MPEqTest()
		{
			Assert.AreEqual(mp1, mp2);
		}

		[Test]
		public void MPNotEqTest()
		{
			Assert.AreNotEqual(mp1, mp3);
		}

		[Test]
		public void MPEqVector2Test()
		{
			Assert.IsTrue(mp1 == vec1);
		}

		[Test]
		public void MPNotEqVector2Test()
		{
			Assert.IsTrue(mp1 != vec2);
		}

		[Test]
		public void MPAsVectorTest()
		{
			Vector2 mpVector = mp1.AsVector2();
			Assert.AreEqual(vec1, mpVector);
		}

		[Test]
		public void MPLessThanTest()
		{
			int cmpRes = comparer.Compare(mp1, mp3);
			Assert.Negative(cmpRes);
		}

		[Test]
		public void MPGreaterThanTest()
		{
			int cmpRes = comparer.Compare(mp4, mp3);
			Assert.Positive(cmpRes);
		}

		[Test]
		public void MPEqualCmpTest()
		{
			int cmpRes = comparer.Compare(mp4, mp5);
			Assert.Zero(cmpRes);
		}

		#endregion

		#region Domain tests

		[Test]
		public void DomainContainsMinTest()
		{
			bool domainContains = domain.Contains(mp1);
			Assert.IsTrue(domainContains);
		}

		[Test]
		public void DomainContainsMaxTest()
		{
			bool domainContains = domain.Contains(mp5);
			Assert.IsFalse(domainContains);
		}

		[Test]
		public void DomainContainTest()
		{
			bool domainContains = domain.Contains(mp3);
			Assert.IsTrue(domainContains);
		}

		[Test]
		public void DomainDoesNotContainTest()
		{
			bool domainContains = domain.Contains(mp6);
			Assert.IsFalse(domainContains);
		}

		#endregion

		#region Line tests

		private static readonly ModelPoint p1 = new ModelPoint(0.0f, 6.0f);
		private static readonly ModelPoint p2 = new ModelPoint(3.0f, 0.0f);
		private static readonly ModelPoint p3 = new ModelPoint(0.0f, 9.0f);
		private static readonly ModelPoint p4 = new ModelPoint(4.0f, 0.0f);
		private static readonly ModelPoint p5 = new ModelPoint(2.0f, 9.0f);
		private static readonly ModelPoint p6 = new ModelPoint(7.0f, 5.0f);

		private readonly ModelLine line1 = new ModelLine(p1, p2);
		private readonly ModelLine line2 = new ModelLine(p3, p4);
		private readonly ModelLine line3 = new ModelLine(p5, p6);

		[Test]
		public void LineSlopeTest1()
		{
			Assert.AreEqual(line1.Slope, -2.0f);
		}

		[Test]
		public void LineYIntTest1()
		{
			Assert.AreEqual(line1.YIntercept, 6.0f);
		}

		[Test]
		public void LineSlopeTest2()
		{
			Assert.AreEqual(line2.Slope, -2.25f);
		}
		
		[Test]
		public void LineYIntTest2()
		{
			//Assert.IsTrue(line2.YIntercept == 9.0f);
			Assert.AreEqual(line2.YIntercept, 9.0f);
		}

		[Test]
		public void LineSlopeTest3()
		{
			Assert.AreEqual(line3.Slope, -0.8f);
		}

		[Test]
		public void LineYIntTest3()
		{
			Assert.AreEqual(line3.YIntercept, 10.6f);
		}

		#endregion

	}
}
