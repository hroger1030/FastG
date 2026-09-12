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
    public readonly struct Triangle3 : IEquatable<Triangle3>
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
                return 0.5f * cross.Length();
            }
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
    }
}
