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

using System.Runtime.CompilerServices;

namespace FastG
{
    /// <summary>
    /// Pairwise intersection and containment tests between 2D shapes. Each shape pair has exactly one
    /// real implementation here; the matching instance methods on the shape types themselves
    /// (e.g. <see cref="Circle.Intersects(AARectangle)"/>) are thin forwarders kept for call-site convenience.
    /// </summary>
    public static class Collisions2d
    {
        /// <summary>
        /// Checks to see if two circles are intersecting. Tangent circles count as intersecting.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Circle a, Circle b)
        {
            float distanceX = b.Center.X - a.Center.X;
            float distanceY = b.Center.Y - a.Center.Y;
            float sumRadius = a.Radius + b.Radius;

            return (sumRadius * sumRadius) >= (distanceX * distanceX + distanceY * distanceY);
        }

        /// <summary>
        /// Returns true if <paramref name="circle"/> overlaps or touches <paramref name="rect"/>, using the closest-point-on-rectangle test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Circle circle, AARectangle rect)
        {
            float closestX = Math.Clamp(circle.Center.X, rect.Left, rect.Right);
            float closestY = Math.Clamp(circle.Center.Y, rect.Top, rect.Bottom);

            float distanceX = circle.Center.X - closestX;
            float distanceY = circle.Center.Y - closestY;

            return (distanceX * distanceX + distanceY * distanceY) <= (circle.Radius * circle.Radius);
        }

        /// <summary>
        /// Gets whether or not two rectangles intersect or are tangent.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(AARectangle a, AARectangle b)
        {
            // Check if the rectangles are intersecting or tangent
            return a.Right >= b.Left &&  // a's right side is to the right of or touching b's left side
                   a.Left <= b.Right &&  // a's left side is to the left of or touching b's right side
                   a.Bottom >= b.Top &&  // a's bottom side is below or touching b's top side
                   a.Top <= b.Bottom;    // a's top side is above or touching b's bottom side
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="circle"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Circle circle, Point2 point)
        {
            float distanceX = point.X - circle.Center.X;
            float distanceY = point.Y - circle.Center.Y;

            // distanceX^2 + distanceY^2 is already non-negative, so no MathF.Abs is needed.
            return (circle.Radius * circle.Radius) >= (distanceX * distanceX + distanceY * distanceY);
        }

        /// <summary>
        /// Returns true if all four corners of <paramref name="rect"/> lie inside or on <paramref name="circle"/>
        /// (i.e. the rectangle is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Circle circle, AARectangle rect)
        {
            if (!Contains(circle, rect.TopLeftCorner)) return false;
            if (!Contains(circle, rect.TopRightCorner)) return false;
            if (!Contains(circle, rect.BottomRightCorner)) return false;
            if (!Contains(circle, rect.BottomLeftCorner)) return false;

            return true;
        }

