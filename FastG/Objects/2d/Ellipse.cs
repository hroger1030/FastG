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
    /// <summary>
    /// This is an axis aligned Ellipse object. For a general ellipse, we need a rotation element.
    /// </summary>
    public readonly struct Ellipse : I2d, IEquatable<Ellipse>
    {
        // TODO: Extend this to a general ellipse object.

        /// <summary>
        /// An ellipse X radius 1, Y radius 2 centered at the origin.
        /// Math doesn't often make use of a 'unit ellipse', but it is worth defining.
        /// </summary>
        public static readonly Ellipse REFERENCE_ELLIPSE = new(Point2.ZERO, 1f, 2f);

        public Point2 Center { get; init; }

        public float RadiusX { get; init; }

        public float RadiusY { get; init; }

        [JsonIgnore]
        public float Area => MathF.PI * RadiusX * RadiusY;

        /// <summary>
        /// The approximate circumference of the ellipse, using Ramanujan's second approximation.
        /// </summary>
        [JsonIgnore]
        public float Perimeter
        {
            get
            {
                float a = MathF.Max(RadiusX, RadiusY);
                float b = MathF.Min(RadiusX, RadiusY);
                float sum = a + b;
                float difference = a - b;
                float h = (difference * difference) / (sum * sum);

                return MathF.PI * (a + b) * (1 + (3f * h) / (10f + MathF.Sqrt(4f - 3f * h)));
            }
        }

        /// <summary>
        /// Creates an axis-aligned ellipse centered at <paramref name="center"/> with the given semi-axis lengths.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if either radius is zero or negative.
        /// </summary>
        public Ellipse(Point2 center, float radiusX, float radiusY)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(radiusX, 0f);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(radiusY, 0f);

            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        /// <summary>
        /// Creates an axis-aligned ellipse centered at (<paramref name="centerX"/>, <paramref name="centerY"/>) with the given semi-axis lengths.
        /// Throws <see cref="ArgumentOutOfRangeException"/> if either radius is zero or negative.
        /// </summary>
        public Ellipse(float centerX, float centerY, float radiusX, float radiusY)
            : this(new Point2(centerX, centerY), radiusX, radiusY) { }

        /// <summary>
        /// Returns a copy of the ellipse with both radii multiplied by <paramref name="scale"/> (center unchanged).
        /// Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public Ellipse Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            return new Ellipse(Center, RadiusX * scale, RadiusY * scale);
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is an <see cref="Ellipse"/> with the same center and semi-axes.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Ellipse other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other ellipse has the same center and semi-axis lengths (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ellipse other)
        {
            return Center.Equals(other.Center) && RadiusX.Equals(other.RadiusX) && RadiusY.Equals(other.RadiusY);
        }

        /// <summary>
        /// Returns true if both ellipses have the same center and semi-axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse a, Ellipse b) => a.Equals(b);

        /// <summary>
        /// Returns true if the ellipses differ in center or either semi-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse a, Ellipse b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the center and both semi-axis lengths.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Center, RadiusX, RadiusY);
        }

        /// <summary>
        /// Returns a string of the form "Ellipse(Center: (x, y), RadiusX: rx, RadiusY: ry)".
        /// </summary>
        public override string ToString()
        {
            return $"Ellipse(Center: {Center}, RadiusX: {RadiusX}, RadiusY: {RadiusY})";
        }

        // The method below is a thin redirect into Collisions2d, kept here only for call-site convenience
        // (e.g. ellipse.Contains(point) instead of Collisions2d.Contains(ellipse, point)). The real logic lives there.

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on this ellipse.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point2 point) => Collisions2d.Contains(this, point);
    }
}
