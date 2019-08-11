using CustomPhysics;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
		ModelPointComparer comparer = new ModelPointComparer();
		ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);
		
		[Test]
		public void MPEqTest()
		{
			Assert.IsTrue(mp1 == mp2);
		}

		[Test]
		public void MPNotEqTest()
		{
			Assert.IsTrue(mp1 != mp3);
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
	}
}
