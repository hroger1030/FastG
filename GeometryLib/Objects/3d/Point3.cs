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
    public readonly struct Point3 : IEquatable<Point3>
    {
        /// <summary>
        /// The origin point (0, 0, 0).
        /// </summary>
        public static readonly Point3 ZERO = new(0, 0, 0);

        /// <summary>
        /// The point (1, 1, 1).
        /// </summary>
        public static readonly Point3 ONE = new(1, 1, 1);

        public float X { get; init; }

        public float Y { get; init; }

        public float Z { get; init; }

        /// <summary>
        /// Creates the origin point (0, 0, 0).
        /// </summary>
        public Point3() : this(0f, 0f, 0f) { }

        /// <summary>
        /// Creates a point from explicit X, Y and Z coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a point from a 2D point, placing it on the Z = 0 plane.
        /// </summary>
        public Point3(Point2 p)
        {
            X = p.X;
            Y = p.Y;
            Z = 0;
        }

        /// <summary>
        /// Translates a point by a vector, returning the displaced point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator +(Point3 p, Vector3 v)
        {
            return new Point3(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        }

        /// <summary>
        /// Translates a point by the negation of a vector, returning the displaced point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator -(Point3 p, Vector3 v)
        {
            return new Point3(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Point3"/> at the same coordinates.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Point3 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other point has exactly equal X, Y and Z coordinates (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Point3 p)
        {
            return X == p.X && Y == p.Y && Z == p.Z;
        }

        /// <summary>
        /// Returns true if both points share the same coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point3 a, Point3 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the points differ in any coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point3 a, Point3 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the X, Y and Z coordinates.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        /// Returns a string of the form "(x, y, z)".
        /// </summary>
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
