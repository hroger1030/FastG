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
    /// <summary>
    /// An immutable polygon backed by an owned <see cref="Point2"/> array. Every constructor copies its input,
    /// so once built, a <see cref="Polygon"/> can never be mutated through any public member (<see cref="Vertices"/>
    /// only ever exposes a read-only view) - matching the value semantics every other shape in this library has.
    /// A polygon always has at least three vertices - there is no such thing as a one- or two-sided polygon,
    /// so construction with fewer is rejected rather than silently allowed.
    /// </summary>
    public readonly struct Polygon : I2d, IEquatable<Polygon>
    {
        private const int MINIMUM_NUMBER_OF_VERTICIES = 3;

        private readonly Point2[] _Vertices;

        /// <summary>
        /// A five-sided polygon with vertices spanning the unit square (0..1 on both axes).
        /// </summary>
        public static readonly Polygon PENTAGON = new(
            [new(0.5f, 0f), new(1f, 0.381966f), new(0.809017f, 1f), new(0.190983f, 1f), new(0f, 0.381966f)]);

        /// <summary>
        /// A six-sided polygon with vertices spanning the unit square (0..1 on both axes).
        /// </summary>
        public static readonly Polygon HEXAGON = new(
            [new(0.5f, 0f), new(1f, 0.25f), new(1f, 0.75f), new(0.5f, 1f), new(0f, 0.75f), new(0f, 0.25f)]);

        /// <summary>
        /// A read-only view of the polygon's vertices, in order. Reapplies the same guards as
        /// <see cref="Polygon(Point2[])"/> - throwing <see cref="ArgumentNullException"/> or
        /// <see cref="ArgumentOutOfRangeException"/> - if this <see cref="Polygon"/> was never constructed
        /// through it (e.g. <c>default(Polygon)</c>). The array-to-span conversion below is normally
        /// null-tolerant and would otherwise silently hand back an empty span instead of surfacing the problem.
        /// </summary>
        public ReadOnlySpan<Point2> Vertices
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_Vertices);
                ArgumentOutOfRangeException.ThrowIfLessThan(_Vertices.Length, MINIMUM_NUMBER_OF_VERTICIES);

                return _Vertices;
            }
        }

        /// <summary>
        /// The area enclosed by the polygon, computed with the shoelace formula. Result is unsigned,
        /// so winding order does not matter; self-intersecting polygons give an ill-defined value.
        /// </summary>
        [JsonIgnore]
        public float Area
        {
            get
            {
                var vertices = Vertices;
                float area = 0;

                for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++)
                {
                    var p1 = vertices[j];
                    var p2 = vertices[i];
                    area += (p1.X * p2.Y) - (p1.Y * p2.X);
                }

                return MathF.Abs(area / 2);
            }
        }

        /// <summary>
        /// The total edge length of the polygon, including the closing edge from the last vertex back to the first.
        /// </summary>
        [JsonIgnore]
        public float Perimeter
        {
            get
            {
                var vertices = Vertices;
                float perimeter = 0;

                for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++)
                    perimeter += vertices[j].DistanceTo(vertices[i]);

                return perimeter;
            }
        }

        /// <summary>
        /// The number of vertices (equivalently, the number of edges).
        /// </summary>
        [JsonIgnore]
        public int Sides => Vertices.Length;

        /// <summary>
        /// The centroid: the average of all vertices. For an irregular polygon (vertices not evenly spaced
        /// around the boundary) this is not the same point as the area-weighted centroid, but it's cheap and
        /// always computable, matching <see cref="Triangle2.Centroid"/>.
        /// </summary>
        [JsonIgnore]
        public Point2 Centroid
        {
            get
            {
                var vertices = Vertices;
                float sumX = 0f, sumY = 0f;

                foreach (var vertex in vertices)
                {
                    sumX += vertex.X;
                    sumY += vertex.Y;
                }

                return new Point2(sumX / vertices.Length, sumY / vertices.Length);
            }
        }

        /// <summary>
        /// Creates a polygon from the given vertices. The array is copied, so mutating <paramref name="vertices"/>
        /// afterward does not affect this polygon. Throws <see cref="ArgumentNullException"/> if <paramref name="vertices"/>
        /// is null, or <see cref="ArgumentOutOfRangeException"/> if it has fewer than three elements.
        /// </summary>
        public Polygon(Point2[] vertices)
        {
            ArgumentNullException.ThrowIfNull(vertices);
            ArgumentOutOfRangeException.ThrowIfLessThan(vertices.Length, MINIMUM_NUMBER_OF_VERTICIES);

            _Vertices = [.. vertices];
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> that is shifted by a vector.
        /// </summary>
        public static Polygon operator +(Polygon p, Vector2 v)
        {
            var source = p.Vertices;
            var result = new Point2[source.Length];

            for (int i = 0; i < source.Length; i++)
                result[i] = source[i] + v;

            return new Polygon(result);
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex shifted by the negation of a vector.
        /// </summary>
        public static Polygon operator -(Polygon p, Vector2 v)
        {
            var source = p.Vertices;
            var result = new Point2[source.Length];

            for (int i = 0; i < source.Length; i++)
                result[i] = source[i] - v;

            return new Polygon(result);
        }

        /// <summary>
        /// Returns a copy of this polygon with every vertex scaled about its <see cref="Centroid"/> by
        /// <paramref name="scale"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/>
        /// is zero or negative.
        /// </summary>
        public Polygon Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            var centroid = Centroid;
            var source = Vertices;
            var result = new Point2[source.Length];

            for (int i = 0; i < source.Length; i++)
                result[i] = new Point2(
                    centroid.X + (source[i].X - centroid.X) * scale,
                    centroid.Y + (source[i].Y - centroid.Y) * scale);

            return new Polygon(result);
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex scaled about the polygon's <see cref="Centroid"/>
        /// by <paramref name="scale"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/>
        /// is zero or negative.
        /// </summary>
        public static Polygon operator *(Polygon p, float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            var centroid = p.Centroid;
            var source = p.Vertices;
            var result = new Point2[source.Length];

            for (int i = 0; i < source.Length; i++)
                result[i] = new Point2(
                    centroid.X + (source[i].X - centroid.X) * scale,
                    centroid.Y + (source[i].Y - centroid.Y) * scale);

            return new Polygon(result);
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex scaled about the polygon's <see cref="Centroid"/>
        /// by 1/<paramref name="scale"/>. Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/>
        /// is zero or negative.
        /// </summary>
        public static Polygon operator /(Polygon p, float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return p * (1f / scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Polygon"/> with the same vertices in the same order.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Polygon other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other polygon has the same vertex count and identical vertices in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Polygon p)
        {
            return Vertices.SequenceEqual(p.Vertices);
        }

        /// <summary>
        /// Returns a hash code derived from every vertex, in order, so vertices that compare equal hash equal.
        /// </summary>
        public override int GetHashCode()
        {
            var hash = new HashCode();

            foreach (var vertex in Vertices)
                hash.Add(vertex);

            return hash.ToHashCode();
        }

        /// <summary>
        /// Returns true if both polygons have the same vertices in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Polygon a, Polygon b) => a.Equals(b);

        /// <summary>
        /// Returns true if the polygons' vertices differ.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Polygon a, Polygon b) => !a.Equals(b);

        /// <summary>
        /// Returns a string of the form "Polygon[(x, y), (x, y), ...]". Throws via <see cref="Vertices"/> if
        /// this <see cref="Polygon"/> was never properly constructed.
        /// </summary>
        public override string ToString()
        {
            return $"Polygon[{string.Join(", ", Vertices.ToArray())}]";
        }

        /// <summary>
        /// Returns true if (<paramref name="x"/>, <paramref name="y"/>) lies inside the polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(float x, float y) => Contains(new Point2(x, y));

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside the polygon, using a ray-casting (even-odd) test.
        /// Behavior on the boundary is not guaranteed. Throws via <see cref="Vertices"/> if this
        /// <see cref="Polygon"/> was never properly constructed (e.g. <c>default(Polygon)</c>) - this
        /// deliberately does not degrade to "false" for that case.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);
    }
}
