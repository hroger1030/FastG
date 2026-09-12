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
        /// Returns true if <paramref name="capsule"/> overlaps or touches <paramref name="sphere"/>, tested via the
        /// closest point on the capsule's core segment to the sphere center. A degenerate capsule
        /// (PointA == PointB) is treated as a sphere.
        /// </summary>
        public static bool Intersects(Capsule capsule, Sphere sphere)
        {
            var ab = new Vector3(capsule.PointA, capsule.PointB);

            if (ab.LengthSquared() == 0f)
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
        /// Tests <paramref name="ray"/> for intersection with <paramref name="sphere"/>. On a hit, returns true and sets
        /// <paramref name="distance"/> to the distance along the ray of the nearest non-negative intersection; otherwise
        /// returns false and sets it to 0. An origin inside the sphere counts as a hit at the exit point.
        /// </summary>
        public static bool Intersects(Ray ray, Sphere sphere, out float distance)
        {
            float dx = ray.Origin.X - sphere.Center.X;
            float dy = ray.Origin.Y - sphere.Center.Y;
            float dz = ray.Origin.Z - sphere.Center.Z;

            float b = dx * ray.Direction.X + dy * ray.Direction.Y + dz * ray.Direction.Z;
            float c = dx * dx + dy * dy + dz * dz - sphere.Radius * sphere.Radius;
            float discriminant = b * b - c;

            if (discriminant < 0f)
            {
                distance = 0f;
                return false;
            }

            float sqrt = MathF.Sqrt(discriminant);
            float t0 = -b - sqrt;
            float t1 = -b + sqrt;

            if (t0 >= 0f)
            {
                distance = t0;
                return true;
            }

            if (t1 >= 0f)
            {
                distance = t1;
                return true;
            }

            distance = 0f;
            return false;
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="aabb"/> using the slab method. On a hit,
        /// returns true and sets <paramref name="distance"/> to the entry distance along the ray (clamped to 0 when the
        /// origin is inside); otherwise returns false and sets it to 0.
        /// </summary>
        public static bool Intersects(Ray ray, AABB aabb, out float distance)
        {
            float tMin = float.NegativeInfinity;
            float tMax = float.PositiveInfinity;

            if (MathF.Abs(ray.Direction.X) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.X < aabb.Min.X || ray.Origin.X > aabb.Max.X)
                {
                    distance = 0f;
                    return false;
                }
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
                {
                    distance = 0f;
                    return false;
                }
            }

            if (MathF.Abs(ray.Direction.Y) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.Y < aabb.Min.Y || ray.Origin.Y > aabb.Max.Y)
                {
                    distance = 0f;
                    return false;
                }
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
                {
                    distance = 0f;
                    return false;
                }
            }

            if (MathF.Abs(ray.Direction.Z) < Constants.FLOAT_ERROR_MARGIN)
            {
                if (ray.Origin.Z < aabb.Min.Z || ray.Origin.Z > aabb.Max.Z)
                {
                    distance = 0f;
                    return false;
                }
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
                {
                    distance = 0f;
                    return false;
                }
            }

            distance = MathF.Max(0f, tMin);
            return true;
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="cube"/> by treating it as an axis-aligned
        /// box. See <see cref="Intersects(Ray, AABB, out float)"/> for the meaning of <paramref name="distance"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Ray ray, Cube cube, out float distance)
        {
            return Intersects(
                ray,
                new AABB(new Point3(cube.X1, cube.Y1, cube.Z1), new Point3(cube.X2, cube.Y2, cube.Z2)),
                out distance);
        }

        /// <summary>
        /// Tests <paramref name="ray"/> for intersection with <paramref name="plane"/>. On a hit in front of the origin,
        /// returns true and sets <paramref name="distance"/> to the distance along the ray; returns false (distance 0)
        /// when the ray is parallel to the plane or the intersection lies behind the origin.
        /// </summary>
        public static bool Intersects(Ray ray, Plane3 plane, out float distance)
        {
            float denominator = plane.Normal.X * ray.Direction.X + plane.Normal.Y * ray.Direction.Y + plane.Normal.Z * ray.Direction.Z;

            if (MathF.Abs(denominator) < Constants.FLOAT_ERROR_MARGIN)
            {
                distance = 0f;
                return false;
            }

            float numerator = -(plane.Normal.X * ray.Origin.X + plane.Normal.Y * ray.Origin.Y + plane.Normal.Z * ray.Origin.Z + plane.D);
            distance = numerator / denominator;

            return distance >= 0f;
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
        /// Returns true if <paramref name="point"/> lies inside or on <paramref name="capsule"/>. A degenerate capsule
        /// (PointA == PointB) is treated as a sphere.
        /// </summary>
        public static bool Contains(Capsule capsule, Point3 point)
        {
            var ab = new Vector3(capsule.PointA, capsule.PointB);
            if (ab.LengthSquared() == 0f)
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
    }
}
