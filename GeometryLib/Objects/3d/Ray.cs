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
    public readonly struct Ray : IEquatable<Ray>
    {
        /// <summary>
        /// A ray from the origin (0, 0, 0) pointing along the (1, 1, 1) direction. As with any ray the
        /// direction is normalized, so <see cref="Direction"/> is (0.577.., 0.577.., 0.577..), not (1, 1, 1).
        /// </summary>
        public static readonly Ray UNIT_RAY = new(Point3.ZERO, Vector3.ONE);

        /// <summary>
        /// The point the ray starts from.
        /// </summary>
        public Point3 Origin { get; init; }

        /// <summary>
        /// The ray direction, always stored as a unit vector.
        /// </summary>
        public Vector3 Direction { get; init; }

        /// <summary>
        /// Creates a ray from an origin and a direction. The direction is normalized on construction.
        /// Throws <see cref="ArgumentException"/> if <paramref name="direction"/> is the zero vector.
        /// </summary>
        public Ray(Point3 origin, Vector3 direction)
        {
            if (direction.LengthSquared() == 0f)
                throw new ArgumentException("Direction vector must be non-zero.", nameof(direction));

            Origin = origin;
            Direction = Vector3.Normalize(direction);
        }

        /// <summary>
        /// Creates a ray from the raw coordinates of its origin and direction. The direction is normalized on construction.
        /// Throws <see cref="ArgumentException"/> if the direction is the zero vector.
        /// </summary>
        public Ray(float originX, float originY, float originZ, float directionX, float directionY, float directionZ)
            : this(new Point3(originX, originY, originZ), new Vector3(directionX, directionY, directionZ)) { }

        /// <summary>
        /// Returns the point at the given signed <paramref name="distance"/> along the ray from its origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 PointAt(float distance)
        {
            return new Point3(
                Origin.X + Direction.X * distance,
                Origin.Y + Direction.Y * distance,
                Origin.Z + Direction.Z * distance);
        }

        /// <summary>
        /// Tests for intersection with a sphere. On a hit, returns true and sets <paramref name="distance"/> to the
        /// distance along the ray of the nearest non-negative intersection; otherwise returns false and sets it to 0.
        /// An origin inside the sphere counts as a hit at the exit point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere, out float distance) => Collisions3d.Intersects(this, sphere, out distance);

        /// <summary>
        /// Tests for intersection with an axis-aligned box using the slab method. On a hit, returns true and sets
        /// <paramref name="distance"/> to the entry distance along the ray (clamped to 0 when the origin is inside);
        /// otherwise returns false and sets it to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB aabb, out float distance) => Collisions3d.Intersects(this, aabb, out distance);

        /// <summary>
        /// Tests for intersection with a cube by treating it as an axis-aligned box. See <see cref="Intersects(AABB, out float)"/>
        /// for the meaning of <paramref name="distance"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cube cube, out float distance) => Collisions3d.Intersects(this, cube, out distance);

        /// <summary>
        /// Tests for intersection with a plane. On a hit in front of the origin, returns true and sets
        /// <paramref name="distance"/> to the distance along the ray; returns false (distance 0) when the ray is
        /// parallel to the plane or the intersection lies behind the origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Plane3 plane, out float distance) => Collisions3d.Intersects(this, plane, out distance);

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Ray"/> with the same origin and direction.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Ray other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other ray has the same origin and (normalized) direction (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ray other)
        {
            return Origin.Equals(other.Origin) && Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Returns true if both rays have the same origin and direction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ray a, Ray b) => a.Equals(b);

        /// <summary>
        /// Returns true if the rays differ in origin or direction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ray a, Ray b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the origin and direction.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Origin, Direction);
        }

        /// <summary>
        /// Returns a string of the form "Ray(Origin: (x, y, z), Direction: &lt;x, y, z&gt;)".
        /// </summary>
        public override string ToString()
        {
            return $"Ray(Origin: {Origin}, Direction: {Direction})";
        }
    }
}
