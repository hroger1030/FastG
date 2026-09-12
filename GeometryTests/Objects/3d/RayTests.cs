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
    public class RayTests
    {
        [Test]
        [Category("Ray")]
        public void Ray_PointAt_Pass()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            Assert.That(ray.Direction.Length(), Is.EqualTo(1f).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(ray.PointAt(5f), Is.EqualTo(new Point3(0f, 0f, 0f)));
        }

        [Test]
        [Category("Ray")]
        public void Ray_Constructor_ZeroDirection_Fail()
        {
            Assert.Throws<ArgumentException>(() => new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)));
        }

        [Test]
        [Category("Ray")]
        public void Ray_Equals_Pass()
        {
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 1f));
            var same = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 1f));
            var different = new Ray(new Point3(1f, 0f, 0f), new Vector3(0f, 0f, 1f));

            Assert.That(ray.Equals(ray), Is.True);
            Assert.That(ray.Equals(same), Is.True);
            Assert.That(ray.Equals(different), Is.False);

            Assert.That(ray.Equals((object)same), Is.True);
            Assert.That(ray.Equals(null), Is.False);
            Assert.That(ray.Equals("not a ray"), Is.False);

            Assert.That(ray.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Ray")]
        public void Ray_UnitRay_Pass()
        {
            Assert.That(Ray.UNIT_RAY.Origin, Is.EqualTo(Point3.ZERO));
            Assert.That(Ray.UNIT_RAY.Direction.X, Is.EqualTo(Constants.INV_SQRT_3).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(Ray.UNIT_RAY.Direction.Y, Is.EqualTo(Constants.INV_SQRT_3).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(Ray.UNIT_RAY.Direction.Z, Is.EqualTo(Constants.INV_SQRT_3).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(Ray.UNIT_RAY.Direction.Length(), Is.EqualTo(1f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Ray")]
        public void Ray_RawFloatConstructor_Pass()
        {
            var fromFloats = new Ray(1f, 2f, 3f, 0f, 0f, 2f);
            var fromObjects = new Ray(new Point3(1f, 2f, 3f), new Vector3(0f, 0f, 2f));

            Assert.That(fromFloats, Is.EqualTo(fromObjects));
            // direction is normalized on construction
            Assert.That(fromFloats.Direction, Is.EqualTo(new Vector3(0f, 0f, 1f)));
            Assert.Throws<ArgumentException>(() => new Ray(0f, 0f, 0f, 0f, 0f, 0f));
        }

        [Test]
        [Category("Ray")]
        public void Ray_ToString_Pass()
        {
            var ray = new Ray(1f, 2f, 3f, 0f, 0f, 1f);

            Assert.That(ray.ToString(), Is.EqualTo("Ray(Origin: (1, 2, 3), Direction: <0, 0, 1>)"));
        }
    }
}


