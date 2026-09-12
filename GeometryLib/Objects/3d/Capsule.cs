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
    public readonly struct Capsule : IEquatable<Capsule>
    {
        public Point3 PointA { get; init; }

        public Point3 PointB { get; init; }

        public float Radius { get; init; }

        /// <summary>
        /// Creates a capsule: the set of points within <paramref name="radius"/> of the segment from <paramref name="pointA"/> to <paramref name="pointB"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is negative.
        /// </summary>
        public Capsule(Point3 pointA, Point3 pointB, float radius)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(radius);

            PointA = pointA;
            PointB = pointB;
            Radius = radius;
        }

        /// <summary>
        /// Creates a capsule from the raw coordinates of its two segment endpoints and a radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is negative.
        /// </summary>
        public Capsule(float pointAX, float pointAY, float pointAZ, float pointBX, float pointBY, float pointBZ, float radius)
            : this(new Point3(pointAX, pointAY, pointAZ), new Point3(pointBX, pointBY, pointBZ), radius) { }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on this capsule. A degenerate capsule
        /// (PointA == PointB) is treated as a sphere.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);

        /// <summary>
        /// Returns true if this capsule overlaps or touches <paramref name="sphere"/>, tested via the closest point
        /// on the capsule's core segment to the sphere center. A degenerate capsule is treated as a sphere.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere) => Collisions3d.Intersects(this, sphere);

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Capsule"/> with the same endpoints and radius.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Capsule other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other capsule has the same endpoints (in the same order) and radius (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Capsule other)
        {
            return PointA.Equals(other.PointA)
                && PointB.Equals(other.PointB)
                && Radius == other.Radius;
        }

        /// <summary>
        /// Returns true if both capsules have the same endpoints and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Capsule a, Capsule b) => a.Equals(b);

        /// <summary>
        /// Returns true if the capsules differ in either endpoint or radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Capsule a, Capsule b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the two endpoints and the radius.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(PointA, PointB, Radius);
        }

        /// <summary>
        /// Returns a string of the form "Capsule(PointA: (x, y, z), PointB: (x, y, z), Radius: r)".
        /// </summary>
        public override string ToString()
        {
            return $"Capsule(PointA: {PointA}, PointB: {PointB}, Radius: {Radius})";
        }
    }
}
