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
    public readonly struct Capsule : I3d, IEquatable<Capsule>
    {
        public Point3 PointA { get; init; }

        public Point3 PointB { get; init; }

        public float Radius { get; init; }

        /// <summary>
        /// The volume enclosed by the capsule: a cylinder spanning the distance between the two endpoints,
        /// capped by a full sphere - the two hemispherical ends, of the same radius, joined together.
        /// </summary>
        [JsonIgnore]
        public float Volume
        {
            get
            {
                float height = new Vector3(PointA, PointB).Length;
                return (Constants.PI * Radius * Radius * height) + ((4f / 3f) * Constants.PI * Radius * Radius * Radius);
            }
        }

        /// <summary>
        /// The total surface area of the capsule: the cylindrical body's lateral surface, plus the full sphere
        /// surface formed by the two hemispherical ends joined together.
        /// </summary>
        [JsonIgnore]
        public float SurfaceArea
        {
            get
            {
                float height = new Vector3(PointA, PointB).Length;
                return (2f * Constants.PI * Radius * height) + (4f * Constants.PI * Radius * Radius);
            }
        }

        /// <summary>
        /// Creates a capsule: the set of points within <paramref name="radius"/> of the segment from <paramref name="pointA"/> to <paramref name="pointB"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is negative, or
        /// <see cref="ArgumentException"/> if <paramref name="pointA"/> and <paramref name="pointB"/> are the
        /// same point - that shape is a <see cref="Sphere"/>, so construct one of those directly instead.
        /// </summary>
        public Capsule(Point3 pointA, Point3 pointB, float radius)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(radius);

            if (pointA.Equals(pointB))
                throw new ArgumentException("a capsule's two endpoints cannot be the same point.", nameof(pointB));

            PointA = pointA;
            PointB = pointB;
            Radius = radius;
        }

        /// <summary>
        /// Creates a capsule from the raw coordinates of its two segment endpoints and a radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is negative, or
        /// <see cref="ArgumentException"/> if the two endpoints are the same point.
        /// </summary>
        public Capsule(float pointAX, float pointAY, float pointAZ, float pointBX, float pointBY, float pointBZ, float radius)
            : this(new Point3(pointAX, pointAY, pointAZ), new Point3(pointBX, pointBY, pointBZ), radius) { }

        /// <summary>
        /// Returns a copy of this capsule scaled uniformly about the midpoint of <see cref="PointA"/> and
        /// <see cref="PointB"/> - both the radius and the distance between the endpoints are multiplied by
        /// <paramref name="scale"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/>
        /// is zero or negative.
        /// </summary>
        public Capsule Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            float centerX = (PointA.X + PointB.X) / 2f;
            float centerY = (PointA.Y + PointB.Y) / 2f;
            float centerZ = (PointA.Z + PointB.Z) / 2f;

            return new Capsule(
                new Point3(centerX + (PointA.X - centerX) * scale, centerY + (PointA.Y - centerY) * scale, centerZ + (PointA.Z - centerZ) * scale),
                new Point3(centerX + (PointB.X - centerX) * scale, centerY + (PointB.Y - centerY) * scale, centerZ + (PointB.Z - centerZ) * scale),
                Radius * scale);
        }

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

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on this capsule.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);

        /// <summary>
        /// Returns true if this capsule overlaps or touches <paramref name="sphere"/>, tested via the closest point
        /// on the capsule's core segment to the sphere center.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere) => Collisions3d.Intersects(this, sphere);

        /// <summary>
        /// Returns true if this capsule overlaps or touches <paramref name="aabb"/>. See
        /// <see cref="Collisions3d.Intersects(AABB, Capsule)"/> for the sphere-and-box decomposition this uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB aabb) => Collisions3d.Intersects(aabb, this);

        /// <summary>
        /// Returns true if this capsule overlaps or touches <paramref name="cube"/>. See
        /// <see cref="Collisions3d.Intersects(AABB, Capsule)"/> for the sphere-and-box decomposition this uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cube cube) => Collisions3d.Intersects(this, cube);

        /// <summary>
        /// Returns true if this capsule overlaps or touches <paramref name="other"/> (the shortest distance between
        /// their core segments is no greater than the sum of their radii).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Capsule other) => Collisions3d.Intersects(this, other);
    }
}