        /// <summary>
        /// Returns true if all three vertices of <paramref name="triangle"/> lie inside or on <paramref name="circle"/>
        /// (i.e. the triangle is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Circle circle, Triangle2 triangle)
        {
            if (!Contains(circle, triangle.A)) return false;
            if (!Contains(circle, triangle.B)) return false;
            if (!Contains(circle, triangle.C)) return false;

            return true;
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of <paramref name="rect"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(AARectangle rect, Point2 point)
        {
            return point.X >= rect.Left && point.X <= rect.Right && point.Y >= rect.Top && point.Y <= rect.Bottom;
        }

        /// <summary>
        /// Returns true if <paramref name="value"/> lies entirely inside or on the edges of <paramref name="rect"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(AARectangle rect, AARectangle value)
        {
            return value.Left >= rect.Left && value.Right <= rect.Right && value.Top >= rect.Top && value.Bottom <= rect.Bottom;
        }

        /// <summary>
        /// Returns true if two line segments intersect or touch, including collinear overlap.
        /// </summary>
        public static bool Intersects(Line2 a, Line2 b)
        {
            Point2 p1 = a.Point1, q1 = a.Point2;
            Point2 p2 = b.Point1, q2 = b.Point2;

            float o1 = Orientation(p1, q1, p2);
            float o2 = Orientation(p1, q1, q2);
            float o3 = Orientation(p2, q2, p1);
            float o4 = Orientation(p2, q2, q1);

            // general case: each segment's endpoints straddle the other segment's line
            if (DiffersInSign(o1, o2) && DiffersInSign(o3, o4))
                return true;

            // special cases: an endpoint is collinear with, and lies on, the other segment
            if (IsZero(o1) && OnSegment(p1, p2, q1)) return true;
            if (IsZero(o2) && OnSegment(p1, q2, q1)) return true;
            if (IsZero(o3) && OnSegment(p2, p1, q2)) return true;
            if (IsZero(o4) && OnSegment(p2, q1, q2)) return true;

            return false;

            static float Orientation(Point2 p, Point2 q, Point2 r) => (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            static bool IsZero(float v) => MathF.Abs(v) < Constants.FLOAT_ERROR_MARGIN;
            static bool DiffersInSign(float x, float y) => !IsZero(x) && !IsZero(y) && (x > 0f) != (y > 0f);

            // assumes p, q, r are collinear; checks whether q lies within the bounding box of p and r
            static bool OnSegment(Point2 p, Point2 q, Point2 r) =>
                q.X <= MathF.Max(p.X, r.X) && q.X >= MathF.Min(p.X, r.X) &&
                q.Y <= MathF.Max(p.Y, r.Y) && q.Y >= MathF.Min(p.Y, r.Y);
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies on <paramref name="line"/> (within <see cref="Constants.FLOAT_ERROR_MARGIN"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Line2 line, Point2 point)
        {
            var direction = line.Point1.DisplacementTo(line.Point2);
            float lengthSquared = direction.LengthSquared;

            if (lengthSquared == 0f)
                return line.Point1.Equals(point);

            var toPoint = line.Point1.DisplacementTo(point);
            float t = Math.Clamp(Vector2.Dot(direction, toPoint) / lengthSquared, 0f, 1f);
            var closest = line.Point1 + (direction * t);

            return closest.DistanceSquaredTo(point) <= Constants.FLOAT_ERROR_MARGIN;
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of <paramref name="triangle"/>,
        /// using the standard same-side sign test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Triangle2 triangle, Point2 point)
        {
            float d1 = Sign(point, triangle.A, triangle.B);
            float d2 = Sign(point, triangle.B, triangle.C);
            float d3 = Sign(point, triangle.C, triangle.A);

            bool hasNegative = d1 < 0f || d2 < 0f || d3 < 0f;
            bool hasPositive = d1 > 0f || d2 > 0f || d3 > 0f;

            return !(hasNegative && hasPositive);

            static float Sign(Point2 p1, Point2 p2, Point2 p3) => (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        /// <summary>
        /// Returns true if two triangles overlap or touch: any pair of edges crosses, or one triangle
        /// fully encloses the other.
        /// </summary>
        public static bool Intersects(Triangle2 a, Triangle2 b)
        {
            Span<Point2> verticesA = [a.A, a.B, a.C];
            Span<Point2> verticesB = [b.A, b.B, b.C];

            for (int i = 0; i < 3; i++)
            {
                var edgeA = new Line2(verticesA[i], verticesA[(i + 1) % 3]);

                for (int j = 0; j < 3; j++)
                {
                    var edgeB = new Line2(verticesB[j], verticesB[(j + 1) % 3]);

                    if (Intersects(edgeA, edgeB))
                        return true;
                }
            }

            // no edges cross, so the triangles either don't overlap or one fully contains the other
            return Contains(a, b.A) || Contains(b, a.A);
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="ellipse"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Ellipse ellipse, Point2 point)
        {
            float dx = point.X - ellipse.Center.X;
            float dy = point.Y - ellipse.Center.Y;

            // these two terms could be precomputed for perf if needed.
            float invRadiusXSquared = 1 / (ellipse.RadiusX * ellipse.RadiusX);
            float invRadiusYSquared = 1 / (ellipse.RadiusY * ellipse.RadiusY);

            return (dx * dx * invRadiusXSquared) + (dy * dy * invRadiusYSquared) <= 1f;
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside <paramref name="polygon"/>, using a ray-casting (even-odd) test.
        /// Behavior on the boundary is not guaranteed. Throws via <see cref="Polygon.Vertices"/> if
        /// <paramref name="polygon"/> was never properly constructed (e.g. <c>default(Polygon)</c>).
        /// </summary>
        public static bool Contains(Polygon polygon, Point2 point)
        {
            bool isInside = false;
            var vertices = polygon.Vertices;

            for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++)
            {
                var vi = vertices[i];
                var vj = vertices[j];

                if ((vi.Y > point.Y) != (vj.Y > point.Y) && point.X < (vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X)
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }
    }
}
