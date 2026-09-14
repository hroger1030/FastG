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
    public readonly struct Sphere : I3d, IEquatable<Sphere>
    {
        /// <summary>
        /// A sphere of radius 1 centered at the origin.
        /// </summary>
        public static readonly Sphere UNIT_SPHERE = new(Point3.ZERO, 1f);

        public Point3 Center { get; init; }

        public float Radius { get; init; }

        /// <summary>
        /// The volume of the sphere (4/3 * PI * r^3).
        /// </summary>
        [JsonIgnore]
        public float Volume => (4f / 3f) * MathF.PI * Radius * Radius * Radius;

        /// <summary>
        /// The surface area of the sphere (4 * PI * r^2).
        /// </summary>
        [JsonIgnore]
        public float SurfaceArea => 4f * MathF.PI * Radius * Radius;

        /// <summary>
        /// Creates a sphere from a center and radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is zero or negative.
        /// </summary>
        public Sphere(Point3 center, float radius)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(radius, 0f);

            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates a sphere from the raw coordinates of its center and a radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is zero or negative.
        /// </summary>
        public Sphere(float centerX, float centerY, float centerZ, float radius)
            : this(new Point3(centerX, centerY, centerZ), radius) { }

        /// <summary>
        /// Returns a copy of the sphere with its radius multiplied by <paramref name="scale"/> (center unchanged).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Sphere Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return new Sphere(Center, Radius * scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Sphere"/> with the same center and radius.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Sphere other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other sphere has the same center and radius (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Sphere other)
        {
            return Center.Equals(other.Center) && Radius.Equals(other.Radius);
        }

        /// <summary>
        /// Returns true if both spheres have the same center and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Sphere a, Sphere b) => a.Equals(b);

        /// <summary>
        /// Returns true if the spheres differ in center or radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Sphere a, Sphere b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the center and radius.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius);
        }

        /// <summary>
        /// Returns a string of the form "Sphere(Center: (x, y, z), Radius: r)".
        /// </summary>
        public override string ToString()
        {
            return $"Sphere(Center: {Center}, Radius: {Radius})";
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on this sphere.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);

        /// <summary>
        /// Returns true if this sphere overlaps or touches <paramref name="other"/> (distance between centers &lt;= sum of radii).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere other) => Collisions3d.Intersects(this, other);

        /// <summary>
        /// Gets whether or not a specified <see cref="Cube"/> intersects with this <see cref="Sphere"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cube c) => Collisions3d.Intersects(c, this);

        /// <summary>
        /// Returns true if all eight corners of <paramref name="c"/> lie inside or on this sphere (i.e. the cube is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Cube c) => Collisions3d.Contains(this, c);

        /// <summary>
        /// Returns true if this sphere overlaps or touches <paramref name="aabb"/>, using the closest-point-on-box test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB aabb) => Collisions3d.Intersects(aabb, this);

        /// <summary>
        /// Returns true if all eight corners of <paramref name="aabb"/> lie inside or on this sphere (i.e. the box is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(AABB aabb) => Collisions3d.Contains(this, aabb);

        /// <summary>
        /// Returns true if this sphere intersects <paramref name="plane"/> (the distance from this sphere's center
        /// to the plane is no greater than its radius).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Plane3 plane) => Collisions3d.Intersects(plane, this);
    }
}
