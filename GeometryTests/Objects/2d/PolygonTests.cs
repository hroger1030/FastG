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
using System.Collections.Generic;

namespace GeometryTests
{
    [TestFixture]
    public class PolygonTests
    {
        [Test]
        [Category("Polygon")]
        public void Polygon_SidesAreaAndPerimeter_Pass()
        {
            var polygon = new Polygon(
            [
                new Point2(0f, 0f),
                new Point2(2f, 0f),
                new Point2(2f, 2f),
                new Point2(0f, 2f),
            ]);

            Assert.That(polygon.Sides, Is.EqualTo(4));
            Assert.That(polygon.Area, Is.EqualTo(4f));
            Assert.That(polygon.Perimeter, Is.EqualTo(8f));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_OperatorTranslation_Pass()
        {
            var polygon = new Polygon(
            [
                new Point2(0f, 0f),
                new Point2(1f, 0f),
                new Point2(1f, 1f),
            ]);

            var translated = polygon + new Vector2(1f, 1f);
            Assert.That(translated.Contains(new Point2(1.1f, 1.1f)), Is.True);

            var shiftedBack = translated - new Vector2(1f, 1f);
            Assert.That(shiftedBack.Equals(polygon), Is.True);
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_ScalingOperators_Pass()
        {
            var polygon = new Polygon(
            [
                new Point2(1f, 1f),
                new Point2(2f, 1f),
                new Point2(2f, 2f),
            ]);

            var scaled = polygon * 2f;
            Assert.That(scaled.Vertices[0].X, Is.EqualTo(2f));
            Assert.That(scaled.Vertices[0].Y, Is.EqualTo(2f));
            Assert.That(scaled.Vertices[2].X, Is.EqualTo(4f));

            var half = scaled / 2f;
            Assert.That(half.Equals(polygon), Is.True);
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_ScalingOperatorDivideByZero_Fail()
        {
            var polygon = new Polygon(
            [
                new Point2(1f, 1f),
                new Point2(2f, 1f),
                new Point2(2f, 2f),
            ]);

            Assert.Throws<DivideByZeroException>(() => { var _ = polygon / 0f; });
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_NullConstructor_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new Polygon((List<Point2>)null));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_PentagonAndHexagon_Pass()
        {
            Assert.That(Polygon.PENTAGON.Vertices.Count, Is.EqualTo(5));
            Assert.That(Polygon.HEXAGON.Vertices.Count, Is.EqualTo(6));

            Assert.That(Polygon.PENTAGON.Area, Is.GreaterThan(0f));
            Assert.That(Polygon.PENTAGON.Perimeter, Is.GreaterThan(0f));
            Assert.That(Polygon.HEXAGON.Area, Is.GreaterThan(0f));
            Assert.That(Polygon.HEXAGON.Perimeter, Is.GreaterThan(0f));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_EqualsHashCodeAndOperators_Pass()
        {
            var triangle = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f)]);
            var same = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f)]);
            var reordered = new Polygon([new Point2(1f, 0f), new Point2(0f, 0f), new Point2(0f, 1f)]);
            var shorter = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f)]);

            Assert.That(triangle.Equals(same), Is.True);
            Assert.That(triangle.Equals(reordered), Is.False);
            Assert.That(triangle.Equals(shorter), Is.False);
            Assert.That(triangle.Equals(null), Is.False);
            Assert.That(triangle.Equals((object)same), Is.True);
            Assert.That(triangle.Equals((object)null), Is.False);
            Assert.That(triangle.Equals("not a polygon"), Is.False);

            Assert.That(triangle.GetHashCode(), Is.EqualTo(same.GetHashCode()));

            Assert.That(triangle == same, Is.True);
            Assert.That(triangle != reordered, Is.True);
            Assert.That((Polygon)null == (Polygon)null, Is.True);
            Assert.That(triangle == null, Is.False);
            Assert.That(null == triangle, Is.False);
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_CopyConstructor_Pass()
        {
            var original = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f)]);
            var copy = new Polygon(original);

            Assert.That(copy, Is.EqualTo(original));
            Assert.That(copy.Vertices, Is.Not.SameAs(original.Vertices));

            // mutating the copy must not affect the original
            copy.Vertices.Add(new Point2(2f, 2f));
            Assert.That(original.Vertices.Count, Is.EqualTo(3));

            Assert.Throws<ArgumentNullException>(() => new Polygon((Polygon)null));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_ToString_Pass()
        {
            var polygon = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f)]);

            Assert.That(polygon.ToString(), Is.EqualTo("Polygon[(0, 0), (1, 0), (0, 1)]"));
        }
    }
}



