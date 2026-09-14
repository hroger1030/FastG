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

using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace FastG
{
    public readonly struct Line2 : I1d, IEquatable<Line2>
    {
        /// <summary>
        /// A line segment from the origin (0, 0) to the point (1, 1).
        /// </summary>
        public static readonly Line2 UNIT_LINE = new(Point2.ZERO, Point2.ONE);

        public Point2 Point1 { get; init; }

        public Point2 Point2 { get; init; }

        /// <summary>
        /// The distance between the two endpoints (length of the segment).
        /// </summary>
        [JsonIgnore]
        public float Length => Point1.DistanceTo(Point2);

        /// <summary>
        /// Creates a line segment from the raw coordinates of its two endpoints.
        /// </summary>
        public Line2(float p1x, float p1y, float p2x, float p2y) : this(new Point2(p1x, p1y), new Point2(p2x, p2y)) { }

        /// <summary>
        /// Creates a line segment between two endpoints.
        /// </summary>
        public Line2(Point2 p1, Point2 p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        /// <summary>
        /// Returns a copy of this segment scaled uniformly about its midpoint by <paramref name="scale"/> - a
        /// scale of 2 doubles the segment's length while keeping its midpoint fixed. Throws
        /// <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Line2 Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            float midX = (Point1.X + Point2.X) / 2f;
            float midY = (Point1.Y + Point2.Y) / 2f;

            return new Line2(
                new Point2(midX + (Point1.X - midX) * scale, midY + (Point1.Y - midY) * scale),
                new Point2(midX + (Point2.X - midX) * scale, midY + (Point2.Y - midY) * scale));
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Line2"/> with the same endpoints in the same order.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Line2 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other line has the same endpoints in the same order (direction-sensitive).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Line2 l)
        {
            return Point1.Equals(l.Point1) && Point2.Equals(l.Point2);
        }

        /// <summary>
        /// Returns true if both lines have the same endpoints in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Line2 a, Line2 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the lines differ in either endpoint or ordering.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Line2 a, Line2 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the two endpoints.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Point1, Point2);
        }

        /// <summary>
        /// Returns a string of the form "Line2(Point1: (x, y), Point2: (x, y))".
        /// </summary>
        public override string ToString()
        {
            return $"Line2(Point1: {Point1}, Point2: {Point2})";
        }

        /// <summary>
        /// Returns true if this segment intersects or touches <paramref name="other"/>, including collinear overlap.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Line2 other) => Collisions2d.Intersects(this, other);

        /// <summary>
        /// Returns true if <paramref name="point"/> lies on this segment (within <see cref="Constants.FLOAT_ERROR_MARGIN"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);
    }
}
