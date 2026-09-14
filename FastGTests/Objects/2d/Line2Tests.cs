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
    public class Line2Tests
    {
        [Test]
        [Category("Line2")]
        public void Line2_LengthAndEquality_Pass()
        {
            var line = new Line2(new Point2(0f, 0f), new Point2(3f, 4f));

            Assert.That(line.Length, Is.EqualTo(5f));
            Assert.That(line.Equals(new Line2(new Point2(0f, 0f), new Point2(3f, 4f))), Is.True);
            Assert.That(line.ToString().Contains("Point1"), Is.True);
        }

        [Test]
        [Category("Line2")]
        public void Line2_FloatConstructor_Pass()
        {
            var line = new Line2(0f, 0f, 3f, 4f);

            Assert.That(line.Point1, Is.EqualTo(new Point2(0f, 0f)));
            Assert.That(line.Point2, Is.EqualTo(new Point2(3f, 4f)));
            Assert.That(line.Length, Is.EqualTo(5f));
        }

        [Test]
        [Category("Line2")]
        public void Line2_Equals_Pass()
        {
            var line = new Line2(new Point2(0f, 0f), new Point2(3f, 4f));
            var same = new Line2(new Point2(0f, 0f), new Point2(3f, 4f));
            var different = new Line2(new Point2(0f, 0f), new Point2(1f, 1f));

            Assert.That(line.Equals(line), Is.True);
            Assert.That(line.Equals(same), Is.True);
            Assert.That(line.Equals(different), Is.False);

            Assert.That(line.Equals((object)same), Is.True);
            Assert.That(line.Equals(null), Is.False);
            Assert.That(line.Equals("not a line"), Is.False);

            Assert.That(line.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Line2")]
        public void Line2_UnitLine_Pass()
        {
            Assert.That(Line2.UNIT_LINE.Point1, Is.EqualTo(Point2.ZERO));
            Assert.That(Line2.UNIT_LINE.Point2, Is.EqualTo(Point2.ONE));
            Assert.That(Line2.UNIT_LINE.Length, Is.EqualTo(Constants.SQRT_2).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Line2")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void Line2_Scale_Pass(float scale)
        {
            var line = new Line2(new Point2(0f, 0f), new Point2(4f, 0f));
            var scaled = line.Scale(scale);

            Assert.That(scaled.Length, Is.EqualTo(4f * scale).Within(Constants.FLOAT_ERROR_MARGIN));

            float midX = (line.Point1.X + line.Point2.X) / 2f;
            float scaledMidX = (scaled.Point1.X + scaled.Point2.X) / 2f;
            Assert.That(scaledMidX, Is.EqualTo(midX).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Line2")]
        [TestCase(0f)]
        [TestCase(-1f)]
        public void Line2_Scale_ThrowsForNonPositiveScale_Fail(float scale)
        {
            var line = new Line2(new Point2(0f, 0f), new Point2(4f, 0f));

            Assert.Throws<ArgumentOutOfRangeException>(() => line.Scale(scale));
        }

        [Test]
        [Category("Line2")]
        public void Line2_ToString_Pass()
        {
            Assert.That(new Line2(0f, 1f, 2f, 3f).ToString(), Is.EqualTo("Line2(Point1: (0, 1), Point2: (2, 3))"));
        }
    }
}


