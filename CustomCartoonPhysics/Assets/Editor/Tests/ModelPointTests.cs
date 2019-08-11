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
		[Test]
		public void MPEqTest()
		{
			// Arrange
			ModelPoint mp1 = new ModelPoint(0.0f, 10.0f);
			ModelPoint mp2 = new ModelPoint(0.0f, 10.0f);

			// Assert
			Assert.IsTrue(mp1 == mp2);
		}

		[Test]
		public void MPNotEqTest()
		{
			// Arrange
			ModelPoint mp1 = new ModelPoint(0.0f, 10.0f);
			ModelPoint mp2 = new ModelPoint(30.0f, 10.0f);

			// Assert
			Assert.IsTrue(mp1 != mp2);
		}

		[Test]
		public void MPEqVector2Test()
		{
			// Arrange
			ModelPoint mp = new ModelPoint(0.0f, 10.0f);
			Vector2 v2 = new Vector2(0.0f, 10.0f);

			// Assert
			Assert.IsTrue(mp == v2);
		}

		[Test]
		public void MPNotEqVector2Test()
		{
			// Arrange
			ModelPoint mp = new ModelPoint(0.0f, 10.0f);
			Vector2 v2 = new Vector2(5.0f, 17.0f);

			// Assert
			Assert.IsTrue(mp != v2);
		}

		[Test]
		public void MPAsVectorTest()
		{
			// Arrange
			ModelPoint mp = new ModelPoint(0.0f, 10.0f);
			Vector2 expectedVector = new Vector2(0.0f, 10.0f);

			// Act
			Vector2 mpVector = mp.AsVector2();

			// Assert
			//Assert.That(mpVector, Is.EqualTo(expectedVector));
			Assert.AreEqual(expectedVector, mpVector);
		}

		[Test]
		public void MPLessThanTest()
		{
			// Arrange
			ModelPointComparer comparer = new ModelPointComparer();
			ModelPoint mp = new ModelPoint(0.0f, 10.0f);
			ModelPoint mp2 = new ModelPoint(3.0f, 6.0f);

			// Act
			int cmpRes = comparer.Compare(mp, mp2);

			// Assert
			//Assert.That(cmpRes, Is.LessThan(0));
			Assert.Negative(cmpRes);
		}

		[Test]
		public void MPGreaterThanTest()
		{
			// Arrange
			ModelPointComparer comparer = new ModelPointComparer();
			ModelPoint mp = new ModelPoint(6.0f, 5.0f);
			ModelPoint mp2 = new ModelPoint(3.0f, 6.0f);

			// Act
			int cmpRes = comparer.Compare(mp, mp2);

			// Assert
			//Assert.That(cmpRes, Is.GreaterThan(0));
			Assert.Positive(cmpRes);
		}

		[Test]
		public void MPEqualCmpTest()
		{
			// Arrange
			ModelPointComparer comparer = new ModelPointComparer();
			ModelPoint mp = new ModelPoint(3.0f, 2.0f);
			ModelPoint mp2 = new ModelPoint(3.0f, 6.0f);

			// Act
			int cmpRes = comparer.Compare(mp, mp2);

			// Assert
			//Assert.That(cmpRes, Is.EqualTo(0));
			Assert.Zero(cmpRes);
		}

		[Test]
		public void DomainContainsMin()
		{
			// Arrange
			ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);
			ModelPoint mp = new ModelPoint(0.0f, 10.0f);

			// Act
			bool domainContains = domain.Contains(mp);

			// Assert
			//Assert.That(domainContains, Is.EqualTo(expDomainContains));
			Assert.IsTrue(domainContains);
		}

		[Test]
		public void DomainContainsMax()
		{
			// Arrange
			ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);
			ModelPoint mp = new ModelPoint(6.0f, 10.0f);

			// Act
			bool domainContains = domain.Contains(mp);

			// Assert
			//Assert.That(domainContains, Is.EqualTo(expDomainNotContains));
			Assert.IsFalse(domainContains);
		}

		[Test]
		public void DomainContains()
		{
			// Arrange
			ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);
			ModelPoint mp = new ModelPoint(3.0f, 10.0f);

			// Act
			bool domainContains = domain.Contains(mp);

			// Assert
			//Assert.That(domainContains, Is.EqualTo(expDomainContains));
			Assert.IsTrue(domainContains);
		}

		[Test]
		public void DomainDoesNotContains()
		{
			// Arrange
			ModelTimeDomain domain = new ModelTimeDomain(0.0f, 6.0f);
			ModelPoint mp = new ModelPoint(8.0f, 10.0f);

			// Act
			bool domainContains = domain.Contains(mp);

			// Assert
			//Assert.That(domainContains, Is.EqualTo(expDomainNotContains));
			Assert.IsFalse(domainContains);
		}
	}
}
