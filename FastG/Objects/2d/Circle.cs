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
    public readonly struct Circle : I2d, IEquatable<Circle>
    {
        /// <summary>
        /// A circle of radius 1 centered at the origin.
        /// </summary>
        public static readonly Circle UNIT_CIRCLE = new();

        public Point2 Center { get; init; }

        public float Radius { get; init; }

        [JsonIgnore]
        public float Left => Center.X - Radius;

        [JsonIgnore]
        public float Right => Center.X + Radius;

        [JsonIgnore]
        public float Top => Center.Y - Radius;

        [JsonIgnore]
        public float Bottom => Center.Y + Radius;

        [JsonIgnore]
        public float Area => MathF.PI * Radius * Radius;

        [JsonIgnore]
        public float Circumference => Constants.TWO_PI * Radius;

        [JsonIgnore]
        public float Diameter => Radius * 2;

        [JsonIgnore]
        public float Perimeter => Circumference;

        /// <summary>
        /// Creates a unit circle (radius 1) centered at the origin.
        /// </summary>
        public Circle() : this(0f, 0f, 1f) { }

        /// <summary>
        /// Creates a circle of the given radius centered at the origin.
        /// </summary>
        public Circle(float radius) : this(0f, 0f, radius) { }

        /// <summary>
        /// Creates a circle centered at (<paramref name="x"/>, <paramref name="y"/>) with the given radius.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="radius"/> is zero or negative.
        /// </summary>
        public Circle(float x, float y, float radius)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(radius, 0f);

            Center = new Point2(x, y);
            Radius = radius;
        }

        /// <summary>
        /// Creates a unit circle (radius 1) centered at <paramref name="position"/>.
        /// </summary>
        public Circle(Point2 position) : this(position.X, position.Y, 1f) { }

        /// <summary>
        /// Creates a circle of specified <paramref name="radius"/> centered at <paramref name="position"/>.
        /// </summary>
        public Circle(Point2 position, float radius) : this(position.X, position.Y, radius) { }

        /// <summary>
        /// Returns a copy of the circle translated by vector <paramref name="v"/> (radius unchanged).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator +(Circle c, Vector2 v)
        {
            return new Circle(c.Center.X + v.X, c.Center.Y + v.Y, c.Radius);
        }

        /// <summary>
        /// Returns a copy of the circle translated by the negation of vector <paramref name="v"/> (radius unchanged).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator -(Circle c, Vector2 v)
        {
            return new Circle(c.Center.X - v.X, c.Center.Y - v.Y, c.Radius);
        }

        /// <summary>
        /// Returns a copy of the circle with its radius multiplied by <paramref name="scale"/> (center unchanged).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Circle Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return new Circle(Center.X, Center.Y, Radius * scale);
        }

        /// <summary>
        /// Returns a copy of the circle with its radius multiplied by <paramref name="scale"/> (center unchanged).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator *(Circle c, float scale) => c.Scale(scale);

        /// <summary>
        /// Returns a copy of the circle with its radius divided by <paramref name="scale"/> (center unchanged).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator /(Circle c, float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return c.Scale(1f / scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Circle"/> with the same center and radius.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Circle other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other circle has the same center and radius (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Circle c)
        {
            return Center.Equals(c.Center) && Radius.Equals(c.Radius);
        }

        /// <summary>
        /// Returns true if both circles have the same center and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Circle a, Circle b) => a.Equals(b);

        /// <summary>
        /// Returns true if the circles differ in center or radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Circle a, Circle b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the center and radius.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius);
        }

        /// <summary>
        /// Returns a string of the form "Circle(Center: (x, y), Radius: r)".
        /// </summary>
        public override string ToString()
        {
            return $"Circle(Center: {Center}, Radius: {Radius})";
        }

        /// <summary>
        /// Checks to see if circles are intersecting.
        /// Tangent circles will return true.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Circle c) => Collisions2d.Intersects(this, c);

        /// <summary>
        /// Returns true if this circle overlaps or touches <paramref name="r"/>, using the closest-point-on-rectangle test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AARectangle r) => Collisions2d.Intersects(this, r);

        /// <summary>
        /// Returns true if point <paramref name="p"/> lies inside or on this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 p) => Collisions2d.Contains(this, p);

        /// <summary>
        /// Returns true if all four corners of <paramref name="r"/> lie inside or on this circle (i.e. the rectangle is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(AARectangle r) => Collisions2d.Contains(this, r);

        /// <summary>
        /// Returns true if all three vertices of <paramref name="t"/> lie inside or on this circle (i.e. the triangle is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Triangle2 t) => Collisions2d.Contains(this, t);
    }
}
