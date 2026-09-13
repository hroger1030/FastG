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
    public readonly struct Vector2 : I1d, IEquatable<Vector2>
    {
        /// <summary>
        /// A vector with both components set to zero.
        /// </summary>
        public static readonly Vector2 ZERO = new(0, 0);

        /// <summary>
        /// A vector with both components set to one.
        /// </summary>
        public static readonly Vector2 ONE = new(1, 1);

        public float X { get; init; }

        public float Y { get; init; }

        /// <summary>
        /// Creates a zero vector (0, 0).
        /// </summary>
        public Vector2() : this(0, 0) { }

        /// <summary>
        /// Creates a vector from the X and Y components of a <see cref="Point2"/>.
        /// </summary>
        public Vector2(Point2 p1) : this(p1.X, p1.Y) { }

        /// <summary>
        /// Creates a unit vector pointing along the given rotation, in radians (counter-clockwise from the +X axis).
        /// </summary>
        public Vector2(float rotation) : this(MathF.Cos(rotation), MathF.Sin(rotation)) { }

        /// <summary>
        /// Creates a vector from explicit X and Y components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds two vectors component-wise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Subtracts <paramref name="v2"/> from <paramref name="v1"/> component-wise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Negates each component of the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        /// <summary>
        /// Multiplies each component of the vector by a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 v, float scale)
        {
            return new Vector2(v.X * scale, v.Y * scale);
        }

        /// <summary>
        /// Divides each component of the vector by a scalar. Throws <see cref="DivideByZeroException"/> if <paramref name="scale"/> is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 v, float scale)
        {
            if (scale == 0f) throw new DivideByZeroException(nameof(scale));

            return new Vector2(v.X / scale, v.Y / scale);
        }

        /// <summary>
        /// Returns a unit-length copy of <paramref name="v"/>. Throws if the vector's magnitude is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normalize(Vector2 v)
        {
            return v.Normalize();
        }

        /// <summary>
        /// Returns a unit-length copy of this vector. Throws if the vector's magnitude is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Normalize()
        {
            float length = Length;

            if (length == 0)
                throw new DivideByZeroException("Cannot normalize a vector when it's magnitude is zero");

            float inverse = 1f / length;

            return new Vector2(X * inverse, Y * inverse);
        }

        /// <summary>
        /// Returns the rotation of this vector in radians, measured counter-clockwise from the +X axis (range -PI to PI).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float VectorToRotation()
        {
            return MathF.Atan2(Y, X);
        }

        /// <summary>
        /// Returns the 2D cross product (perp-dot) of two vectors.
        /// A 2D cross product has no perpendicular axis to return a vector along, so the result is the
        /// scalar Z component that a 3D cross product would produce; sign indicates winding/orientation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        /// <summary>
        /// Returns the 2D cross product (perp-dot) of this vector with <paramref name="v"/>.
        /// See <see cref="Cross(Vector2, Vector2)"/> for why the result is a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Cross(Vector2 v)
        {
            return Cross(this, v);
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        /// <summary>
        /// Returns the dot product of this vector with <paramref name="v"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector2 v)
        {
            return Dot(this, v);
        }

        /// <summary>
        /// The squared magnitude of this vector. Cheaper than <see cref="Length"/> (no square root);
        /// prefer it when comparing magnitudes or testing against a squared threshold.
        /// </summary>
        public float LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (X * X) + (Y * Y);
        }

        /// <summary>
        /// The magnitude (Euclidean length) of this vector.
        /// </summary>
        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MathF.Sqrt(LengthSquared);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="Vector2"/> with the same components.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Vector2 other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other vector has exactly equal X and Y components (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2 v)
        {
            return X == v.X && Y == v.Y;
        }

        /// <summary>
        /// Returns true if both vectors have exactly equal components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);

        /// <summary>
        /// Returns true if the vectors differ in either component.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the X and Y components.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Returns a string of the form "&ltx, y&gt;".
        /// </summary>
        public override string ToString()
        {
            return $"<{X}, {Y}>";
        }
    }
}