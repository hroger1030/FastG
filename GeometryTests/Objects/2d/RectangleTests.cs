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
    public class RectangleTests
    {
        [Test]
        [Category("Rectangle")]
        public void TestBasicProperties()
        {
            var r = new Rectangle(0, 0, 2, 3);

            Assert.That(r.Top == 0f, Is.True, "Failed top check");
            Assert.That(r.Left == 0f, Is.True, "Failed left check");

            Assert.That(r.Width == 2f, Is.True, "Failed width check");
            Assert.That(r.Height == 3f, Is.True, "Failed height check");

            Assert.That(r.Perimeter == 10f, Is.True, "Failed perimeter check");
            Assert.That(r.Area == 6f, Is.True, "Failed area check");

            Assert.That(r.Right == 2f, Is.True, "Failed right side check");
            Assert.That(r.Bottom == 3f, Is.True, "Failed bottom side check");
            Assert.That(r.Top == 0f, Is.True, "Failed top side check");
            Assert.That(r.Left == 0f, Is.True, "Failed left side check");

            Assert.That(r.Center.X == 1f, Is.True, "Failed center X check");
            Assert.That(r.Center.Y == 1.5f, Is.True, "Failed center X check");
        }

        [Test]
        [Category("Rectangle")]
        [Category("Math")]
        [TestCase(1f, 1f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(-0.5f, -0.5f)]
        public void Rectangle_Scale_Pass(float x, float y)
        {
            var r = new Rectangle(0f, 0f, 2f, 2f);
            r = r.Scale(x, y);

            Assert.That(r.Width == (2f * x), Is.True, "Failed width check");
            Assert.That(r.Height == (2f * y), Is.True, "Failed height check");
        }

        [Test]
        [Category("Rectangle")]
        public void TestUnion()
        {
            var r1 = new Rectangle(0, 0, 2, 2);
            var r2 = new Rectangle(1, 1, 2, 2);
            var r3 = Rectangle.Union(r1, r2);

            Assert.That(r3.X == 0f, Is.True, "Failed X check");
            Assert.That(r3.Y == 0f, Is.True, "Failed Y check");
            Assert.That(r3.Width == 3f, Is.True, "Failed width check");
            Assert.That(r3.Height == 3f, Is.True, "Failed height check");
        }

        [Test]
        [Category("Rectangle")]
        public void TestOperatorOverloads()
        {
            var r1 = new Rectangle(0, 0, 2, 3);
            var v1 = new Vector2(1f, 1f);

            var r2 = r1 + v1;
            Assert.That(r2.Top == 1f && r2.Top == 1f && r2.Height == 3f && r2.Width == 2f, Is.True, "Failed add check");

            r2 = r1 - v1;
            Assert.That(r2.Top == -1f && r2.Top == -1f && r2.Height == 3f && r2.Width == 2f, Is.True, "Failed subtract check");

            r2 = r1 * 3f;
            Assert.That(r2.Top == 0f && r2.Top == 0f && r2.Height == 9f && r2.Width == 6f, Is.True, "Failed multiply check");

            r2 = r1 / 2f;
            Assert.That(r2.Top == 0f && r2.Top == 0f && r2.Height == 1.5f && r2.Width == 1f, Is.True, "Failed divide check");

            r2 = new Rectangle(0, 0, 2, 3);
            Assert.That(r1.Equals(r2), Is.True, "Failed equals check");

            r2 = new Rectangle(1, 2, 4, 5);
            Assert.That(r1.Equals(r2), Is.False, "Failed not equals check");
        }

        [Test]
        [Category("Rectangle")]
        public void Rectangle_ConstructorInvalidDimensions_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(0, 0, 0f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(0, 0, 1f, 0f));
        }

        [Test]
        [Category("Rectangle")]
        public void Rectangle_Union_Pass()
        {
            var r1 = new Rectangle(1, 1, 2, 2);
            var r2 = new Rectangle(0, 0, 1, 1);
            var union = Rectangle.Union(r1, r2);

            Assert.That(union.Left, Is.EqualTo(0f));
            Assert.That(union.Top, Is.EqualTo(0f));
            Assert.That(union.Width, Is.EqualTo(3f));
            Assert.That(union.Height, Is.EqualTo(3f));
        }

        [Test]
        [Category("Rectangle")]
        public void Rectangle_UnitRectangle_Pass()
        {
            Assert.That(Rectangle.UNIT_RECTANGLE.Left, Is.EqualTo(0f));
            Assert.That(Rectangle.UNIT_RECTANGLE.Top, Is.EqualTo(0f));
            Assert.That(Rectangle.UNIT_RECTANGLE.Width, Is.EqualTo(1f));
            Assert.That(Rectangle.UNIT_RECTANGLE.Height, Is.EqualTo(1f));
            Assert.That(Rectangle.UNIT_RECTANGLE.Area, Is.EqualTo(1f));
        }

        [Test]
        [Category("Rectangle")]
        public void Rectangle_ToString_Pass()
        {
            Assert.That(new Rectangle(1f, 2f, 3f, 4f).ToString(), Is.EqualTo("Rectangle(Left: 1, Top: 2, Width: 3, Height: 4)"));
        }

    }
}



