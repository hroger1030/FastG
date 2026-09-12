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
using System;

namespace GeometryTests
{
    [TestFixture]
    public class CapsuleTests
    {
        [Test]
        [Category("Capsule")]
        public void Capsule_ContainsAndIntersects_Pass()
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);
            Assert.That(capsule.Contains(new Point3(0f, 0f, 1f)), Is.True);
            Assert.That(capsule.Contains(new Point3(1f, 0f, 1f)), Is.True);
            Assert.That(capsule.Contains(new Point3(0f, 2f, 1f)), Is.False);

            var sphere = new Sphere(new Point3(0f, 0f, 3f), 1f);
            Assert.That(capsule.Intersects(sphere), Is.True);
            Assert.That(capsule.Intersects(new Sphere(new Point3(0f, 0f, 5f), 0.5f)), Is.False);
        }

        [Test]
        [Category("Capsule")]
        public void Capsule_RawFloatConstructor_Pass()
        {
            var fromFloats = new Capsule(0f, 0f, 0f, 0f, 0f, 2f, 1f);
            var fromPoints = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            Assert.That(fromFloats, Is.EqualTo(fromPoints));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Capsule(0f, 0f, 0f, 0f, 0f, 2f, -1f));
        }

        [Test]
        [Category("Capsule")]
        public void Capsule_ToString_Pass()
        {
            var capsule = new Capsule(0f, 0f, 0f, 0f, 0f, 2f, 1f);

            Assert.That(capsule.ToString(), Is.EqualTo("Capsule(PointA: (0, 0, 0), PointB: (0, 0, 2), Radius: 1)"));
        }
    }
}


