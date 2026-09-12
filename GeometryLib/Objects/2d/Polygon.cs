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
    public class Polygon : I2d, IEquatable<Polygon>
    {
        /// <summary>
        /// A five-sided polygon with vertices spanning the unit square (0..1 on both axes).
        /// </summary>
        public static readonly Polygon PENTAGON = new() { Vertices = [new() { X = 0.5f, Y = 0f }, new() { X = 1f, Y = 0.381966f }, new() { X = 0.809017f, Y = 1f }, new() { X = 0.190983f, Y = 1f }, new() { X = 0f, Y = 0.381966f }] };

        /// <summary>
        /// A six-sided polygon with vertices spanning the unit square (0..1 on both axes).
        /// </summary>
        public static readonly Polygon HEXAGON = new() { Vertices = [new() { X = 0.5f, Y = 0f }, new() { X = 1f, Y = 0.25f }, new() { X = 1f, Y = 0.75f }, new() { X = 0.5f, Y = 1f }, new() { X = 0f, Y = 0.75f }, new() { X = 0f, Y = 0.25f }] };

        public List<Point2> Vertices { get; set; } = new();

        /// <summary>
        /// The area enclosed by the polygon, computed with the shoelace formula. Result is unsigned,
        /// so winding order does not matter; self-intersecting polygons give an ill-defined value.
        /// </summary>
        [JsonIgnore]
        public float Area
        {
            get
            {
                var vertices = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(Vertices);
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
                var vertices = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(Vertices);
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
        public int Sides => Vertices.Count;

        /// <summary>
        /// Creates an empty polygon with no vertices.
        /// </summary>
        public Polygon() { }

        /// <summary>
        /// Creates a polygon that wraps the given vertex list (the list is stored by reference, not copied).
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="vertices"/> is null.
        /// </summary>
        public Polygon(List<Point2> vertices)
        {
            ArgumentNullException.ThrowIfNull(vertices);

            Vertices = vertices;
        }

        /// <summary>
        /// Creates a deep copy of an existing polygon (its vertex list is copied into a new list).
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="polygon"/> is null.
        /// </summary>
        public Polygon(Polygon polygon)
        {
            ArgumentNullException.ThrowIfNull(polygon);

            Vertices = [.. polygon.Vertices];
        }

        /// <summary>
        /// Returns true if (<paramref name="x"/>, <paramref name="y"/>) lies inside the polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(float x, float y) => Contains(new Point2(x, y));

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside the polygon, using a ray-casting (even-odd) test.
        /// Always returns false for polygons with fewer than three vertices; behavior on the boundary is not guaranteed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);

        /// <summary>
        /// Creates a new <see cref="Polygon"/> that is shifted by a vector.
        /// </summary>
        public static Polygon operator +(Polygon p, Vector2 v)
        {
            ArgumentNullException.ThrowIfNull(p);

            var output = new Polygon();

            foreach (var vertex in p.Vertices)
                output.Vertices.Add(vertex + v);

            return output;
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex shifted by the negation of a vector.
        /// </summary>
        public static Polygon operator -(Polygon p, Vector2 v)
        {
            ArgumentNullException.ThrowIfNull(p);

            var output = new Polygon();

            foreach (var vertex in p.Vertices)
                output.Vertices.Add(vertex - v);

            return output;
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex scaled about the origin by <paramref name="scale"/>.
        /// </summary>
        public static Polygon operator *(Polygon p, float scale)
        {
            ArgumentNullException.ThrowIfNull(p);

            var output = new Polygon();

            foreach (var vertex in p.Vertices)
                output.Vertices.Add(new Point2(vertex.X * scale, vertex.Y * scale));

            return output;
        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with every vertex divided about the origin by <paramref name="scale"/>.
        /// Throws <see cref="DivideByZeroException"/> if <paramref name="scale"/> is zero.
        /// </summary>
        public static Polygon operator /(Polygon p, float scale)
        {
            ArgumentNullException.ThrowIfNull(p);

            if (scale == 0f)
                throw new DivideByZeroException(nameof(scale));

            var output = new Polygon();

            foreach (var vertex in p.Vertices)
                output.Vertices.Add(new Point2(vertex.X / scale, vertex.Y / scale));

            return output;
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Polygon"/> with the same vertices in the same order.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;

            var newObj = (Polygon)obj;
            return Equals(newObj);
        }

        /// <summary>
        /// Returns true if the other polygon has the same vertex count and identical vertices in the same order.
        /// </summary>
        public bool Equals(Polygon p)
        {
            if (p is null)
                return false;

            if (Vertices.Count != p.Vertices.Count)
                return false;

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (!Vertices[i].Equals(p.Vertices[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code formed by XOR-ing the hash codes of all vertices (order-independent).
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int output = 0;

                foreach (var vertex in Vertices)
                    output ^= vertex.GetHashCode();

                return output;
            }
        }

        /// <summary>
        /// Returns true if both polygons are null, or have the same vertices in the same order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Polygon a, Polygon b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        /// <summary>
        /// Returns true if exactly one polygon is null, or their vertices differ.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Polygon a, Polygon b) => !(a == b);

        /// <summary>
        /// Returns a string of the form "Polygon[(x, y), (x, y), ...]".
        /// </summary>
        public override string ToString()
        {
            return $"Polygon[{string.Join(", ", Vertices)}]";
        }
    }
}
