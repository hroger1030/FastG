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
    public class SphereTests
    {
        [Test]
        [Category("Sphere")]
        public void Sphere_VolumeAndSurfaceArea_Pass()
        {
            var sphere = new Sphere(new Point3(0f, 0f, 0f), 2f);
            Assert.That(sphere.Volume, Is.EqualTo((4f / 3f) * MathF.PI * 8f).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(sphere.SurfaceArea, Is.EqualTo(4f * MathF.PI * 4f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Sphere")]
        public void Sphere_InvalidRadius_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sphere(new Point3(0f, 0f, 0f), 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sphere(new Point3(0f, 0f, 0f), -1f));
        }

        [Test]
        [Category("Sphere")]
        public void Sphere_Equals_Pass()
        {
            var sphere = new Sphere(new Point3(1f, 1f, 1f), 2f);
            var same = new Sphere(new Point3(1f, 1f, 1f), 2f);
            var differentCenter = new Sphere(new Point3(2f, 1f, 1f), 2f);
            var differentRadius = new Sphere(new Point3(1f, 1f, 1f), 3f);

            Assert.That(sphere.Equals(sphere), Is.True);
            Assert.That(sphere.Equals(same), Is.True);
            Assert.That(sphere.Equals(differentCenter), Is.False);
            Assert.That(sphere.Equals(differentRadius), Is.False);

            Assert.That(sphere.Equals((object)same), Is.True);
            Assert.That(sphere.Equals(null), Is.False);
            Assert.That(sphere.Equals("not a sphere"), Is.False);

            Assert.That(sphere.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Sphere")]
        public void Sphere_UnitSphere_Pass()
        {
            Assert.That(Sphere.UNIT_SPHERE.Center, Is.EqualTo(Point3.ZERO));
            Assert.That(Sphere.UNIT_SPHERE.Radius, Is.EqualTo(1f));
            Assert.That(Sphere.UNIT_SPHERE.Volume, Is.EqualTo((4f / 3f) * MathF.PI).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(Sphere.UNIT_SPHERE.SurfaceArea, Is.EqualTo(4f * MathF.PI).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Sphere")]
        public void Sphere_RawFloatConstructor_Pass()
        {
            var fromFloats = new Sphere(1f, 2f, 3f, 4f);
            var fromPoint = new Sphere(new Point3(1f, 2f, 3f), 4f);

            Assert.That(fromFloats, Is.EqualTo(fromPoint));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sphere(0f, 0f, 0f, 0f));
        }

        [Test]
        [Category("Sphere")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void Sphere_Scale_Pass(float scale)
        {
            var sphere = new Sphere(new Point3(1f, 2f, 3f), 2f);
            var scaled = sphere.Scale(scale);

            Assert.That(scaled.Center, Is.EqualTo(sphere.Center));
            Assert.That(scaled.Radius, Is.EqualTo(2f * scale));
        }

        [Test]
        [Category("Sphere")]
        [TestCase(0f)]
        [TestCase(-1f)]
        public void Sphere_Scale_ThrowsForNonPositiveScale_Fail(float scale)
        {
            var sphere = new Sphere(new Point3(0f, 0f, 0f), 2f);

            Assert.Throws<ArgumentOutOfRangeException>(() => sphere.Scale(scale));
        }

        [Test]
        [Category("Sphere")]
        public void Sphere_ToString_Pass()
        {
            var sphere = new Sphere(1f, 2f, 3f, 4f);

            Assert.That(sphere.ToString(), Is.EqualTo("Sphere(Center: (1, 2, 3), Radius: 4)"));
        }
    }
}



