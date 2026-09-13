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
    /// <summary>
    /// An immutable N-dimensional vector backed by an owned <see cref="float"/> array. Every constructor
    /// copies its input, so once built, a <see cref="VectorN"/> can never be mutated through any public
    /// member (<see cref="Axis"/> only ever exposes a read-only view) - matching the value semantics every
    /// other shape in this library has. A vector always has at least one axis; there is no zero-dimensional
    /// <see cref="VectorN"/>, so construction with no components is rejected rather than silently allowed.
    /// </summary>
    public readonly struct VectorN : IEquatable<VectorN>
    {
        private const int MINIMUM_NUMBER_OF_AXIS = 1;

        private readonly float[] _Axis;

        /// <summary>
        /// A read-only view of the vector's components, in order. Reapplies the same guards as
        /// <see cref="VectorN(float[])"/> - throwing <see cref="ArgumentNullException"/> or
        /// <see cref="ArgumentOutOfRangeException"/> - if this <see cref="VectorN"/> was never constructed
        /// through it (e.g. <c>default(VectorN)</c>). The array-to-span conversion below is normally
        /// null-tolerant and would otherwise silently hand back an empty span instead of surfacing the problem.
        /// </summary>
        public ReadOnlySpan<float> Axis
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_Axis);
                ArgumentOutOfRangeException.ThrowIfLessThan(_Axis.Length, MINIMUM_NUMBER_OF_AXIS);

                return _Axis;
            }
        }

        /// <summary>
        /// Creates a vector from the given components, in order. <paramref name="values"/> is copied into a
        /// private array, so mutating the caller's array afterward does not affect this vector. Throws
        /// <see cref="ArgumentNullException"/> if <paramref name="values"/> is null, or
        /// <see cref="ArgumentOutOfRangeException"/> if it is empty.
        /// </summary>
        public VectorN(float[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, MINIMUM_NUMBER_OF_AXIS);

            _Axis = [.. values];
        }

        /// <summary>
        /// Returns the zero vector - the origin - for the given number of axes. Throws
        /// <see cref="ArgumentOutOfRangeException"/> if <paramref name="length"/> is less than 1.
        /// </summary>
        public static VectorN Zero(int length)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(length, 1);

            return new(new float[length]);
        }

        /// <summary>
        /// Adds two vectors component-wise. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        public static VectorN operator +(VectorN v1, VectorN v2)
        {
            if (v1.Axis.Length != v2.Axis.Length)
                throw new InvalidOperationException($"cannot add vectors of unequal orders");

            var result = new float[v1.Axis.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = v1.Axis[i] + v2.Axis[i];

            return new VectorN(result);
        }

        /// <summary>
        /// Subtracts <paramref name="v2"/> from <paramref name="v1"/> component-wise. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        public static VectorN operator -(VectorN v1, VectorN v2)
        {
            if (v1.Axis.Length != v2.Axis.Length)
                throw new InvalidOperationException($"cannot add vectors of unequal orders");

            var result = new float[v1.Axis.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = v1.Axis[i] - v2.Axis[i];

            return new VectorN(result);
        }

        /// <summary>
        /// Multiplies each component of the vector by a scalar.
        /// </summary>
        public static VectorN operator *(VectorN v, float scalar)
        {
            var result = new float[v.Axis.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = v.Axis[i] * scalar;

            return new VectorN(result);
        }

        /// <summary>
        /// Divides each component of the vector by a scalar.
        /// </summary>
        public static VectorN operator /(VectorN v, float scalar)
        {
            var result = new float[v.Axis.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = v.Axis[i] / scalar;

            return new VectorN(result);
        }

        /// <summary>
        /// Returns the dot product of two vectors. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        public static float Dot(VectorN v1, VectorN v2)
        {
            if (v1.Axis.Length != v2.Axis.Length)
                throw new InvalidOperationException($"cannot take the dot product of vectors of unequal orders");

            float sum = 0f;

            for (int i = 0; i < v1.Axis.Length; i++)
                sum += v1.Axis[i] * v2.Axis[i];

            return sum;
        }

        /// <summary>
        /// Returns the dot product of this vector with <paramref name="v"/>. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(VectorN v)
        {
            return Dot(this, v);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is a <see cref="VectorN"/> of the same dimension and equal components.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is VectorN other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other vector has the same dimension and exactly equal components (no tolerance).
        /// Uses <c>!=</c>, not <see cref="float.Equals(float)"/>, so (unlike a naive span comparison) NaN components
        /// never compare equal - consistent with every other shape's float comparisons in this library.
        /// </summary>
        public bool Equals(VectorN v)
        {
            if (Axis.Length != v.Axis.Length)
                return false;

            for (int i = 0; i < Axis.Length; i++)
            {
                if (Axis[i] != v.Axis[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code derived from every component, so vectors that compare equal hash equal.
        /// </summary>
        public override int GetHashCode()
        {
            var hash = new HashCode();

            foreach (float component in Axis)
                hash.Add(component);

            return hash.ToHashCode();
        }

        /// <summary>
        /// Returns true if both vectors have the same dimension and equal components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(VectorN a, VectorN b) => a.Equals(b);

        /// <summary>
        /// Returns true if the vectors' dimensions or components differ.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(VectorN a, VectorN b) => !a.Equals(b);

        /// <summary>
        /// Returns a string of the form "VectorN[a, b, c]". Throws via <see cref="Axis"/> if this
        /// <see cref="VectorN"/> was never properly constructed.
        /// </summary>
        public override string ToString()
        {
            return $"VectorN[{string.Join(", ", Axis.ToArray())}]";
        }
    }
}
