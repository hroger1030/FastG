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
using System.Collections.Generic;

namespace FastGTests
{
    /// <summary>
    /// Tests for the pairwise intersection/containment tests in <see cref="Collisions2d"/>, exercised through
    /// each shape's forwarding instance methods (e.g. <see cref="Circle.Intersects(AARectangle)"/>).
    /// Moved here from the individual shape test fixtures when the collision logic itself moved to <see cref="Collisions2d"/>.
    /// </summary>
    [TestFixture]
    public class Collisions2dTests
    {
        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        [TestCase(0f, 0f, 2f, 0f, 0f, 2f)] // overlapping
        [TestCase(0f, 0f, 2f, 1f, 1f, 2f)] // offset
        [TestCase(0f, 0f, 1f, 0f, 0f, 2f)] // contained
        [TestCase(0f, 0f, 2f, 0f, 0f, 1f)] // containing
        [TestCase(0f, 0f, 1f, 0f, 2f, 1f)] // tangent
        public void Circle_IntersectsCircle_Pass(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            var c1 = new Circle(x1, y1, r1);
            var c2 = new Circle(x2, y2, r2);

            Assert.That(c1.Intersects(c2), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        [TestCase(0f, 0f, 2f, 9f, 9f, 2f)] // remote
        [TestCase(0f, 0f, 1f, 2.01f, 0f, 1f)] // close
        public void Circle_IntersectsAdjacentCircle_Fail(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            var c1 = new Circle(x1, y1, r1);
            var c2 = new Circle(x2, y2, r2);

            Assert.That(c1.Intersects(c2), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        [TestCase(0f, 0f, true)] // center
        [TestCase(0.5f, 0.5f, true)] // inside
        [TestCase(1f, 0f, true)] // tangent
        [TestCase(-1f, 0f, true)] // tangent
        [TestCase(1f, 1f, false)] // outside corner
        [TestCase(100f, 100f, false)] // far distant
        public void Circle_ContainsPoints_Pass(float x, float y, bool expectedResult)
        {
            var c1 = new Circle(0, 0, 1);
            var p1 = new Point2(x, y);

            Assert.That(c1.Contains(p1) == expectedResult, Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_IntersectsAARectangle_Pass()
        {
            var c = new Circle(2f, 2f, 1f);
            var r = new AARectangle(2f, 1f, 2f, 2f);

            Assert.That(c.Intersects(r), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_IntersectsAARectangle_Fail()
        {
            var c = new Circle(0f, 0f, 1f);
            var r = new AARectangle(2.1f, 2.1f, 1f, 1f);

            Assert.That(c.Intersects(r), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_ContainsAARectangle_Pass()
        {
            var c = new Circle(0f, 0f, 5f);
            var r = new AARectangle(-1f, -1f, 2f, 2f);

            Assert.That(c.Contains(r), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_ContainsAARectangle_Fail()
        {
            var c = new Circle(0f, 0f, 1f);
            var r = new AARectangle(0f, 0f, 2f, 2f);

            Assert.That(c.Contains(r), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_ContainsTriangle_Pass()
        {
            var c = new Circle(0f, 0f, 5f);
            var t = new Triangle2(new Point2(-1f, -1f), new Point2(1f, -1f), new Point2(0f, 2f));

            Assert.That(c.Contains(t), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Circle")]
        public void Circle_ContainsTriangle_ThirdVertexOutside_Fail()
        {
            var c = new Circle(0f, 0f, 2f);
            var t = new Triangle2(new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 10f));

            Assert.That(c.Contains(t), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        public void TestContainsGeometry()
        {
            var r = new AARectangle(0, 0, 10, 10);

            // point inside
            Assert.That(r.Contains(new Point2(3, 3)), Is.True, "Failed contained check 1");

            // point outside
            Assert.That(r.Contains(new Point2(12, 12)), Is.False, "Failed contained check 2");

            // points on border
            Assert.That(r.Contains(new Point2(0, 0)), Is.True, "Failed contained check 3");
            Assert.That(r.Contains(new Point2(10, 10)), Is.True, "Failed contained check 4");

            // rectangle inside
            Assert.That(r.Contains(new AARectangle(2, 2, 2, 2)), Is.True, "Failed contained check 5");

            // rectangle outside
            Assert.That(r.Contains(new AARectangle(-5, -5, 2, 2)), Is.False, "Failed contained check 6");

            // rectangle outside & surrounds
            Assert.That(r.Contains(new AARectangle(-50, -50, 200, 200)), Is.False, "Failed contained check 7");

            // rectangle inside & tangent
            Assert.That(r.Contains(new AARectangle(0, 0, 2, 2)), Is.True, "Failed contained check 8");

            // rectangle outside & tangent
            Assert.That(r.Contains(new AARectangle(-2, -2, 2, 2)), Is.False, "Failed contained check 9");

            // rectangle intersects
            Assert.That(r.Contains(new AARectangle(-2, -2, 10, 10)), Is.False, "Failed contained check 10");
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        [TestCase(-2, -2, 10, 10)] // overlapping & tangent
        [TestCase(2, 2, 2, 2)] // containing
        [TestCase(-50, -50, 200, 200)] // contains
        [TestCase(0, 0, 2, 2)] // tangent
        [TestCase(-2, -2, 2, 2)] // overlapping
        public void AARectangle_IntersectsGeometry_Pass(float top, float left, float width, float height)
        {
            var r1 = new AARectangle(0, 0, 10, 10);
            var r2 = new AARectangle(left, top, width, height);

            Assert.That(r1.Intersects(r2), Is.True, "Failed intersect check");
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        [TestCase(0f, 0f, 6f)] // overlapping
        [TestCase(0f, 0f, 2f)] // offset
        [TestCase(0f, 0f, 1f)] // tangent
        [TestCase(5f, 5f, 1f)] // contained
        [TestCase(5f, 5f, 25f)] // containing
        public void AARectangle_IntersectsCircle_Pass(float x, float y, float radius)
        {
            var r = new AARectangle(0, 0, 10, 10);
            var c = new Circle(x, y, radius);

            Assert.That(r.Intersects(c), Is.True, "Failed intersect check");
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        public void AARectangle_IntersectsAARectangle_Overlap_Pass()
        {
            // overlap
            var r1 = new AARectangle(0, 0, 1, 1);
            var r2 = new AARectangle(0, 0, 1, 1);

            Assert.That(r1.Intersects(r2), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        public void AARectangle_IntersectsAARectangle_Separate_Fail()
        {
            // doesn't intersect
            var r1 = new AARectangle(0, 0, 1, 1) + Vector2.ONE;
            var r2 = new AARectangle(0, 0, 1, 1) - Vector2.ONE;

            Assert.That(r1.Intersects(r2), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("AARectangle")]
        public void AARectangle_IntersectsCircle_Fail()
        {
            var r = new AARectangle(0, 0, 1, 1);
            var c = new Circle(3f, 3f, 0.5f);

            Assert.That(r.Intersects(c), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_Crossing_Pass()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(2f, 2f));
            var b = new Line2(new Point2(0f, 2f), new Point2(2f, 0f));

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_ParallelNonOverlapping_Fail()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(1f, 0f));
            var b = new Line2(new Point2(0f, 1f), new Point2(1f, 1f));

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_CollinearOverlapping_Pass()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(2f, 0f));
            var b = new Line2(new Point2(1f, 0f), new Point2(3f, 0f));

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_CollinearDisjoint_Fail()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(1f, 0f));
            var b = new Line2(new Point2(2f, 0f), new Point2(3f, 0f));

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_TJunction_Pass()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(2f, 0f));
            var b = new Line2(new Point2(1f, 0f), new Point2(1f, 1f));

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        public void Line2_IntersectsLine2_ClearlyDisjoint_Fail()
        {
            var a = new Line2(new Point2(0f, 0f), new Point2(1f, 0f));
            var b = new Line2(new Point2(5f, 5f), new Point2(6f, 6f));

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Line2")]
        [TestCase(2f, 0f, true)] // midpoint
        [TestCase(0f, 0f, true)] // endpoint
        [TestCase(2f, 1f, false)] // off the line
        [TestCase(5f, 0f, false)] // on the infinite line, outside the segment
        public void Line2_Contains_Pass(float x, float y, bool expected)
        {
            var line = new Line2(new Point2(0f, 0f), new Point2(4f, 0f));

            Assert.That(line.Contains(new Point2(x, y)), Is.EqualTo(expected));
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Triangle2")]
        [TestCase(1f, 1f, true)] // inside
        [TestCase(10f, 10f, false)] // outside
        [TestCase(2f, 0f, true)] // on an edge
        [TestCase(0f, 0f, true)] // on a vertex
        public void Triangle2_Contains_Pass(float x, float y, bool expected)
        {
            var triangle = new Triangle2(new Point2(0f, 0f), new Point2(4f, 0f), new Point2(0f, 4f));

            Assert.That(triangle.Contains(new Point2(x, y)), Is.EqualTo(expected));
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Triangle2")]
        public void Triangle2_IntersectsTriangle2_VertexInsideOther_Pass()
        {
            var a = new Triangle2(new Point2(0f, 0f), new Point2(4f, 0f), new Point2(0f, 4f));
            var b = new Triangle2(new Point2(1f, 1f), new Point2(5f, 1f), new Point2(1f, 5f));

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Triangle2")]
        public void Triangle2_IntersectsTriangle2_FullyContained_Pass()
        {
            var outer = new Triangle2(new Point2(0f, 0f), new Point2(10f, 0f), new Point2(0f, 10f));
            var inner = new Triangle2(new Point2(1f, 1f), new Point2(2f, 1f), new Point2(1f, 2f));

            Assert.That(outer.Intersects(inner), Is.True);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Triangle2")]
        public void Triangle2_IntersectsTriangle2_Separate_Fail()
        {
            var a = new Triangle2(new Point2(0f, 0f), new Point2(1f, 0f), new Point2(0f, 1f));
            var b = new Triangle2(new Point2(10f, 10f), new Point2(11f, 10f), new Point2(10f, 11f));

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Ellipse")]
        public void Ellipse_Contains_Pass()
        {
            var ellipse = new Ellipse(new Point2(0f, 0f), 2f, 1f);

            Assert.That(ellipse.Contains(new Point2(1f, 0f)), Is.True);
            Assert.That(ellipse.Contains(new Point2(3f, 0f)), Is.False);
        }

        [Test]
        [Category("Collisions2d")]
        [Category("Polygon")]
        public void Polygon_Contains_Pass()
        {
            var polygon = new Polygon(
            [
                new Point2(0f, 0f),
                new Point2(2f, 0f),
                new Point2(2f, 2f),
                new Point2(0f, 2f),
            ]);

            Assert.That(polygon.Contains(new Point2(1f, 1f)), Is.True);
            Assert.That(polygon.Contains(new Point2(3f, 3f)), Is.False);
        }
    }
}
