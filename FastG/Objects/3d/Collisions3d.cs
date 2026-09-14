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

namespace FastG
{
    /// <summary>
    /// Pairwise intersection and containment tests between 3D shapes (including ray casts). Each shape pair
    /// has exactly one real implementation here; the matching instance methods on the shape types themselves
    /// (e.g. <see cref="Sphere.Intersects(Cube)"/>) are thin forwarders kept for call-site convenience.
    /// </summary>
    public static class Collisions3d
    {
        /// <summary>
        /// Returns true if this sphere overlaps or touches <paramref name="b"/> (distance between centers &lt;= sum of radii).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Sphere a, Sphere b)
        {
            float dx = b.Center.X - a.Center.X;
            float dy = b.Center.Y - a.Center.Y;
            float dz = b.Center.Z - a.Center.Z;
            float radiusSum = a.Radius + b.Radius;

            return (dx * dx + dy * dy + dz * dz) <= (radiusSum * radiusSum);
        }

        /// <summary>
        /// Gets whether or not <paramref name="sphere"/> intersects with <paramref name="cube"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Cube cube, Sphere sphere)
        {
            float closestX = Math.Clamp(sphere.Center.X, cube.X1, cube.X2);
            float closestY = Math.Clamp(sphere.Center.Y, cube.Y1, cube.Y2);
            float closestZ = Math.Clamp(sphere.Center.Z, cube.Z1, cube.Z2);

            float distanceX = sphere.Center.X - closestX;
            float distanceY = sphere.Center.Y - closestY;
            float distanceZ = sphere.Center.Z - closestZ;

            return (distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ) <= (sphere.Radius * sphere.Radius);
        }

        /// <summary>
        /// Returns true if this cube overlaps or touches <paramref name="b"/> on all three axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Cube a, Cube b)
        {
            return a.X1 <= b.X2 && a.X2 >= b.X1 &&
                   a.Y1 <= b.Y2 && a.Y2 >= b.Y1 &&
                   a.Z1 <= b.Z2 && a.Z2 >= b.Z1;
        }

        /// <summary>
        /// Returns true if this box overlaps or touches <paramref name="b"/> on all three axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(AABB a, AABB b)
        {
            return a.Min.X <= b.Max.X && a.Max.X >= b.Min.X &&
                   a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y &&
                   a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z;
        }

        /// <summary>
        /// Returns true if <paramref name="aabb"/> overlaps or touches <paramref name="cube"/> on all three axes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(AABB aabb, Cube cube)
        {
            return aabb.Min.X <= cube.X2 && aabb.Max.X >= cube.X1 &&
                   aabb.Min.Y <= cube.Y2 && aabb.Max.Y >= cube.Y1 &&
                   aabb.Min.Z <= cube.Z2 && aabb.Max.Z >= cube.Z1;
        }

        /// <summary>
        /// Returns true if <paramref name="aabb"/> overlaps or touches <paramref name="sphere"/>, using the
        /// closest-point-on-box test.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(AABB aabb, Sphere sphere)
        {
            float closestX = Math.Clamp(sphere.Center.X, aabb.Min.X, aabb.Max.X);
            float closestY = Math.Clamp(sphere.Center.Y, aabb.Min.Y, aabb.Max.Y);
            float closestZ = Math.Clamp(sphere.Center.Z, aabb.Min.Z, aabb.Max.Z);

            float distanceX = sphere.Center.X - closestX;
            float distanceY = sphere.Center.Y - closestY;
            float distanceZ = sphere.Center.Z - closestZ;

            return (distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ) <= (sphere.Radius * sphere.Radius);
        }

        /// <summary>
        /// Returns true if <paramref name="plane"/> intersects <paramref name="sphere"/> (the distance from the
        /// sphere's center to the plane is no greater than its radius).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Plane3 plane, Sphere sphere)
        {
            return MathF.Abs(plane.DistanceTo(sphere.Center)) <= sphere.Radius;
        }

