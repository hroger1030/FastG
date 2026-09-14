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
    public readonly struct Plane3 : IEquatable<Plane3>
    {
        /// <summary>
        /// The XY plane (normal +Z, through the origin), provided as a convenience constant.
        /// </summary>
        public static readonly Plane3 ZERO_PLANE = new(new Vector3(0f, 0f, 1f), 0f);

        /// <summary>
        /// The plane normal. Not required to be unit length; see <see cref="Normalize"/>.
        /// </summary>
        public Vector3 Normal { get; init; }

        /// <summary>
        /// The plane constant D in the equation: Normal.X*x + Normal.Y*y + Normal.Z*z + D = 0.
        /// </summary>
        public float D { get; init; }

        /// <summary>
        /// Creates a plane from a normal and the constant D of the equation dot(Normal, p) + D = 0.
        /// Throws <see cref="ArgumentException"/> if <paramref name="normal"/> is the zero vector.
        /// </summary>
        public Plane3(Vector3 normal, float d)
        {
            if (normal.X == 0f && normal.Y == 0f && normal.Z == 0f)
                throw new ArgumentException("Normal must be non-zero.", nameof(normal));

            Normal = normal;
            D = d;
        }

        /// <summary>
        /// Creates a plane from the raw components of its normal and the constant D of the equation dot(Normal, p) + D = 0.
        /// Throws <see cref="ArgumentException"/> if the normal is the zero vector.
        /// </summary>
        public Plane3(float normalX, float normalY, float normalZ, float d)
            : this(new Vector3(normalX, normalY, normalZ), d) { }

        /// <summary>
        /// Returns the signed distance from <paramref name="point"/> to this plane. Positive on the side the
        /// normal points toward, negative on the other side. Correct for a non-unit normal (it divides by the normal length).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float DistanceTo(Point3 point)
        {

            float length = MathF.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z);

            return (Normal.X * point.X + Normal.Y * point.Y + Normal.Z * point.Z + D) / length;
        }

        /// <summary>
        /// Returns an equivalent plane whose normal has unit length (normal and D both divided by the current normal length).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane3 Normalize()
        {
            float length = MathF.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z);
            var normalized = new Vector3(Normal.X / length, Normal.Y / length, Normal.Z / length);

            return new Plane3(normalized, D / length);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Plane3"/> with the same normal and D.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Plane3 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other plane has the same normal and D (no tolerance; equivalent planes with
        /// scaled coefficients are not considered equal).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Plane3 other)
        {
            return Normal.Equals(other.Normal) && D.Equals(other.D);
        }

        /// <summary>
        /// Returns true if both planes have the same normal and D.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Plane3 a, Plane3 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the planes differ in normal or D.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Plane3 a, Plane3 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the normal and D.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(Normal, D);

        /// <summary>
        /// Returns a string of the form "Plane3(Normal: &lt;x, y, z&gt;, D: d)".
        /// </summary>
        public override string ToString()
        {
            return $"Plane3(Normal: {Normal}, D: {D})";
        }

        // The method below is a thin redirect into Collisions3d, kept here only for call-site convenience
        // (e.g. plane.Intersects(sphere) instead of Collisions3d.Intersects(plane, sphere)). The real logic lives there.

        /// <summary>
        /// Returns true if this plane intersects <paramref name="sphere"/> (the distance from the sphere's
        /// center to this plane is no greater than its radius).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere) => Collisions3d.Intersects(this, sphere);
    }
}
