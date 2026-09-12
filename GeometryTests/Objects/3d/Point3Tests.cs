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

using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    public class Point3Tests
    {
        [Test]
        [Category("Point3")]
        public void Point3_OperatorsAndEquality_Pass()
        {
            var p1 = new Point3(1f, 2f, 3f);
            var v = new Vector3(4f, 5f, 6f);
            var p2 = p1 + v;
            var p3 = p2 - v;

            Assert.That(p2.X == 5f && p2.Y == 7f && p2.Z == 9f, Is.True);
            Assert.That(p3.Equals(p1), Is.True);
            Assert.That(p1.Equals((object)p1), Is.True);
            Assert.That(p1.Equals(null), Is.False);
        }

        [Test]
        [Category("Point3")]
        public void Point3_DefaultConstructor_Pass()
        {
            var p = new Point3();

            Assert.That(p.X, Is.EqualTo(0f));
            Assert.That(p.Y, Is.EqualTo(0f));
            Assert.That(p.Z, Is.EqualTo(0f));
        }

        [Test]
        [Category("Point3")]
        public void Point3_FromPoint2Constructor_Pass()
        {
            var point2 = new Point2(1f, 2f);
            var point3 = new Point3(point2);

            Assert.That(point3.X, Is.EqualTo(1f));
            Assert.That(point3.Y, Is.EqualTo(2f));
            Assert.That(point3.Z, Is.EqualTo(0f));
        }

        [Test]
        [Category("Point3")]
        public void Point3_Equals_Pass()
        {
            var p = new Point3(1f, 2f, 3f);
            var same = new Point3(1f, 2f, 3f);
            var differentX = new Point3(9f, 2f, 3f);
            var differentY = new Point3(1f, 9f, 3f);
            var differentZ = new Point3(1f, 2f, 9f);

            Assert.That(p.Equals(p), Is.True);
            Assert.That(p.Equals(same), Is.True);
            Assert.That(p.Equals(differentX), Is.False);
            Assert.That(p.Equals(differentY), Is.False);
            Assert.That(p.Equals(differentZ), Is.False);

            Assert.That(p.Equals((object)p), Is.True);
            Assert.That(p.Equals((object)same), Is.True);
            Assert.That(p.Equals(null), Is.False);
            Assert.That(p.Equals("not a point"), Is.False);

            Assert.That(p.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Point3")]
        public void Point3_ZeroAndOne_Pass()
        {
            Assert.That(Point3.ZERO.X, Is.EqualTo(0f));
            Assert.That(Point3.ZERO.Y, Is.EqualTo(0f));
            Assert.That(Point3.ZERO.Z, Is.EqualTo(0f));

            Assert.That(Point3.ONE.X, Is.EqualTo(1f));
            Assert.That(Point3.ONE.Y, Is.EqualTo(1f));
            Assert.That(Point3.ONE.Z, Is.EqualTo(1f));
        }

        [Test]
        [Category("Point3")]
        public void Point3_ToString_Pass()
        {
            Assert.That(new Point3(1f, -2f, 3f).ToString(), Is.EqualTo("(1, -2, 3)"));
        }
    }
}


