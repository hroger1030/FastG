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
    public readonly struct AARectangle : I2d, IEquatable<AARectangle>
    {
        /// <summary>
        /// A 1x1 rectangle with its top-left corner at the origin.
        /// </summary>
        public static readonly AARectangle UNIT_AARECTANGLE = new(0f, 0f, 1f, 1f);

        /// <summary>
        /// Returns the x coordinate of the left edge of this <see cref="AARectangle"/>.
        /// </summary>
        public float Left { get; init; }

        /// <summary>
        /// Returns the x coordinate of the right edge of this <see cref="AARectangle"/>.
        /// </summary>
        public float Right { get; init; }

        /// <summary>
        /// Returns the y coordinate of the top edge of this <see cref="AARectangle"/>.
        /// </summary>
        public float Top { get; init; }

        /// <summary>
        /// Returns the y coordinate of the bottom edge of this <see cref="AARectangle"/>.
        /// </summary>
        public float Bottom { get; init; }

        [JsonIgnore]
        public float X => Left;

        [JsonIgnore]
        public float Y => Top;

        [JsonIgnore]
        public float Width => Right - Left;

        [JsonIgnore]
        public float Height => Bottom - Top;

        [JsonIgnore]
        public Point2 TopLeftCorner => new(Left, Top);

        [JsonIgnore]
        public Point2 TopRightCorner => new(Right, Top);

        [JsonIgnore]
        public Point2 BottomLeftCorner => new(Left, Bottom);

        [JsonIgnore]
        public Point2 BottomRightCorner => new(Right, Bottom);

        /// <summary>
        /// The top-left coordinates of this <see cref="AARectangle"/>.
        /// </summary>
        [JsonIgnore]
        public Point2 Location => TopLeftCorner;

        /// <summary>
        /// The width-height coordinates of this <see cref="AARectangle"/>.
        /// </summary>
        [JsonIgnore]
        public Point2 Size => new(Width, Height);

        /// <summary>
        /// A <see cref="Point2"/> located in the center of this <see cref="AARectangle"/>.
        /// </summary>
        [JsonIgnore]
        public Point2 Center => new((Left + Right) / 2, (Top + Bottom) / 2);

        [JsonIgnore]
        public float Area => Width * Height;

        [JsonIgnore]
        public float Perimeter => (Width + Height) * 2f;

        /// <summary>
        /// Creates a 1x1 rectangle with its top-left corner at the origin.
        /// </summary>
        public AARectangle() : this(0f, 0f, 1f, 1f) { }

        /// <summary>
        /// Creates a rectangle of the given size with its top-left corner at the origin.
        /// </summary>
        public AARectangle(float width, float height) : this(0f, 0f, width, height) { }

        /// <summary>
        /// Creates a rectangle from its top-left corner and size.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="width"/> or <paramref name="height"/> is zero or negative.
        /// </summary>
        public AARectangle(float left, float top, float width, float height)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0f);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 0f);

            Left = left;
            Top = top;
            Right = left + width;
            Bottom = top + height;
        }

        /// <summary>
        /// Creates a rectangle of the given size centered on <paramref name="center"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="width"/> or <paramref name="height"/> is zero or negative.
        /// </summary>
        public AARectangle(Point2 center, float width, float height)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0f);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 0f);

            Left = center.X - width / 2;
            Top = center.Y - height / 2;
            Right = center.X + width / 2;
            Bottom = center.Y + height / 2;
        }

        /// <summary>
        /// Returns a copy of this <see cref="AARectangle"/> scaled uniformly about its center by <paramref name="scale"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public AARectangle Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            var center = Center;
            float halfWidth = Width * scale / 2f;
            float halfHeight = Height * scale / 2f;

            return new AARectangle
            {
                Left = center.X - halfWidth,
                Top = center.Y - halfHeight,
                Right = center.X + halfWidth,
                Bottom = center.Y + halfHeight,
            };
        }

        /// <summary>
        /// Returns a copy of this <see cref="AARectangle"/> moved so that its <see cref="Center"/> is at <paramref name="p"/>.
        /// Width and height are unchanged.
        /// </summary>
        public AARectangle MoveTo(Point2 p)
        {
            float halfWidth = Width / 2f;
            float halfHeight = Height / 2f;

            return new AARectangle
            {
                Left = p.X - halfWidth,
                Top = p.Y - halfHeight,
                Right = p.X + halfWidth,
                Bottom = p.Y + halfHeight,
            };
        }

        /// <summary>
        /// Creates a new <see cref="AARectangle"/> that completely contains two r rectangles.
        /// </summary>
        public static AARectangle Union(AARectangle r1, AARectangle r2)
        {
            return new AARectangle()
            {
                Left = MathF.Min(r1.Left, r2.Left),
                Top = MathF.Min(r1.Top, r2.Top),
                Right = MathF.Max(r1.Right, r2.Right),
                Bottom = MathF.Max(r1.Bottom, r2.Bottom),
            };
        }

        /// <summary>
        /// Creates a new <see cref="AARectangle"/> that is shifted by a vector.
        /// </summary>
        /// <param name="r">The rectangle to shift.</param>
        /// <param name="v">The translation to apply.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AARectangle operator +(AARectangle r, Vector2 v)
        {
            return new AARectangle()
            {
                Left = r.X + v.X,
                Top = r.Y + v.Y,
                Right = r.Right + v.X,
                Bottom = r.Bottom + v.Y,
            };
        }

        /// <summary>
        /// Creates a new <see cref="AARectangle"/> that is shifted by the negation of a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AARectangle operator -(AARectangle r, Vector2 v)
        {
            return new AARectangle()
            {
                Left = r.X - v.X,
                Top = r.Y - v.Y,
                Right = r.Right - v.X,
                Bottom = r.Bottom - v.Y,
            };
        }

        /// <summary>
        /// Returns a copy of the rectangle scaled uniformly about its center (width and height multiplied by <paramref name="scale"/>).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AARectangle operator *(AARectangle r, float scale) => r.Scale(scale);

        /// <summary>
        /// Returns a copy of the rectangle scaled uniformly about its center by 1/<paramref name="scale"/>.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AARectangle operator /(AARectangle r, float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return r * (1 / scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="AARectangle"/> with the same edges.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is AARectangle other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other rectangle has the same left, top, right and bottom edges (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AARectangle r)
        {
            return Left == r.Left && Top == r.Top && Right == r.Right && Bottom == r.Bottom;
        }

        /// <summary>
        /// Returns true if both rectangles have the same edges.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(AARectangle a, AARectangle b) => a.Equals(b);

        /// <summary>
        /// Returns true if the rectangles differ in any edge.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(AARectangle a, AARectangle b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the four edge coordinates.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right, Top, Bottom);
        }

        /// <summary>
        /// Returns a string of the form "AARectangle(Left: l, Top: t, Width: w, Height: h)".
        /// </summary>
        public override string ToString()
        {
            return $"AARectangle(Left: {Left}, Top: {Top}, Width: {Width}, Height: {Height})";
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);

        /// <summary>
        /// Returns true if (<paramref name="x"/>, <paramref name="y"/>) lies inside or on the edges of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(float x, float y) => Contains(new Point2(x, y));

        /// <summary>
        /// Returns true if <paramref name="value"/> lies entirely inside or on the edges of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(AARectangle value) => Collisions2d.Contains(this, value);

        /// <summary>
        /// Gets whether or not a specified <see cref="AARectangle"/> intersects with this <see cref="AARectangle"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AARectangle r) => Collisions2d.Intersects(this, r);

        /// <summary>
        /// Gets whether or not a specified <see cref="Circle"/> intersects with this <see cref="AARectangle"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Circle c) => Collisions2d.Intersects(c, this);
    }
}
