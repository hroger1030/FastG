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
    public readonly struct Triangle2 : I2d, IEquatable<Triangle2>
    {
        /// <summary>
        /// A right triangle with legs of length 1 along the axes, with its right angle at the origin
        /// and a clockwise winding.
        /// </summary>
        public static readonly Triangle2 UNIT_TRIANGLE = new(new Point2(0f, 0f), new Point2(0f, 1f), new Point2(1f, 0f));

        /// <summary>
        /// Classification of a triangle by its side lengths.
        /// </summary>
        public enum Type
        {
            /// <summary>All three sides equal.</summary>
            Equilateral,
            /// <summary>Exactly two sides equal.</summary>
            Isosceles,
            /// <summary>No two sides equal.</summary>
            Scalene,
        }

        public Point2 A { get; init; }

        public Point2 B { get; init; }

        public Point2 C { get; init; }

        /// <summary>
        /// The sum of the three side lengths.
        /// </summary>
        [JsonIgnore]
        public float Perimeter
        {
            get { return A.DistanceTo(B) + B.DistanceTo(C) + C.DistanceTo(A); }
        }

        /// <summary>
        /// The area of the triangle, computed with a numerically stable form of Heron's formula
        /// (sides sorted descending before the subtraction terms).
        /// </summary>
        [JsonIgnore]
        public float Area
        {
            get
            {
                float buffer;

                // using heron's formula
                var a = A.DistanceTo(B);
                var b = B.DistanceTo(C);
                var c = C.DistanceTo(A);

                // arrange a > b > c
                if (b > a)
                {
                    buffer = a;
                    a = b;
                    b = buffer;
                }

                if (c > a)
                {
                    buffer = a;
                    a = c;
                    c = buffer;
                }

                if (c > b)
                {
                    buffer = b;
                    b = c;
                    c = buffer;
                }

                buffer = (a + (b + c)) * (c - (a - b)) * (c + (a - b)) * (a + (b - c));
                return MathF.Sqrt(buffer) * 0.25f;
            }
        }

        /// <summary>
        /// The centroid: the average of the three vertices. Always lies inside the triangle, unlike the
        /// circumcenter or orthocenter, which can fall outside an obtuse triangle.
        /// </summary>
        [JsonIgnore]
        public Point2 Centroid => new((A.X + B.X + C.X) / 3f, (A.Y + B.Y + C.Y) / 3f);

        /// <summary>
        /// Classifies this triangle as <see cref="Type.Equilateral"/>, <see cref="Type.Isosceles"/> or <see cref="Type.Scalene"/>
        /// based on exact side-length equality.
        /// </summary>
        [JsonIgnore]
        public Type TriangleType
        {
            get
            {
                // Squared side lengths are enough to compare for equality, and skip three square roots.
                var a = A.DistanceSquaredTo(B);
                var b = B.DistanceSquaredTo(C);
                var c = C.DistanceSquaredTo(A);

                if (a == b && b == c)
                    return Type.Equilateral;

                if (a == b || b == c || c == a)
                    return Type.Isosceles;

                return Type.Scalene;
            }
        }

        /// <summary>
        /// Creates a triangle from three vertices. Throws <see cref="ArgumentException"/> if any two vertices coincide.
        /// </summary>
        public Triangle2(Point2 p1, Point2 p2, Point2 p3)
        {
            if (p1 == p2 || p2 == p3 || p3 == p1)
                throw new ArgumentException("All points must be distinct points with separate locations");

            A = p1;
            B = p2;
            C = p3;
        }

        /// <summary>
        /// Creates a triangle from the raw coordinates of its three vertices.
        /// Throws <see cref="ArgumentException"/> if any two vertices coincide.
        /// </summary>
        public Triangle2(float ax, float ay, float bx, float by, float cx, float cy)
            : this(new Point2(ax, ay), new Point2(bx, by), new Point2(cx, cy)) { }

        /// <summary>
        /// Returns a copy of this triangle scaled uniformly about its <see cref="Centroid"/> by <paramref name="scale"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Triangle2 Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            var centroid = Centroid;

            return new Triangle2(
                new Point2(centroid.X + (A.X - centroid.X) * scale, centroid.Y + (A.Y - centroid.Y) * scale),
                new Point2(centroid.X + (B.X - centroid.X) * scale, centroid.Y + (B.Y - centroid.Y) * scale),
                new Point2(centroid.X + (C.X - centroid.X) * scale, centroid.Y + (C.Y - centroid.Y) * scale));
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Triangle2"/> with the same vertices in the same order.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Triangle2 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other triangle has the same vertices in the same order (A, B, C positionally equal).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Triangle2 t)
        {
            return A.Equals(t.A) && B.Equals(t.B) && C.Equals(t.C);
        }

        /// <summary>
        /// Returns true if both triangles have the same vertices in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Triangle2 a, Triangle2 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the triangles differ in any vertex or vertex ordering.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Triangle2 a, Triangle2 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the three vertices.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(A, B, C);
        }

        /// <summary>
        /// Returns a string of the form "Triangle2(A: (x, y), B: (x, y), C: (x, y))".
        /// </summary>
        public override string ToString()
        {
            return $"Triangle2(A: {A}, B: {B}, C: {C})";
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of this triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);

        /// <summary>
        /// Returns true if this triangle overlaps or touches <paramref name="other"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Triangle2 other) => Collisions2d.Intersects(this, other);
    }
}
