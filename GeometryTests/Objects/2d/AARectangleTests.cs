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
    public class AARectangleTests
    {
        [Test]
        [Category("AARectangle")]
        public void AARectangle_BasicProperties_Pass()
        {
            var r = new AARectangle(0, 0, 2, 3);

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
        [Category("AARectangle")]
        [Category("Math")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void AARectangle_Scale_Pass(float scale)
        {
            var r = new AARectangle(0f, 0f, 2f, 2f);
            var center = r.Center;
            r = r.Scale(scale);

            Assert.That(r.Width, Is.EqualTo(2f * scale), "Failed width check");
            Assert.That(r.Height, Is.EqualTo(2f * scale), "Failed height check");
            Assert.That(r.Center, Is.EqualTo(center), "Failed to stay centered on the same point");
        }

        [Test]
        [Category("AARectangle")]
        [Category("Math")]
        [TestCase(0f)]
        [TestCase(-0.5f)]
        public void AARectangle_Scale_ThrowsForNonPositiveScale(float scale)
        {
            var r = new AARectangle(0f, 0f, 2f, 2f);

            Assert.Throws<ArgumentOutOfRangeException>(() => r.Scale(scale));
        }

        [Test]
        [Category("AARectangle")]
        [Category("Math")]
        [TestCase(5f, 5f)]
        [TestCase(-3f, 2f)]
        [TestCase(0f, 0f)]
        public void AARectangle_MoveTo_Pass(float x, float y)
        {
            var r = new AARectangle(0f, 0f, 4f, 2f);

            r = r.MoveTo(new Point2(x, y));

            Assert.That(r.Center, Is.EqualTo(new Point2(x, y)), "Failed to move to the target center");
            Assert.That(r.Width, Is.EqualTo(4f), "Failed width check");
            Assert.That(r.Height, Is.EqualTo(2f), "Failed height check");
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_UnionXY_Pass()
        {
            var r1 = new AARectangle(0, 0, 2, 2);
            var r2 = new AARectangle(1, 1, 2, 2);
            var r3 = AARectangle.Union(r1, r2);

            Assert.That(r3.X == 0f, Is.True, "Failed X check");
            Assert.That(r3.Y == 0f, Is.True, "Failed Y check");
            Assert.That(r3.Width == 3f, Is.True, "Failed width check");
            Assert.That(r3.Height == 3f, Is.True, "Failed height check");
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_OperatorOverloads_Pass()
        {
            var r1 = new AARectangle(0, 0, 2, 3);
            var v1 = new Vector2(1f, 1f);

            var r2 = r1 + v1;
            Assert.That(r2.Top == 1f && r2.Top == 1f && r2.Height == 3f && r2.Width == 2f, Is.True, "Failed add check");

            r2 = r1 - v1;
            Assert.That(r2.Top == -1f && r2.Top == -1f && r2.Height == 3f && r2.Width == 2f, Is.True, "Failed subtract check");

            r2 = r1 * 3f;
            Assert.That(r2.Height == 9f && r2.Width == 6f, Is.True, "Failed multiply check");
            Assert.That(r2.Center, Is.EqualTo(r1.Center), "Failed to stay centered on the same point");

            r2 = r1 / 2f;
            Assert.That(r2.Height == 1.5f && r2.Width == 1f, Is.True, "Failed divide check");
            Assert.That(r2.Center, Is.EqualTo(r1.Center), "Failed to stay centered on the same point");

            r2 = new AARectangle(0, 0, 2, 3);
            Assert.That(r1.Equals(r2), Is.True, "Failed equals check");

            r2 = new AARectangle(1, 2, 4, 5);
            Assert.That(r1.Equals(r2), Is.False, "Failed not equals check");
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_ConstructorInvalidDimensions_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AARectangle(0, 0, 0f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new AARectangle(0, 0, 1f, 0f));
        }

        [Test]
        [Category("AARectangle")]
        [Category("Math")]
        [TestCase(0f)] // sets width/height to 0
        [TestCase(-2f)] // sets width/height to negative
        public void AARectangle_MultiplyOrDivideByScale_ThrowsForNonPositiveScale(float scale)
        {
            var r = new AARectangle(0f, 0f, 2f, 2f);

            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = r * scale; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = r / scale; });
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_Union_Pass()
        {
            var r1 = new AARectangle(1, 1, 2, 2);
            var r2 = new AARectangle(0, 0, 1, 1);
            var union = AARectangle.Union(r1, r2);

            Assert.That(union.Left, Is.EqualTo(0f));
            Assert.That(union.Top, Is.EqualTo(0f));
            Assert.That(union.Width, Is.EqualTo(3f));
            Assert.That(union.Height, Is.EqualTo(3f));
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_Unit_Pass()
        {
            Assert.That(AARectangle.UNIT_AARECTANGLE.Left, Is.EqualTo(0f));
            Assert.That(AARectangle.UNIT_AARECTANGLE.Top, Is.EqualTo(0f));
            Assert.That(AARectangle.UNIT_AARECTANGLE.Width, Is.EqualTo(1f));
            Assert.That(AARectangle.UNIT_AARECTANGLE.Height, Is.EqualTo(1f));
            Assert.That(AARectangle.UNIT_AARECTANGLE.Area, Is.EqualTo(1f));
        }

        [Test]
        [Category("AARectangle")]
        public void AARectangle_ToString_Pass()
        {
            Assert.That(new AARectangle(1f, 2f, 3f, 4f).ToString(), Is.EqualTo("AARectangle(Left: 1, Top: 2, Width: 3, Height: 4)"));
        }

    }
}
