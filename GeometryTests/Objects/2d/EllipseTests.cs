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
    public class EllipseTests
    {
        [Test]
        [Category("Ellipse")]
        public void Ellipse_Area_Pass()
        {
            var ellipse = new Ellipse(Point2.ZERO, 2f, 1f);
            Assert.That(ellipse.Area, Is.EqualTo(Constants.TWO_PI * 1f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_InvalidRadii_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ellipse(new Point2(0f, 0f), 0f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ellipse(new Point2(0f, 0f), 1f, 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ellipse(new Point2(0f, 0f), -1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ellipse(new Point2(0f, 0f), 1f, -1f));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_Perimeter_Pass()
        {
            var circleAsEllipse = new Ellipse(new Point2(0f, 0f), 2f, 2f);
            Assert.That(circleAsEllipse.Perimeter, Is.EqualTo(Constants.TWO_PI * 2f).Within(Constants.FLOAT_ERROR_MARGIN));

            var ellipse = new Ellipse(new Point2(0f, 0f), 3f, 1f);
            Assert.That(ellipse.Perimeter, Is.GreaterThan(0f));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_Equals_Pass()
        {
            var ellipse = new Ellipse(new Point2(1f, 1f), 2f, 3f);
            var same = new Ellipse(new Point2(1f, 1f), 2f, 3f);
            var differentCenter = new Ellipse(new Point2(2f, 1f), 2f, 3f);
            var differentRadiusX = new Ellipse(new Point2(1f, 1f), 4f, 3f);
            var differentRadiusY = new Ellipse(new Point2(1f, 1f), 2f, 5f);

            Assert.That(ellipse.Equals(ellipse), Is.True);
            Assert.That(ellipse.Equals(same), Is.True);
            Assert.That(ellipse.Equals(differentCenter), Is.False);
            Assert.That(ellipse.Equals(differentRadiusX), Is.False);
            Assert.That(ellipse.Equals(differentRadiusY), Is.False);

            Assert.That(ellipse.Equals((object)same), Is.True);
            Assert.That(ellipse.Equals(null), Is.False);
            Assert.That(ellipse.Equals("not an ellipse"), Is.False);

            Assert.That(ellipse.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_ReferenceEllipse_Pass()
        {
            Assert.That(Ellipse.REFERENCE_ELLIPSE.Center.X, Is.EqualTo(0f));
            Assert.That(Ellipse.REFERENCE_ELLIPSE.Center.Y, Is.EqualTo(0f));
            Assert.That(Ellipse.REFERENCE_ELLIPSE.RadiusX, Is.EqualTo(1f));
            Assert.That(Ellipse.REFERENCE_ELLIPSE.RadiusY, Is.EqualTo(2f));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_RawFloatConstructor_Pass()
        {
            var fromFloats = new Ellipse(1f, 2f, 3f, 4f);
            var fromPoint = new Ellipse(new Point2(1f, 2f), 3f, 4f);

            Assert.That(fromFloats, Is.EqualTo(fromPoint));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ellipse(0f, 0f, 0f, 1f));
        }

        [Test]
        [Category("Ellipse")]
        public void Ellipse_ToString_Pass()
        {
            var ellipse = new Ellipse(1f, 2f, 3f, 4f);

            Assert.That(ellipse.ToString(), Is.EqualTo("Ellipse(Center: (1, 2), RadiusX: 3, RadiusY: 4)"));
        }
    }
}
