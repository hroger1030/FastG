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
    /// <summary>
    /// A right circular cylinder: a flat-capped tube of constant <see cref="Radius"/> running along the
    /// segment from <see cref="PointA"/> to <see cref="PointB"/>. Unlike <see cref="Capsule"/>, the caps are
    /// flat disks, not hemispheres, so a point exactly at <see cref="PointA"/> or <see cref="PointB"/> and
    /// within <see cref="Radius"/> of the axis is on the cylinder, but a point just beyond either endpoint
    /// along the axis is not, no matter how close it is to the axis.
    /// </summary>
    public readonly struct Cylinder : I3d, IEquatable<Cylinder>
    {
        public Point3 PointA { get; init; }

        public Point3 PointB { get; init; }

        public float Radius { get; init; }

        /// <summary>
        /// The distance between <see cref="PointA"/> and <see cref="PointB"/>.
        /// </summary>
        [JsonIgnore]
        public float Height => new Vector3(PointA, PointB).Length;

        /// <summary>
        /// The volume of the cylinder (PI * r^2 * height).
        /// </summary>
        [JsonIgnore]
        public float Volume => Constants.PI * Radius * Radius * Height;

        /// <summary>
        /// The total surface area of the cylinder, including both end caps (2 * PI * r * height + 2 * PI * r^2).
        /// </summary>
        [JsonIgnore]
        public float SurfaceArea => (2f * Constants.PI * Radius * Height) + (2f * Constants.PI * Radius * Radius);

        /// <summary>
        /// Creates a cylinder of the given <paramref name="radius"/> along the segment from <paramref name="pointA"/>
        /// to <paramref name="pointB"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/>
        /// is zero or negative, or <see cref="ArgumentException"/> if <paramref name="pointA"/> and
        /// <paramref name="pointB"/> are the same point - a cylinder cannot have zero height.
        /// </summary>
        public Cylinder(Point3 pointA, Point3 pointB, float radius)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(radius, 0f);

            if (pointA.Equals(pointB))
                throw new ArgumentException("a cylinder's two endpoints cannot be the same point - it would have zero height", nameof(pointB));

            PointA = pointA;
            PointB = pointB;
            Radius = radius;
        }

        /// <summary>
        /// Creates a cylinder from the raw coordinates of its two endpoints and a radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is zero or negative, or
        /// <see cref="ArgumentException"/> if the two endpoints are the same point.
        /// </summary>
        public Cylinder(float pointAX, float pointAY, float pointAZ, float pointBX, float pointBY, float pointBZ, float radius)
            : this(new Point3(pointAX, pointAY, pointAZ), new Point3(pointBX, pointBY, pointBZ), radius) { }

        /// <summary>
        /// Returns a copy of this cylinder scaled uniformly about the midpoint of <see cref="PointA"/> and
        /// <see cref="PointB"/> - both the radius and the distance between the endpoints are multiplied by
        /// <paramref name="scale"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/>
        /// is zero or negative.
        /// </summary>
        public Cylinder Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            float centerX = (PointA.X + PointB.X) / 2f;
            float centerY = (PointA.Y + PointB.Y) / 2f;
            float centerZ = (PointA.Z + PointB.Z) / 2f;

            return new Cylinder(
                new Point3(centerX + (PointA.X - centerX) * scale, centerY + (PointA.Y - centerY) * scale, centerZ + (PointA.Z - centerZ) * scale),
                new Point3(centerX + (PointB.X - centerX) * scale, centerY + (PointB.Y - centerY) * scale, centerZ + (PointB.Z - centerZ) * scale),
                Radius * scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Cylinder"/> with the same endpoints and radius.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Cylinder other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other cylinder has the same endpoints (in the same order) and radius (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cylinder other)
        {
            return PointA.Equals(other.PointA)
                && PointB.Equals(other.PointB)
                && Radius == other.Radius;
        }

        /// <summary>
        /// Returns true if both cylinders have the same endpoints and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cylinder a, Cylinder b) => a.Equals(b);

        /// <summary>
        /// Returns true if the cylinders differ in either endpoint or radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cylinder a, Cylinder b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the two endpoints and the radius.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(PointA, PointB, Radius);
        }

        /// <summary>
        /// Returns a string of the form "Cylinder(PointA: (x, y, z), PointB: (x, y, z), Radius: r)".
        /// </summary>
        public override string ToString()
        {
            return $"Cylinder(PointA: {PointA}, PointB: {PointB}, Radius: {Radius})";
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on this cylinder.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);

        /// <summary>
        /// Returns true if this cylinder overlaps or touches <paramref name="sphere"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere) => Collisions3d.Intersects(this, sphere);

        /// <summary>
        /// Returns true if this cylinder overlaps or touches <paramref name="aabb"/>. See
        /// <see cref="Collisions3d.Intersects(AABB, Cylinder)"/> for the exact-AABB approach this uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB aabb) => Collisions3d.Intersects(aabb, this);

        /// <summary>
        /// Returns true if this cylinder overlaps or touches <paramref name="cube"/>. See
        /// <see cref="Collisions3d.Intersects(AABB, Cylinder)"/> for the exact-AABB approach this uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cube cube) => Collisions3d.Intersects(cube, this);

        /// <summary>
        /// Returns true if this cylinder overlaps or touches <paramref name="other"/>. See
        /// <see cref="Collisions3d.Intersects(Cylinder, Cylinder)"/> for the capsule-approximation this uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cylinder other) => Collisions3d.Intersects(this, other);
    }
}
