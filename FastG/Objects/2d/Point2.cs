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
    public readonly struct Point2 : IEquatable<Point2>
    {
        /// <summary>
        /// The origin point (0, 0).
        /// </summary>
        public static readonly Point2 ZERO = new(0, 0);

        /// <summary>
        /// The point (1, 1).
        /// </summary>
        public static readonly Point2 ONE = new(1, 1);

        public float X { get; init; }

        public float Y { get; init; }

        /// <summary>
        /// Creates the origin point (0, 0).
        /// </summary>
        public Point2() : this(0f, 0f) { }

        /// <summary>
        /// Creates a point from double coordinates, narrowed to <see cref="float"/>.
        /// </summary>
        public Point2(double x, double y) : this((float)x, (float)y) { }

        /// <summary>
        /// Creates a point from integer coordinates.
        /// </summary>
        public Point2(int x, int y) : this((float)x, (float)y) { }

        /// <summary>
        /// Creates a point from short coordinates.
        /// </summary>
        public Point2(short x, short y) : this((float)x, (float)y) { }

        /// <summary>
        /// Creates a point from explicit X and Y coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns the Euclidean distance from this point to <paramref name="p"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float DistanceTo(Point2 p)
        {
            return DistanceTo(this, p);
        }

        /// <summary>
        /// Returns the vector2 required to diplace to a specified point2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 DisplacementTo(Point2 to)
        {
            return new Vector2(to.X - X, to.Y - Y);
        }

        /// <summary>
        /// Returns the squared Euclidean distance from this point to <paramref name="p"/>.
        /// Cheaper than <see cref="DistanceTo(Point2)"/> (no square root); prefer it when comparing distances.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float DistanceSquaredTo(Point2 p)
        {
            return DistanceSquaredTo(this, p);
        }

        /// <summary>
        /// Returns the Euclidean distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceTo(Point2 p1, Point2 p2)
        {
            return MathF.Sqrt(DistanceSquaredTo(p1, p2));
        }

        /// <summary>
        /// Returns the squared Euclidean distance between two points. Cheaper than
        /// <see cref="DistanceTo(Point2, Point2)"/> (no square root); prefer it when comparing distances.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquaredTo(Point2 p1, Point2 p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;

            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Translates a point by a vector, returning the displaced point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator +(Point2 p, Vector2 v)
        {
            return new Point2(p.X + v.X, p.Y + v.Y);
        }

        /// <summary>
        /// Translates a point by the negation of a vector, returning the displaced point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator -(Point2 p, Vector2 v)
        {
            return new Point2(p.X - v.X, p.Y - v.Y);
        }

        /// <summary>
        /// Returns this point rotated <paramref name="radians"/> about <paramref name="pivot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 RotateAround(Point2 pivot, float radians)
        {
            return RotateAround(this, pivot, radians);
        }

        /// <summary>
        /// Returns <paramref name="point"/> rotated <paramref name="radians"/> about <paramref name="pivot"/>,
        /// counter-clockwise from the +X axis - the same convention as <see cref="Vector2(float)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 RotateAround(Point2 point, Point2 pivot, float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            float dx = point.X - pivot.X;
            float dy = point.Y - pivot.Y;

            return new Point2(
                pivot.X + (dx * cos) - (dy * sin),
                pivot.Y + (dx * sin) + (dy * cos));
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Point2"/> at the same coordinates.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Point2 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other point has exactly equal X and Y coordinates (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Point2 p)
        {
            return X == p.X && Y == p.Y;
        }

        /// <summary>
        /// Returns true if both points share the same coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point2 a, Point2 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the points differ in either coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point2 a, Point2 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the X and Y coordinates.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Returns a string of the form "(x, y)".
        /// </summary>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
