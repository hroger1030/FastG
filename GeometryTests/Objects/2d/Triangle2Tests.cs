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
    public class TriangleTests
    {
        [Test]
        [Category("Triangle2")]
        public void Triangle_AreaPerimeterType_Pass()
        {
            var equilateral = new Triangle2(new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0.5f, Constants.SQRT_3 / 2f));

            Assert.That(equilateral.TriangleType, Is.EqualTo(Triangle2.Type.Equilateral));
            Assert.That(equilateral.Perimeter, Is.EqualTo(3f));
            Assert.That(equilateral.Area, Is.Positive);

            var isosceles = new Triangle2(new Point2(0f, 0f), new Point2(2f, 0f), new Point2(1f, 1f));
            Assert.That(isosceles.TriangleType, Is.EqualTo(Triangle2.Type.Isosceles));

            var scalene = new Triangle2(new Point2(0f, 0f), new Point2(2f, 0f), new Point2(0f, 3f));
            Assert.That(scalene.TriangleType, Is.EqualTo(Triangle2.Type.Scalene));
        }

        [Test]
        [Category("Triangle2")]
        public void Triangle_DuplicatePoint_Fail()
        {
            var p = new Point2(0f, 0f);
            Assert.Throws<ArgumentException>(() => new Triangle2(p, p, new Point2(1f, 0f)));
        }

        [Test]
        [Category("Triangle2")]
        public void Triangle_UnitTriangle_Pass()
        {
            Assert.That(Triangle2.UNIT_TRIANGLE.A, Is.EqualTo(new Point2(0f, 0f)));
            Assert.That(Triangle2.UNIT_TRIANGLE.B, Is.EqualTo(new Point2(0f, 1f)));
            Assert.That(Triangle2.UNIT_TRIANGLE.C, Is.EqualTo(new Point2(1f, 0f)));

            Assert.That(Triangle2.UNIT_TRIANGLE.Area, Is.EqualTo(0.5f).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(Triangle2.UNIT_TRIANGLE.Perimeter, Is.EqualTo(2f + MathF.Sqrt(2f)).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Triangle2")]
        public void Triangle_RawFloatConstructor_Pass()
        {
            var fromFloats = new Triangle2(0f, 0f, 0f, 1f, 1f, 0f);
            var fromPoints = new Triangle2(new Point2(0f, 0f), new Point2(0f, 1f), new Point2(1f, 0f));

            Assert.That(fromFloats, Is.EqualTo(fromPoints));
            Assert.Throws<ArgumentException>(() => new Triangle2(0f, 0f, 0f, 0f, 1f, 0f));
        }

        [Test]
        [Category("Triangle2")]
        public void Triangle_ToString_Pass()
        {
            var triangle = new Triangle2(0f, 0f, 0f, 1f, 1f, 0f);

            Assert.That(triangle.ToString(), Is.EqualTo("Triangle2(A: (0, 0), B: (0, 1), C: (1, 0))"));
        }
    }
}



