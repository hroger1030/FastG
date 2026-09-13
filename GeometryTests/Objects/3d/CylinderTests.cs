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
    public class CylinderTests
    {
        [Test]
        [Category("Cylinder")]
        public void Cylinder_RawFloatConstructor_Pass()
        {
            var fromFloats = new Cylinder(0f, 0f, 0f, 0f, 0f, 2f, 1f);
            var fromPoints = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            Assert.That(fromFloats, Is.EqualTo(fromPoints));
        }

        [Test]
        [Category("Cylinder")]
        public void Cylinder_ZeroOrNegativeRadius_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cylinder(0f, 0f, 0f, 0f, 0f, 2f, 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cylinder(0f, 0f, 0f, 0f, 0f, 2f, -1f));
        }

        [Test]
        [Category("Cylinder")]
        public void Cylinder_EqualEndpoints_Fail()
        {
            Assert.Throws<ArgumentException>(() => new Cylinder(new Point3(1f, 1f, 1f), new Point3(1f, 1f, 1f), 1f));
        }

        [Test]
        [Category("Cylinder")]
        public void Cylinder_HeightVolumeAndSurfaceArea_Pass()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            Assert.That(cylinder.Height, Is.EqualTo(2f));
            Assert.That(cylinder.Volume, Is.EqualTo(MathF.PI * 1f * 1f * 2f).Within(1e-5f));
            Assert.That(cylinder.SurfaceArea, Is.EqualTo((2f * MathF.PI * 1f * 2f) + (2f * MathF.PI * 1f * 1f)).Within(1e-5f));
        }

        [Test]
        [Category("Cylinder")]
        public void Cylinder_EqualsHashCodeAndOperators_Pass()
        {
            var a = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);
            var same = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);
            var different = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 3f), 1f);

            Assert.That(a.Equals(same), Is.True);
            Assert.That(a.Equals(different), Is.False);
            Assert.That(a.Equals((object)same), Is.True);
            Assert.That(a.Equals("not a cylinder"), Is.False);

            Assert.That(a.GetHashCode(), Is.EqualTo(same.GetHashCode()));

            Assert.That(a == same, Is.True);
            Assert.That(a != different, Is.True);
        }

        [Test]
        [Category("Cylinder")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void Cylinder_Scale_Pass(float scale)
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var scaled = cylinder.Scale(scale);

            Assert.That(scaled.Radius, Is.EqualTo(1f * scale).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(scaled.Height, Is.EqualTo(cylinder.Height * scale).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Cylinder")]
        [TestCase(0f)]
        [TestCase(-1f)]
        public void Cylinder_Scale_ThrowsForNonPositiveScale_Fail(float scale)
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);

            Assert.Throws<ArgumentOutOfRangeException>(() => cylinder.Scale(scale));
        }

        [Test]
        [Category("Cylinder")]
        public void Cylinder_ToString_Pass()
        {
            var cylinder = new Cylinder(0f, 0f, 0f, 0f, 0f, 2f, 1f);

            Assert.That(cylinder.ToString(), Is.EqualTo("Cylinder(PointA: (0, 0, 0), PointB: (0, 0, 2), Radius: 1)"));
        }
    }
}
