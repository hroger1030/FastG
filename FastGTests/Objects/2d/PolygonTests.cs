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

            var centroid = polygon.Centroid;
            var scaled = polygon * 2f;

            // scaling is about the centroid, so the centroid itself does not move
            Assert.That(scaled.Centroid.X, Is.EqualTo(centroid.X).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(scaled.Centroid.Y, Is.EqualTo(centroid.Y).Within(Constants.FLOAT_ERROR_MARGIN));

            // each vertex should now be twice as far from the centroid
            for (int i = 0; i < polygon.Vertices.Length; i++)
            {
                float expectedX = centroid.X + (polygon.Vertices[i].X - centroid.X) * 2f;
                float expectedY = centroid.Y + (polygon.Vertices[i].Y - centroid.Y) * 2f;

                Assert.That(scaled.Vertices[i].X, Is.EqualTo(expectedX).Within(Constants.FLOAT_ERROR_MARGIN));
                Assert.That(scaled.Vertices[i].Y, Is.EqualTo(expectedY).Within(Constants.FLOAT_ERROR_MARGIN));
            }

            var half = scaled / 2f;
            for (int i = 0; i < polygon.Vertices.Length; i++)
            {
                Assert.That(half.Vertices[i].X, Is.EqualTo(polygon.Vertices[i].X).Within(Constants.FLOAT_ERROR_MARGIN));
                Assert.That(half.Vertices[i].Y, Is.EqualTo(polygon.Vertices[i].Y).Within(Constants.FLOAT_ERROR_MARGIN));
            }
        }

        [Test]
        [Category("Polygon")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void Polygon_Scale_Pass(float scale)
        {
            var polygon = new Polygon(
            [
                new Point2(1f, 1f),
                new Point2(2f, 1f),
                new Point2(2f, 2f),
            ]);

            var centroid = polygon.Centroid;
            var scaled = polygon.Scale(scale);

            Assert.That(scaled.Centroid.X, Is.EqualTo(centroid.X).Within(Constants.FLOAT_ERROR_MARGIN));
            Assert.That(scaled.Centroid.Y, Is.EqualTo(centroid.Y).Within(Constants.FLOAT_ERROR_MARGIN));

            for (int i = 0; i < polygon.Vertices.Length; i++)
            {
                float expectedX = centroid.X + (polygon.Vertices[i].X - centroid.X) * scale;
                float expectedY = centroid.Y + (polygon.Vertices[i].Y - centroid.Y) * scale;

                Assert.That(scaled.Vertices[i].X, Is.EqualTo(expectedX).Within(Constants.FLOAT_ERROR_MARGIN));
                Assert.That(scaled.Vertices[i].Y, Is.EqualTo(expectedY).Within(Constants.FLOAT_ERROR_MARGIN));
            }
        }

        [Test]
        [Category("Polygon")]
        [TestCase(0f)]
        [TestCase(-2f)]
        public void Polygon_Scale_ThrowsForNonPositiveScale_Fail(float scale)
        {
            var polygon = new Polygon(
            [
                new Point2(1f, 1f),
                new Point2(2f, 1f),
                new Point2(2f, 2f),
            ]);

            Assert.Throws<ArgumentOutOfRangeException>(() => polygon.Scale(scale));
        }

        [Test]
        [Category("Polygon")]
        [TestCase(0f)]
        [TestCase(-2f)]
        public void Polygon_ScalingOperatorNonPositiveScale_Fail(float scale)
        {
            var polygon = new Polygon(
            [
                new Point2(1f, 1f),
                new Point2(2f, 1f),
                new Point2(2f, 2f),
            ]);

            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = polygon * scale; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = polygon / scale; });
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_Centroid_Pass()
        {
            var polygon = new Polygon(
            [
                new Point2(0f, 0f),
                new Point2(6f, 0f),
                new Point2(6f, 6f),
                new Point2(0f, 6f),
            ]);

            Assert.That(polygon.Centroid, Is.EqualTo(new Point2(3f, 3f)));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_NullConstructor_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new Polygon((Point2[])null));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_FewerThanThreeVertices_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Polygon([]));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Polygon([new Point2(0f, 0f)]));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Polygon([new Point2(0f, 0f), new Point2(1f, 0f)]));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_DefaultAndParameterlessConstructor_ThrowOnUse_Fail()
        {
            // Polygon has no explicit parameterless constructor - a struct always has an implicit one that C#
            // cannot suppress, so `new Polygon()` (and `default(Polygon)`) both bypass the array constructor's
            // guard and produce a Polygon with a null backing array. Rather than covering for that with a
            // graceful empty-polygon fallback, Vertices reuses the same guards as the constructor and throws
            // the moment anyone touches it, so the mistake surfaces immediately instead of silently behaving
            // like a valid empty polygon.
            Assert.Throws<ArgumentNullException>(() => { var _ = new Polygon().Sides; });
            Assert.Throws<ArgumentNullException>(() => { var _ = default(Polygon).Sides; });
            Assert.Throws<ArgumentNullException>(() => { var _ = default(Polygon).Vertices; });
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_PentagonAndHexagon_Pass()
        {
            Assert.That(Polygon.PENTAGON.Vertices.Length, Is.EqualTo(5));
            Assert.That(Polygon.HEXAGON.Vertices.Length, Is.EqualTo(6));

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
            var differentVertexCount = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(1f, 1f), new Point2(0f, 1f)]);

            Assert.That(triangle.Equals(same), Is.True);
            Assert.That(triangle.Equals(reordered), Is.False);
            Assert.That(triangle.Equals(differentVertexCount), Is.False);
            Assert.That(triangle.Equals(null), Is.False);
            Assert.That(triangle.Equals((object)same), Is.True);
            Assert.That(triangle.Equals((object)null), Is.False);
            Assert.That(triangle.Equals("not a polygon"), Is.False);

            Assert.That(triangle.GetHashCode(), Is.EqualTo(same.GetHashCode()));

            Assert.That(triangle == same, Is.True);
            Assert.That(triangle != reordered, Is.True);
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_ConstructorCopiesInputArray_Pass()
        {
            // Polygon is an immutable value type: the constructor must copy its input, so mutating the
            // caller's array afterward cannot leak through and change an already-constructed Polygon.
            var source = new Point2[] { new(0f, 0f), new(1f, 0f), new(0f, 1f) };
            var polygon = new Polygon(source);

            source[0] = new Point2(99f, 99f);

            Assert.That(polygon.Vertices[0], Is.EqualTo(new Point2(0f, 0f)));
        }

        [Test]
        [Category("Polygon")]
        public void Polygon_StructAssignment_CopiesTheValue_Pass()
        {
            var original = new Polygon([new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f)]);
            var copy = original;

            Assert.That(copy, Is.EqualTo(original));
            Assert.That(copy.Vertices.Length, Is.EqualTo(3));
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



