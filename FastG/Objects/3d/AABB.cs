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
    public readonly struct AABB : I3d, IEquatable<AABB>
    {
        /// <summary>
        /// A 1x1x1 AABB with one corner at the origin and the opposite corner at (1, 1, 1).
        /// </summary>
        public static readonly AABB UNIT_AABB = new(new Point3(0f, 0f, 0f), new Point3(1f, 1f, 1f));

        public Point3 Min { get; init; }

        public Point3 Max { get; init; }

        /// <summary>
        /// The volume of the box (width * height * depth). The constructor guarantees Max >= Min on every axis.
        /// </summary>
        [JsonIgnore]
        public float Volume => (Max.X - Min.X) * (Max.Y - Min.Y) * (Max.Z - Min.Z);

        /// <summary>
        /// The total surface area of the box's six faces.
        /// </summary>
        [JsonIgnore]
        public float SurfaceArea
        {
            get
            {
                float width = Max.X - Min.X;
                float height = Max.Y - Min.Y;
                float depth = Max.Z - Min.Z;

                return 2f * (width * height + height * depth + depth * width);
            }
        }

        /// <summary>
        /// Creates an axis-aligned bounding box from its minimum and maximum corners.
        /// Throws <see cref="ArgumentException"/> if any component of <paramref name="max"/> is less than the matching component of <paramref name="min"/>.
        /// </summary>
        public AABB(Point3 min, Point3 max)
        {
            if (max.X < min.X || max.Y < min.Y || max.Z < min.Z)
                throw new ArgumentException("Max must be greater than or equal to Min.");

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates an axis-aligned bounding box from the raw coordinates of its minimum and maximum corners.
        /// Throws <see cref="ArgumentException"/> if any max component is less than the matching min component.
        /// </summary>
        public AABB(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
            : this(new Point3(minX, minY, minZ), new Point3(maxX, maxY, maxZ)) { }

        /// <summary>
        /// Returns a copy of this box scaled uniformly about its center - a scale of 2 doubles the width,
        /// height and depth while keeping the center fixed. Throws <see cref="ArgumentOutOfRangeException"/>
        /// if <paramref name="scale"/> is zero or negative.
        /// </summary>
        public AABB Scale(float scale)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(scale, 0f);

            float centerX = (Min.X + Max.X) / 2f;
            float centerY = (Min.Y + Max.Y) / 2f;
            float centerZ = (Min.Z + Max.Z) / 2f;

            float halfWidth = (Max.X - Min.X) * scale / 2f;
            float halfHeight = (Max.Y - Min.Y) * scale / 2f;
            float halfDepth = (Max.Z - Min.Z) * scale / 2f;

            return new AABB(
                new Point3(centerX - halfWidth, centerY - halfHeight, centerZ - halfDepth),
                new Point3(centerX + halfWidth, centerY + halfHeight, centerZ + halfDepth));
        }

        /// <summary>
        /// Returns true if <paramref name="obj"/> is an <see cref="AABB"/> with the same corners.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is AABB other && Equals(other);
        }

        /// <summary>
        /// Returns true if the other box has the same min and max corners (no tolerance).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AABB other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        /// <summary>
        /// Returns true if both boxes have the same corners.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(AABB a, AABB b) => a.Equals(b);

        /// <summary>
        /// Returns true if the boxes differ in either corner.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(AABB a, AABB b) => !a.Equals(b);

        /// <summary>
        /// Returns a hash code derived from the min and max corners.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        /// <summary>
        /// Returns a string of the form "AABB(Min: (x, y, z), Max: (x, y, z))".
        /// </summary>
        public override string ToString()
        {
            return $"AABB(Min: {Min}, Max: {Max})";
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the faces of this box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point3 point) => Collisions3d.Contains(this, point);

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="other"/> on all three axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB other) => Collisions3d.Intersects(this, other);

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="cube"/> on all three axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cube cube) => Collisions3d.Intersects(this, cube);

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="sphere"/>, using the closest-point-on-box test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Sphere sphere) => Collisions3d.Intersects(this, sphere);

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="capsule"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Capsule capsule) => Collisions3d.Intersects(this, capsule);

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="cylinder"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Cylinder cylinder) => Collisions3d.Intersects(this, cylinder);
    }
}
