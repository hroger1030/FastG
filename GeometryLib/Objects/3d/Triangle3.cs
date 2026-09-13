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

namespace Geometry
{
    public readonly struct Triangle3 : I2d, IEquatable<Triangle3>
    {
        public Point3 A { get; init; }

        public Point3 B { get; init; }

        public Point3 C { get; init; }

        /// <summary>
        /// The sum of the three side lengths.
        /// </summary>
        [JsonIgnore]
        public float Perimeter =>
            Vector3.DistanceTo(new Vector3(A), new Vector3(B)) +
            Vector3.DistanceTo(new Vector3(B), new Vector3(C)) +
            Vector3.DistanceTo(new Vector3(C), new Vector3(A));

        /// <summary>
        /// The area of the triangle, computed as half the magnitude of the cross product of two edge vectors.
        /// </summary>
        [JsonIgnore]
        public float Area
        {
            get
            {
                var ab = new Vector3(A, B);
                var ac = new Vector3(A, C);
                var cross = Vector3.Cross(ab, ac);
                return 0.5f * cross.Length;
            }
        }

        /// <summary>
        /// The centroid: the average of the three vertices. Always lies inside the triangle, unlike the
        /// circumcenter or orthocenter, which can fall outside an obtuse triangle.
        /// </summary>
        [JsonIgnore]
        public Point3 Centroid => new((A.X + B.X + C.X) / 3f, (A.Y + B.Y + C.Y) / 3f, (A.Z + B.Z + C.Z) / 3f);

        /// <summary>
        /// Returns a copy of this triangle scaled uniformly about its <see cref="Centroid"/> by <paramref name="scale"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Triangle3 Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            var centroid = Centroid;

            return new Triangle3(
                new Point3(centroid.X + (A.X - centroid.X) * scale, centroid.Y + (A.Y - centroid.Y) * scale, centroid.Z + (A.Z - centroid.Z) * scale),
                new Point3(centroid.X + (B.X - centroid.X) * scale, centroid.Y + (B.Y - centroid.Y) * scale, centroid.Z + (B.Z - centroid.Z) * scale),
                new Point3(centroid.X + (C.X - centroid.X) * scale, centroid.Y + (C.Y - centroid.Y) * scale, centroid.Z + (C.Z - centroid.Z) * scale));
        }

        /// <summary>
        /// Creates a triangle from three vertices. Throws <see cref="ArgumentException"/> if any two vertices coincide.
        /// </summary>
        public Triangle3(Point3 a, Point3 b, Point3 c)
        {

            if (a.Equals(b) || b.Equals(c) || c.Equals(a))
                throw new ArgumentException("All points must be distinct points with separate locations");

            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Creates a triangle from the raw coordinates of its three vertices.
        /// Throws <see cref="ArgumentException"/> if any two vertices coincide.
        /// </summary>
        public Triangle3(float ax, float ay, float az, float bx, float by, float bz, float cx, float cy, float cz)
            : this(new Point3(ax, ay, az), new Point3(bx, by, bz), new Point3(cx, cy, cz)) { }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Triangle3"/> with the same vertices in the same order.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Triangle3 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other triangle has the same vertices in the same order (A, B, C positionally equal).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Triangle3 other)
        {
            return A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C);
        }

        /// <summary>
        /// Returns true if both triangles have the same vertices in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Triangle3 a, Triangle3 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the triangles differ in any vertex or vertex ordering.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Triangle3 a, Triangle3 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the three vertices.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(A, B, C);
        }

        /// <summary>
        /// Returns a string of the form "Triangle3(A: (x, y, z), B: (x, y, z), C: (x, y, z))".
        /// </summary>
        public override string ToString()
        {
            return $"Triangle3(A: {A}, B: {B}, C: {C})";
        }

        // The method below is a thin redirect into Collisions3d, kept here only for call-site convenience
        // (e.g. triangle.Contains(point) instead of Collisions3d.Contains(triangle, point)). The real logic lives there.

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of this triangle. The point must
        /// be coplanar with the triangle (within <see cref="Constants.FLOAT_ERROR_MARGIN"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);
    }
}
