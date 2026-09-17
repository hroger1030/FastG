using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class RayBenchmarks
    {
        private readonly Ray _Ray = new(Point3.ZERO, new Vector3(1f, 0f, 0f));
        private readonly Sphere _Sphere = new(new Point3(5f, 0f, 0f), 1f);
        private readonly AABB _AABB = new(new Point3(4f, -1f, -1f), new Point3(6f, 1f, 1f));
        private readonly Cube _Cube = new(4f, -1f, -1f, 6f, 1f, 1f);
        private readonly Plane3 _Plane = new(new Vector3(1f, 0f, 0f), -5f);
        private readonly Triangle3 _Triangle = new(new Point3(5f, -1f, -1f), new Point3(5f, 1f, -1f), new Point3(5f, 0f, 1f));
        private readonly Capsule _Capsule = new(new Point3(5f, -1f, 0f), new Point3(5f, 1f, 0f), 0.5f);
        private readonly Cylinder _Cylinder = new(new Point3(5f, -1f, 0f), new Point3(5f, 1f, 0f), 0.5f);

        [Benchmark]
        public Ray Construct()
        {
            return new Ray(Point3.ZERO, new Vector3(1f, 0f, 0f));
        }

        [Benchmark]
        public Point3 PointAt()
        {
            return _Ray.PointAt(5f);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsSphere()
        {
            return _Ray.Intersects(_Sphere);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsAABB()
        {
            return _Ray.Intersects(_AABB);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsCube()
        {
            return _Ray.Intersects(_Cube);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsPlane3()
        {
            return _Ray.Intersects(_Plane);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsTriangle3()
        {
            return _Ray.Intersects(_Triangle);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsCapsule()
        {
            return _Ray.Intersects(_Capsule);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsCylinder()
        {
            return _Ray.Intersects(_Cylinder);
        }
    }
}