        /// <summary>
        /// Returns true if <paramref name="capsule"/> overlaps or touches <paramref name="sphere"/>, tested via the
        /// closest point on the capsule's core segment to the sphere center. <see cref="Capsule"/>'s constructor
        /// rejects equal endpoints, so the zero-length check below only guards a <c>default(Capsule)</c>.
        /// </summary>
        public static bool Intersects(Capsule capsule, Sphere sphere)
        {
            var ab = new Vector3(capsule.PointA, capsule.PointB);

            if (ab.LengthSquared == 0f)
                return Intersects(new Sphere(capsule.PointA, capsule.Radius), sphere);

            var ac = new Vector3(capsule.PointA, sphere.Center);
            float t = (ab.X * ac.X + ab.Y * ac.Y + ab.Z * ac.Z) / (ab.X * ab.X + ab.Y * ab.Y + ab.Z * ab.Z);
            t = Math.Clamp(t, 0f, 1f);

            var closest = new Point3(
                capsule.PointA.X + ab.X * t,
                capsule.PointA.Y + ab.Y * t,
                capsule.PointA.Z + ab.Z * t);

            return Intersects(new Sphere(closest, capsule.Radius), sphere);
        }

        /// <summary>
        /// Returns true if <paramref name="aabb"/> overlaps or touches <paramref name="capsule"/>. Decomposes the
        /// capsule into two exact end-cap spheres plus the tightest axis-aligned box that can bound the straight
        /// cylindrical body between them (padding each axis by <c>Radius * sqrt(1 - direction² )</c> on that axis,
        /// not a flat <c>Radius</c> — exact for an axis-aligned capsule, and still a true superset of the capsule
        /// at any other orientation). The union of the three is therefore never smaller than the real capsule, so
        /// this can't produce a false negative; the only inaccuracy is a possible false positive in the thin sliver
        /// between the box's square corners and the cylinder's round cross-section for a diagonal capsule.
        /// <see cref="Capsule"/>'s constructor rejects equal endpoints, so the zero-length check below only
        /// guards a <c>default(Capsule)</c>.
        /// </summary>
        public static bool Intersects(AABB aabb, Capsule capsule)
        {
            var axis = new Vector3(capsule.PointA, capsule.PointB);
            float axisLength = axis.Length;

            if (axisLength == 0f)
                return Intersects(aabb, new Sphere(capsule.PointA, capsule.Radius));

            float dirX = axis.X / axisLength;
            float dirY = axis.Y / axisLength;
            float dirZ = axis.Z / axisLength;

            float padX = capsule.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirX * dirX)));
            float padY = capsule.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirY * dirY)));
            float padZ = capsule.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirZ * dirZ)));

            var cylinderBounds = new AABB(
                new Point3(
                    MathF.Min(capsule.PointA.X, capsule.PointB.X) - padX,
                    MathF.Min(capsule.PointA.Y, capsule.PointB.Y) - padY,
                    MathF.Min(capsule.PointA.Z, capsule.PointB.Z) - padZ),
                new Point3(
                    MathF.Max(capsule.PointA.X, capsule.PointB.X) + padX,
                    MathF.Max(capsule.PointA.Y, capsule.PointB.Y) + padY,
                    MathF.Max(capsule.PointA.Z, capsule.PointB.Z) + padZ));

            return Intersects(aabb, cylinderBounds)
                || Intersects(aabb, new Sphere(capsule.PointA, capsule.Radius))
                || Intersects(aabb, new Sphere(capsule.PointB, capsule.Radius));
        }

        /// <summary>
        /// Returns true if <paramref name="capsule"/> overlaps or touches <paramref name="cube"/>. Treats
        /// <paramref name="cube"/> as an axis-aligned box and delegates to <see cref="Intersects(AABB, Capsule)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Capsule capsule, Cube cube)
        {
            return Intersects(new AABB(new Point3(cube.X1, cube.Y1, cube.Z1), new Point3(cube.X2, cube.Y2, cube.Z2)), capsule);
        }

        /// <summary>
        /// Returns true if two capsules overlap or touch: the shortest distance between their core segments is no
        /// greater than the sum of their radii. Uses the standard closest-point-between-two-segments algorithm
        /// (Ericson, "Real-Time Collision Detection", section 5.1.9).
        /// </summary>
        public static bool Intersects(Capsule a, Capsule b)
        {
            var d1 = new Vector3(a.PointA, a.PointB);
            var d2 = new Vector3(b.PointA, b.PointB);
            var r = new Vector3(b.PointA, a.PointA);

            float lengthSquared1 = Vector3.Dot(d1, d1);
            float lengthSquared2 = Vector3.Dot(d2, d2);
            float f = Vector3.Dot(d2, r);

            float s, t;

            if (lengthSquared1 <= Constants.FLOAT_ERROR_MARGIN && lengthSquared2 <= Constants.FLOAT_ERROR_MARGIN)
            {
                s = 0f;
                t = 0f;
            }
            else if (lengthSquared1 <= Constants.FLOAT_ERROR_MARGIN)
            {
                s = 0f;
                t = Math.Clamp(f / lengthSquared2, 0f, 1f);
            }
            else
            {
                float c = Vector3.Dot(d1, r);

                if (lengthSquared2 <= Constants.FLOAT_ERROR_MARGIN)
                {
                    t = 0f;
                    s = Math.Clamp(-c / lengthSquared1, 0f, 1f);
                }
                else
                {
                    float dotD1D2 = Vector3.Dot(d1, d2);
                    float denom = (lengthSquared1 * lengthSquared2) - (dotD1D2 * dotD1D2);

                    s = denom != 0f ? Math.Clamp(((dotD1D2 * f) - (c * lengthSquared2)) / denom, 0f, 1f) : 0f;
                    t = ((dotD1D2 * s) + f) / lengthSquared2;

                    if (t < 0f)
                    {
                        t = 0f;
                        s = Math.Clamp(-c / lengthSquared1, 0f, 1f);
                    }
                    else if (t > 1f)
                    {
                        t = 1f;
                        s = Math.Clamp((dotD1D2 - c) / lengthSquared1, 0f, 1f);
                    }
                }
            }

            var closestA = new Point3(a.PointA.X + (d1.X * s), a.PointA.Y + (d1.Y * s), a.PointA.Z + (d1.Z * s));
            var closestB = new Point3(b.PointA.X + (d2.X * t), b.PointA.Y + (d2.Y * t), b.PointA.Z + (d2.Z * t));

            float radiusSum = a.Radius + b.Radius;
            var closestDelta = new Vector3(closestA, closestB);
            return closestDelta.LengthSquared <= (radiusSum * radiusSum);
        }

        /// <summary>
        /// Returns true if <paramref name="cylinder"/> overlaps or touches <paramref name="sphere"/>, tested via the
        /// exact closest point on the solid cylinder - including its flat end caps, not just the core axis - to the
        /// sphere center.
        /// </summary>
        public static bool Intersects(Cylinder cylinder, Sphere sphere)
        {
            var axis = new Vector3(cylinder.PointA, cylinder.PointB);
            var toCenter = new Vector3(cylinder.PointA, sphere.Center);

            float t = Vector3.Dot(axis, toCenter) / axis.LengthSquared;
            var perpendicular = toCenter - (axis * t);
            float radialDistance = perpendicular.Length;

            if (t >= 0f && t <= 1f)
            {
                float lateralGap = MathF.Max(0f, radialDistance - cylinder.Radius);
                return lateralGap <= sphere.Radius;
            }

            // beyond one of the flat end caps: combine the axial overshoot past the cap plane with any
            // remaining radial excess beyond the cap's disk - the two are perpendicular to each other.
            float tClamped = Math.Clamp(t, 0f, 1f);
            float axialOvershoot = axis.Length * MathF.Abs(t - tClamped);
            float radialExcess = MathF.Max(0f, radialDistance - cylinder.Radius);
            float distance = MathF.Sqrt((axialOvershoot * axialOvershoot) + (radialExcess * radialExcess));

            return distance <= sphere.Radius;
        }

        /// <summary>
        /// Returns true if <paramref name="aabb"/> overlaps or touches <paramref name="cylinder"/>. Computes the
        /// exact tight AABB of the solid cylinder - unlike <see cref="Capsule"/>, flat end caps never bulge past
        /// the lateral silhouette, so (unlike <see cref="Intersects(AABB, Capsule)"/>) no extra end-sphere padding
        /// is needed - and tests box against box. That's still a safe superset of the true intersection, not an
        /// exact test: a tilted cylinder can occupy less of its own bounding box than a corner of it suggests.
        /// </summary>
        public static bool Intersects(AABB aabb, Cylinder cylinder)
        {
            var axis = new Vector3(cylinder.PointA, cylinder.PointB);
            float axisLength = axis.Length;

            float dirX = axis.X / axisLength;
            float dirY = axis.Y / axisLength;
            float dirZ = axis.Z / axisLength;

            float padX = cylinder.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirX * dirX)));
            float padY = cylinder.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirY * dirY)));
            float padZ = cylinder.Radius * MathF.Sqrt(MathF.Max(0f, 1f - (dirZ * dirZ)));

            var cylinderBounds = new AABB(
                new Point3(
                    MathF.Min(cylinder.PointA.X, cylinder.PointB.X) - padX,
                    MathF.Min(cylinder.PointA.Y, cylinder.PointB.Y) - padY,
                    MathF.Min(cylinder.PointA.Z, cylinder.PointB.Z) - padZ),
                new Point3(
                    MathF.Max(cylinder.PointA.X, cylinder.PointB.X) + padX,
                    MathF.Max(cylinder.PointA.Y, cylinder.PointB.Y) + padY,
                    MathF.Max(cylinder.PointA.Z, cylinder.PointB.Z) + padZ));

            return Intersects(aabb, cylinderBounds);
        }

        /// <summary>
        /// Returns true if <paramref name="cylinder"/> overlaps or touches <paramref name="cube"/>. Treats
        /// <paramref name="cube"/> as an axis-aligned box and delegates to <see cref="Intersects(AABB, Cylinder)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Cube cube, Cylinder cylinder)
        {
            return Intersects(new AABB(new Point3(cube.X1, cube.Y1, cube.Z1), new Point3(cube.X2, cube.Y2, cube.Z2)), cylinder);
        }

        /// <summary>
        /// Returns true if two cylinders overlap or touch. This is a conservative approximation: it treats each
        /// cylinder as the <see cref="Capsule"/> that encloses it (same core segment and radius, rounded caps
        /// instead of flat ones) and tests those with the standard closest-point-between-two-segments algorithm.
        /// A capsule always contains the cylinder it's built from, so a "no" here is always correct; a "yes" can
        /// be a false positive when the true intersection (if any) would only have occurred in the rounded-off
        /// region just past one of the flat caps. Unlike <see cref="Intersects(Capsule, Capsule)"/>, there's no
        /// near-zero-length guard below: <see cref="Cylinder"/>'s constructor rejects equal endpoints, so a
        /// zero-length axis only happens via <c>default(Cylinder)</c>, which divides by zero into NaN and always
        /// compares false rather than throwing.
        /// </summary>
        public static bool Intersects(Cylinder a, Cylinder b)
        {
            var d1 = new Vector3(a.PointA, a.PointB);
            var d2 = new Vector3(b.PointA, b.PointB);
            var r = new Vector3(b.PointA, a.PointA);

            float lengthSquared1 = Vector3.Dot(d1, d1);
            float lengthSquared2 = Vector3.Dot(d2, d2);
            float f = Vector3.Dot(d2, r);

            float s, t;
            float c = Vector3.Dot(d1, r);
            float dotD1D2 = Vector3.Dot(d1, d2);
            float denom = (lengthSquared1 * lengthSquared2) - (dotD1D2 * dotD1D2);

            s = denom != 0f ? Math.Clamp(((dotD1D2 * f) - (c * lengthSquared2)) / denom, 0f, 1f) : 0f;
            t = ((dotD1D2 * s) + f) / lengthSquared2;

            if (t < 0f)
            {
                t = 0f;
                s = Math.Clamp(-c / lengthSquared1, 0f, 1f);
            }
            else if (t > 1f)
            {
                t = 1f;
                s = Math.Clamp((dotD1D2 - c) / lengthSquared1, 0f, 1f);
            }

            var closestA = new Point3(a.PointA.X + (d1.X * s), a.PointA.Y + (d1.Y * s), a.PointA.Z + (d1.Z * s));
            var closestB = new Point3(b.PointA.X + (d2.X * t), b.PointA.Y + (d2.Y * t), b.PointA.Z + (d2.Z * t));

            float radiusSum = a.Radius + b.Radius;
            var closestDelta = new Vector3(closestA, closestB);
            return closestDelta.LengthSquared <= (radiusSum * radiusSum);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="sphere"/>. On a hit, returns
        /// (true, distance) where distance is along the ray to the nearest non-negative intersection; otherwise
        /// returns (false, 0). An origin inside the sphere counts as a hit at the exit point.
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, Sphere sphere)
        {
            float dx = ray.Origin.X - sphere.Center.X;
            float dy = ray.Origin.Y - sphere.Center.Y;
            float dz = ray.Origin.Z - sphere.Center.Z;

            float b = dx * ray.Direction.X + dy * ray.Direction.Y + dz * ray.Direction.Z;
            float c = dx * dx + dy * dy + dz * dz - sphere.Radius * sphere.Radius;
            float discriminant = b * b - c;

            if (discriminant < 0f)
                return (false, 0f);

            float sqrt = MathF.Sqrt(discriminant);
            float t0 = -b - sqrt;
            float t1 = -b + sqrt;

            if (t0 >= 0f)
                return (true, t0);

            if (t1 >= 0f)
                return (true, t1);

            return (false, 0f);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="aabb"/> using the slab method. On a hit,
        /// returns (true, distance) where distance is the entry distance along the ray (clamped to 0 when the origin
        /// is inside); otherwise returns (false, 0).
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, AABB aabb)
        {
            float tMin = float.NegativeInfinity;
            float tMax = float.PositiveInfinity;

            if (MathF.Abs(ray.Direction.X) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.X < aabb.Min.X || ray.Origin.X > aabb.Max.X)
                    return (false, 0f);
            }
            else
            {
                float t1 = (aabb.Min.X - ray.Origin.X) / ray.Direction.X;
                float t2 = (aabb.Max.X - ray.Origin.X) / ray.Direction.X;

                if (t1 > t2)
                    (t2, t1) = (t1, t2);

                tMin = MathF.Max(tMin, t1);
                tMax = MathF.Min(tMax, t2);

                if (tMin > tMax)
                    return (false, 0f);
            }

            if (MathF.Abs(ray.Direction.Y) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.Y < aabb.Min.Y || ray.Origin.Y > aabb.Max.Y)
                    return (false, 0f);
            }
            else
            {
                float t1 = (aabb.Min.Y - ray.Origin.Y) / ray.Direction.Y;
                float t2 = (aabb.Max.Y - ray.Origin.Y) / ray.Direction.Y;

                if (t1 > t2)
                    (t2, t1) = (t1, t2);

                tMin = MathF.Max(tMin, t1);
                tMax = MathF.Min(tMax, t2);

                if (tMin > tMax)
                    return (false, 0f);
            }

            if (MathF.Abs(ray.Direction.Z) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.Z < aabb.Min.Z || ray.Origin.Z > aabb.Max.Z)
                    return (false, 0f);
            }
            else
            {
                float t1 = (aabb.Min.Z - ray.Origin.Z) / ray.Direction.Z;
                float t2 = (aabb.Max.Z - ray.Origin.Z) / ray.Direction.Z;

                if (t1 > t2)
                    (t2, t1) = (t1, t2);

                tMin = MathF.Max(tMin, t1);
                tMax = MathF.Min(tMax, t2);

                if (tMin > tMax)
                    return (false, 0f);
            }

            return (true, MathF.Max(0f, tMin));
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="cube"/> by treating it as an axis-aligned
        /// box. See <see cref="Intersects(Ray, AABB)"/> for the meaning of the returned distance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (bool Hit, float Distance) Intersects(Ray ray, Cube cube)
        {
            return Intersects(ray, new AABB(new Point3(cube.X1, cube.Y1, cube.Z1), new Point3(cube.X2, cube.Y2, cube.Z2)));
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="plane"/>. On a hit in front of the origin,
        /// returns (true, distance) along the ray; returns (false, 0) when the ray is parallel to the plane or the
        /// intersection lies behind the origin.
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, Plane3 plane)
        {
            float denominator = plane.Normal.X * ray.Direction.X + plane.Normal.Y * ray.Direction.Y + plane.Normal.Z * ray.Direction.Z;

            if (MathF.Abs(denominator) < Constants.FLOAT_ERROR_MARGIN)
                return (false, 0f);

            float numerator = -(plane.Normal.X * ray.Origin.X + plane.Normal.Y * ray.Origin.Y + plane.Normal.Z * ray.Origin.Z + plane.D);
            float distance = numerator / denominator;

            return distance >= 0f ? (true, distance) : (false, 0f);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="triangle"/> using the Möller–Trumbore
        /// algorithm. On a hit in front of the origin, returns (true, distance) along the ray; returns (false, 0)
        /// for a miss, a ray parallel to the triangle's plane, or an intersection behind the origin.
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, Triangle3 triangle)
        {
            var edge1 = new Vector3(triangle.A, triangle.B);
            var edge2 = new Vector3(triangle.A, triangle.C);

            var h = Vector3.Cross(ray.Direction, edge2);
            float a = Vector3.Dot(edge1, h);

            if (MathF.Abs(a) < Constants.FLOAT_ERROR_MARGIN)
                return (false, 0f); // ray is parallel to the triangle

            float f = 1f / a;
            var s = new Vector3(triangle.A, ray.Origin);
            float u = f * Vector3.Dot(s, h);

            if (u < 0f || u > 1f)
                return (false, 0f);

            var q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(ray.Direction, q);

            if (v < 0f || u + v > 1f)
                return (false, 0f);

            float t = f * Vector3.Dot(edge2, q);

            if (t < 0f)
                return (false, 0f); // triangle is behind the ray origin

            return (true, t);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="capsule"/>. On a hit, returns
        /// (true, distance) along the ray to the nearest intersection; otherwise returns (false, 0).
        /// <see cref="Capsule"/>'s constructor rejects equal endpoints, so the zero-length check below only guards
        /// a <c>default(Capsule)</c>. Closed-form solution against the capsule's cylindrical body and its two
        /// hemispherical end caps (Quilez, "Capsule - Intersection").
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, Capsule capsule)
        {
            var axis = new Vector3(capsule.PointA, capsule.PointB);
            float axisLengthSquared = axis.LengthSquared;

            if (axisLengthSquared == 0f)
                return Intersects(ray, new Sphere(capsule.PointA, capsule.Radius));

            var originToA = new Vector3(capsule.PointA, ray.Origin);

            float axisDotDirection = Vector3.Dot(axis, ray.Direction);
            float axisDotOriginToA = Vector3.Dot(axis, originToA);

            float a = axisLengthSquared - (axisDotDirection * axisDotDirection);
            float b = (axisLengthSquared * Vector3.Dot(ray.Direction, originToA)) - (axisDotOriginToA * axisDotDirection);
            float c = (axisLengthSquared * Vector3.Dot(originToA, originToA)) - (axisDotOriginToA * axisDotOriginToA) - (capsule.Radius * capsule.Radius * axisLengthSquared);

            float h = (b * b) - (a * c);

            if (h >= 0f && MathF.Abs(a) > Constants.FLOAT_ERROR_MARGIN)
            {
                float t = (-b - MathF.Sqrt(h)) / a;
                float y = axisDotOriginToA + (t * axisDotDirection);

                // hit lies against the cylindrical body, between the two end caps
                if (y > 0f && y < axisLengthSquared && t >= 0f)
                    return (true, t);
            }

            // otherwise, test the two hemispherical end caps as ordinary spheres and take the nearer valid hit
            var (hitA, distanceA) = Intersects(ray, new Sphere(capsule.PointA, capsule.Radius));
            var (hitB, distanceB) = Intersects(ray, new Sphere(capsule.PointB, capsule.Radius));

            if (hitA && (!hitB || distanceA <= distanceB))
                return (true, distanceA);

            if (hitB)
                return (true, distanceB);

            return (false, 0f);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="cylinder"/>. On a hit, returns
        /// (true, distance) along the ray to the nearest intersection; otherwise returns (false, 0). Uses the
        /// same closed-form quadratic as <see cref="Intersects(Ray, Capsule)"/> against the infinite cylindrical
        /// body, but - since <see cref="Cylinder"/>'s ends are flat, not hemispherical - falls back to two
        /// flat-disk cap tests instead of two sphere tests when the lateral hit falls outside the finite body.
        /// </summary>
        public static (bool Hit, float Distance) Intersects(Ray ray, Cylinder cylinder)
        {
            var axis = new Vector3(cylinder.PointA, cylinder.PointB);
            float axisLengthSquared = axis.LengthSquared;

            var originToA = new Vector3(cylinder.PointA, ray.Origin);

            float axisDotDirection = Vector3.Dot(axis, ray.Direction);
            float axisDotOriginToA = Vector3.Dot(axis, originToA);

            float a = axisLengthSquared - (axisDotDirection * axisDotDirection);
            float b = (axisLengthSquared * Vector3.Dot(ray.Direction, originToA)) - (axisDotOriginToA * axisDotDirection);
            float c = (axisLengthSquared * Vector3.Dot(originToA, originToA)) - (axisDotOriginToA * axisDotOriginToA) - (cylinder.Radius * cylinder.Radius * axisLengthSquared);

            float h = (b * b) - (a * c);

            if (h >= 0f && MathF.Abs(a) > Constants.FLOAT_ERROR_MARGIN)
            {
                float t = (-b - MathF.Sqrt(h)) / a;
                float y = axisDotOriginToA + (t * axisDotDirection);

                // hit lies against the cylindrical body, between the two flat end caps
                if (y > 0f && y < axisLengthSquared && t >= 0f)
                    return (true, t);
            }

            // otherwise, test the two flat end-cap disks and take the nearer valid hit
            var (hitA, distanceA) = RayDisk(cylinder.PointA);
            var (hitB, distanceB) = RayDisk(cylinder.PointB);

            if (hitA && (!hitB || distanceA <= distanceB))
                return (true, distanceA);

            if (hitB)
                return (true, distanceB);

            return (false, 0f);

            (bool Hit, float Distance) RayDisk(Point3 capCenter)
            {
                float denominator = Vector3.Dot(ray.Direction, axis);

                if (MathF.Abs(denominator) < Constants.FLOAT_ERROR_MARGIN)
                    return (false, 0f);

                float planeDistance = Vector3.Dot(new Vector3(ray.Origin, capCenter), axis) / denominator;

                if (planeDistance < 0f)
                    return (false, 0f);

                var planePoint = ray.PointAt(planeDistance);

                if (new Vector3(capCenter, planePoint).LengthSquared <= (cylinder.Radius * cylinder.Radius))
                    return (true, planeDistance);

                return (false, 0f);
            }
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="sphere"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Sphere sphere, Point3 point)
        {
            float dx = point.X - sphere.Center.X;
            float dy = point.Y - sphere.Center.Y;
            float dz = point.Z - sphere.Center.Z;

            return (dx * dx + dy * dy + dz * dz) <= (sphere.Radius * sphere.Radius);
        }

        /// <summary>
        /// Returns true if all eight corners of <paramref name="cube"/> lie inside or on <paramref name="sphere"/>
        /// (i.e. the cube is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Sphere sphere, Cube cube)
        {
            for (int i = 0; i < 8; i++)
            {
                if (!Contains(sphere, cube[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if all eight corners of <paramref name="aabb"/> lie inside or on <paramref name="sphere"/>
        /// (i.e. the box is fully enclosed).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Sphere sphere, AABB aabb)
        {
            return Contains(sphere, new Point3(aabb.Min.X, aabb.Min.Y, aabb.Min.Z))
                && Contains(sphere, new Point3(aabb.Min.X, aabb.Min.Y, aabb.Max.Z))
                && Contains(sphere, new Point3(aabb.Min.X, aabb.Max.Y, aabb.Min.Z))
                && Contains(sphere, new Point3(aabb.Min.X, aabb.Max.Y, aabb.Max.Z))
                && Contains(sphere, new Point3(aabb.Max.X, aabb.Min.Y, aabb.Min.Z))
                && Contains(sphere, new Point3(aabb.Max.X, aabb.Min.Y, aabb.Max.Z))
                && Contains(sphere, new Point3(aabb.Max.X, aabb.Max.Y, aabb.Min.Z))
                && Contains(sphere, new Point3(aabb.Max.X, aabb.Max.Y, aabb.Max.Z));
        }

        /// <summary>
        /// Returns true if point <paramref name="p"/> lies inside or on the faces of <paramref name="cube"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Cube cube, Point3 p)
        {
            return (cube.X1 <= p.X && cube.X2 >= p.X) && (cube.Y1 <= p.Y && cube.Y2 >= p.Y) && (cube.Z1 <= p.Z && cube.Z2 >= p.Z);
        }

        /// <summary>
        /// Returns true if <paramref name="b"/> lies entirely inside or on <paramref name="a"/> (i.e. <paramref name="b"/> is fully enclosed).
        /// Like the other containment tests, this assumes both cubes have X1 &lt;= X2, Y1 &lt;= Y2 and Z1 &lt;= Z2, so
        /// enclosing the two extreme corners of <paramref name="b"/> is enough.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Cube a, Cube b)
        {
            return Contains(a, new Point3(b.X1, b.Y1, b.Z1)) && Contains(a, new Point3(b.X2, b.Y2, b.Z2));
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the faces of <paramref name="aabb"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(AABB aabb, Point3 point)
        {
            return point.X >= aabb.Min.X && point.X <= aabb.Max.X &&
                   point.Y >= aabb.Min.Y && point.Y <= aabb.Max.Y &&
                   point.Z >= aabb.Min.Z && point.Z <= aabb.Max.Z;
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="capsule"/>. <see cref="Capsule"/>'s
        /// constructor rejects equal endpoints, so the zero-length check below only guards a <c>default(Capsule)</c>.
        /// </summary>
        public static bool Contains(Capsule capsule, Point3 point)
        {
            var ab = new Vector3(capsule.PointA, capsule.PointB);
            if (ab.LengthSquared == 0f)
                return Contains(new Sphere(capsule.PointA, capsule.Radius), point);

            var ap = new Vector3(capsule.PointA, point);
            float t = (ab.X * ap.X + ab.Y * ap.Y + ab.Z * ap.Z) / (ab.X * ab.X + ab.Y * ab.Y + ab.Z * ab.Z);
            t = Math.Clamp(t, 0f, 1f);

            var closest = new Point3(
                capsule.PointA.X + ab.X * t,
                capsule.PointA.Y + ab.Y * t,
                capsule.PointA.Z + ab.Z * t);

            return Contains(new Sphere(closest, capsule.Radius), point);
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="cylinder"/>. Unlike
        /// <see cref="Contains(Capsule, Point3)"/>, the ends are flat disks rather than hemispheres, so a point
        /// beyond either endpoint along the axis is excluded even if it's within <see cref="Cylinder.Radius"/>
        /// of the axis line. Unlike <see cref="Capsule"/>, a zero-length axis has no well-defined cap orientation
        /// to fall back to as some other shape - <see cref="Cylinder"/>'s constructor rejects equal endpoints, so
        /// this only matters for <c>default(Cylinder)</c>, which always returns false here (division by a
        /// zero-length axis produces NaN, and every NaN comparison below is false).
        /// </summary>
        public static bool Contains(Cylinder cylinder, Point3 point)
        {
            var axis = new Vector3(cylinder.PointA, cylinder.PointB);
            var ap = new Vector3(cylinder.PointA, point);

            float t = (axis.X * ap.X + axis.Y * ap.Y + axis.Z * ap.Z) / axis.LengthSquared;

            // beyond one of the flat end caps
            if (t < 0f || t > 1f)
                return false;

            var closest = new Point3(
                cylinder.PointA.X + axis.X * t,
                cylinder.PointA.Y + axis.Y * t,
                cylinder.PointA.Z + axis.Z * t);

            return new Vector3(closest, point).LengthSquared <= (cylinder.Radius * cylinder.Radius);
        }

        /// <summary>
        /// Returns true if <paramref name="point"/> lies inside or on the edges of <paramref name="triangle"/>.
        /// The point must be coplanar with the triangle (within <see cref="Constants.FLOAT_ERROR_MARGIN"/>);
        /// off-plane points always return false. Uses the standard barycentric-coordinate technique.
        /// </summary>
        public static bool Contains(Triangle3 triangle, Point3 point)
        {
            var edgeAB = new Vector3(triangle.A, triangle.B);
            var edgeAC = new Vector3(triangle.A, triangle.C);
            var toPoint = new Vector3(triangle.A, point);

            var normal = Vector3.Cross(edgeAB, edgeAC);

            // the point must lie in the triangle's plane
            if (MathF.Abs(Vector3.Dot(normal, toPoint)) > Constants.FLOAT_ERROR_MARGIN)
                return false;

            float dotABAB = Vector3.Dot(edgeAB, edgeAB);
            float dotABAC = Vector3.Dot(edgeAB, edgeAC);
            float dotACAC = Vector3.Dot(edgeAC, edgeAC);
            float dotABP = Vector3.Dot(edgeAB, toPoint);
            float dotACP = Vector3.Dot(edgeAC, toPoint);

            float denom = (dotABAB * dotACAC) - (dotABAC * dotABAC);

            if (MathF.Abs(denom) < Constants.FLOAT_ERROR_MARGIN)
                return false; // degenerate (zero-area) triangle

            float invDenom = 1f / denom;
            float weightB = ((dotACAC * dotABP) - (dotABAC * dotACP)) * invDenom;
            float weightC = ((dotABAB * dotACP) - (dotABAC * dotABP)) * invDenom;

            return weightB >= 0f && weightC >= 0f && (weightB + weightC) <= 1f;
        }
    }
}
