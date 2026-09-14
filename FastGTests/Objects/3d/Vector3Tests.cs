/*
The MIT License (MIT)

Copyright (c) 2017 Roger Hill

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do 
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using FastG;
using NUnit.Framework;
using System;

namespace FastGTests
{
    [TestFixture]
    public class Vector3Tests
    {
        [Test]
        [Category("Vector3")]
        public void TestDistanceOperations()
        {
            Vector3 v1;
            float distance;

            v1 = new Vector3(0, 0, 3);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(distance == 3f, Is.True, "Expected distance of 3");

            v1 = new Vector3(3, 4, 0);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(distance == 5f, Is.True, "Expected distance of 5");

            v1 = new Vector3(-1, 0, 0);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(distance == 1f, Is.True, "Expected distance of 1");

            v1 = Vector3.ZERO;
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(distance == 0f, Is.True, "Expected distance of 0");

            v1 = new Vector3(float.NaN, 0, 0);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(float.IsNaN(distance), Is.True, "Expected distance of float.NaN");

            v1 = new Vector3(float.PositiveInfinity, 0, 0);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(float.IsPositiveInfinity(distance), Is.True, "Expected distance of float.PositiveInfinity");

            v1 = new Vector3(float.NegativeInfinity, 0, 0);
            distance = Vector3.ZERO.DistanceTo(v1);
            Assert.That(float.IsPositiveInfinity(distance), Is.True, "Expected distance of float.PositiveInfinity");
        }

        [Test]
        [Category("Vector3")]
        public void TestOperatorOverloads()
        {
            var v1 = new Vector3(3, 3, 3);
            var v2 = new Vector3(1, 2, 3);

            var v3 = v1 + v2;
            Assert.That(v3.X == 4f && v3.Y == 5 && v3.Z == 6, Is.True, "Failed addition");

            v3 = v1 - v2;
            Assert.That(v3.X == 2f && v3.Y == 1 && v3.Z == 0, Is.True, "Failed subtraction");

            v3 = v1 * 2;
            Assert.That(v3.X == 6f && v3.Y == 6 && v3.Z == 6, Is.True, "Failed multiplication");

            v3 = v1 / 2;
            Assert.That(v3.X == 1.5f && v3.Y == 1.5 && v3.Z == 1.5, Is.True, "Failed division");
        }

        [Test]
        [Category("Vector3")]
        public void TestNormalize_ZeroVector_Fail()
        {
            var v = new Vector3(0f, 0f, 0f);

            Assert.Throws<DivideByZeroException>(() => Vector3.Normalize(v));
        }

        [Test]
        [Category("Vector3")]
        public void TestEqualsAndHashCode()
        {
            var v1 = new Vector3(1f, 2f, 3f);
            var v2 = new Vector3(1f, 2f, 3f);

            Assert.That(v1.Equals(v2), Is.True);
            Assert.That(v1.Equals((object)v2), Is.True);
            Assert.That(v1.GetHashCode(), Is.EqualTo(v2.GetHashCode()));
            Assert.That(v1.Equals(null), Is.False);
        }

        [Test]
        [Category("Vector3")]
        public void Vector3_ZeroAndOne_Pass()
        {
            Assert.That(Vector3.ZERO.X, Is.EqualTo(0f));
            Assert.That(Vector3.ZERO.Y, Is.EqualTo(0f));
            Assert.That(Vector3.ZERO.Z, Is.EqualTo(0f));

            Assert.That(Vector3.ONE.X, Is.EqualTo(1f));
            Assert.That(Vector3.ONE.Y, Is.EqualTo(1f));
            Assert.That(Vector3.ONE.Z, Is.EqualTo(1f));
        }

        [Test]
        [Category("Vector3")]
        public void Vector3_ToString_Pass()
        {
            Assert.That(new Vector3(1f, -2f, 3f).ToString(), Is.EqualTo("<1, -2, 3>"));
        }

        [Test]
        [Category("Vector3")]
        public void TestDot_Pass()
        {
            var v1 = new Vector3(1f, 2f, 3f);
            var v2 = new Vector3(4f, -5f, 6f);

            Assert.That(Vector3.Dot(v1, v2), Is.EqualTo(12f));
            Assert.That(v1.Dot(v2), Is.EqualTo(12f));

            var x = new Vector3(1f, 0f, 0f);
            var y = new Vector3(0f, 1f, 0f);
            Assert.That(x.Dot(y), Is.EqualTo(0f));
            Assert.That(v1.Dot(v1), Is.EqualTo(v1.LengthSquared));
        }

        [Test]
        [Category("Vector3")]
        public void TestCross_Pass()
        {
            var x = new Vector3(1f, 0f, 0f);
            var y = new Vector3(0f, 1f, 0f);
            var z = new Vector3(0f, 0f, 1f);

            Assert.That(Vector3.Cross(x, y), Is.EqualTo(z));
            Assert.That(x.Cross(y), Is.EqualTo(z));
            Assert.That(y.Cross(x), Is.EqualTo(new Vector3(0f, 0f, -1f)));
            Assert.That(x.Cross(x), Is.EqualTo(Vector3.ZERO));
        }

        [Test]
        [Category("Vector3")]
        public void Vector3_LengthSquaredAndDistanceSquared_Pass()
        {
            var v = new Vector3(2f, 3f, 6f);

            Assert.That(v.LengthSquared, Is.EqualTo(49f));
            Assert.That(v.Length, Is.EqualTo(7f));

            var a = new Vector3(1f, 2f, 3f);
            var b = new Vector3(1f, 6f, 6f);
            Assert.That(a.DistanceSquaredTo(b), Is.EqualTo(25f));
            Assert.That(Vector3.DistanceSquaredTo(a, b), Is.EqualTo(25f));
            Assert.That(a.DistanceTo(b), Is.EqualTo(5f));
        }

        [Test]
        [Category("Vector3")]
        public void Vector3_I1dLength_Pass()
        {
            I1d v = new Vector3(2f, 3f, 6f);

            Assert.That(v.Length, Is.EqualTo(7f));
        }
    }
}



