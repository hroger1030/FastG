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

namespace Geometry
{
    /// <summary>
    /// Pairwise intersection and containment tests between 2D shapes. Each shape pair has exactly one
    /// real implementation here; the matching instance methods on the shape types themselves
    /// (e.g. <see cref="Circle.Intersects(Rectangle)"/>) are thin forwarders kept for call-site convenience.
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
        public static bool Intersects(Circle circle, Rectangle rect)
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
        public static bool Intersects(Rectangle a, Rectangle b)
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
        public static bool Contains(Circle circle, Rectangle rect)
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
        public static bool Contains(Rectangle rect, Point2 point)
        {
            return point.X >= rect.Left && point.X <= rect.Right && point.Y >= rect.Top && point.Y <= rect.Bottom;
        }

        /// <summary>
        /// Returns true if <paramref name="value"/> lies entirely inside or on the edges of <paramref name="rect"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Rectangle rect, Rectangle value)
        {
            return value.Left >= rect.Left && value.Right <= rect.Right && value.Top >= rect.Top && value.Bottom <= rect.Bottom;
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
        /// Always returns false for polygons with fewer than three vertices; behavior on the boundary is not guaranteed.
        /// </summary>
        public static bool Contains(Polygon polygon, Point2 point)
        {
            if (polygon.Sides < 3)
                return false;

            bool isInside = false;
            var vertices = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(polygon.Vertices);

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
