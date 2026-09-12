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
    public class VectorN : IEquatable<VectorN>
    {
        public float[] Axis { get; set; } = Array.Empty<float>();

        /// <summary>
        /// Creates a zero-filled vector with the given number of axes (dimensions).
        /// </summary>
        public VectorN(int length)
        {
            Axis = new float[length];

            for (int i = 0; i < length; i++)
                Axis[i] = 0f;
        }

        /// <summary>
        /// Creates a deep copy of an existing vector (its axis array is cloned).
        /// </summary>
        public VectorN(VectorN v)
        {
            Axis = new float[v.Axis.Length];

            for (int i = 0; i < v.Axis.Length; i++)
                Axis[i] = v.Axis[i];
        }

        /// <summary>
        /// Adds two vectors component-wise. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        public static VectorN operator +(VectorN v1, VectorN v2)
        {
            if (v1.Axis.Length != v2.Axis.Length)
                throw new InvalidOperationException($"cannot add vectors of unequal orders");

            var output = new VectorN(v1.Axis.Length);

            for (int i = 0; i < v1.Axis.Length; i++)
                output.Axis[i] = v1.Axis[i] + v2.Axis[i];

            return output;
        }

        /// <summary>
        /// Subtracts <paramref name="v2"/> from <paramref name="v1"/> component-wise. Throws <see cref="InvalidOperationException"/> if the vectors have different dimensions.
        /// </summary>
        public static VectorN operator -(VectorN v1, VectorN v2)
        {
            if (v1.Axis.Length != v2.Axis.Length)
                throw new InvalidOperationException($"cannot add vectors of unequal orders");

            var output = new VectorN(v1.Axis.Length);

            for (int i = 0; i < v1.Axis.Length; i++)
                output.Axis[i] = v1.Axis[i] - v2.Axis[i];

            return output;
        }

        /// <summary>
        /// Multiplies each component of the vector by a scalar.
        /// </summary>
        public static VectorN operator *(VectorN v, float scalar)
        {
            var output = new VectorN(v.Axis.Length);

            for (int i = 0; i < v.Axis.Length; i++)
                output.Axis[i] = v.Axis[i] * scalar;

            return output;
        }

        /// <summary>
        /// Divides each component of the vector by a scalar.
        /// </summary>
        public static VectorN operator /(VectorN v, float scalar)
        {
            var output = new VectorN(v.Axis.Length);

            for (int i = 0; i < v.Axis.Length; i++)
                output.Axis[i] = v.Axis[i] / scalar;

            return output;
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
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;

            var newObj = (VectorN)obj;
            return Equals(newObj);
        }

        /// <summary>
        /// Returns true if the other vector has the same dimension and exactly equal components (no tolerance).
        /// </summary>
        public bool Equals(VectorN v)
        {
            if (v is null)
                return false;

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
        /// Returns true if both vectors are null, or have the same dimension and equal components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(VectorN a, VectorN b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        /// <summary>
        /// Returns true if exactly one vector is null, or their dimensions or components differ.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(VectorN a, VectorN b) => !(a == b);

        /// <summary>
        /// Returns a string of the form "VectorN[a, b, c]".
        /// </summary>
        public override string ToString()
        {
            return $"VectorN[{string.Join(", ", Axis)}]";
        }
    }
}
