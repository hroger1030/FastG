using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class AABBBenchmarks
    {
        private readonly AABB _AABB = new(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f));
        private readonly Cube _Cube = new(1f, 1f, 1f, 3f, 3f, 3f);
        private readonly Sphere _Sphere = new(new Point3(2f, 2f, 2f), 1f);
        private readonly Capsule _Capsule = new(new Point3(1f, 1f, 1f), new Point3(3f, 3f, 3f), 0.5f);
        private readonly Cylinder _Cylinder = new(new Point3(1f, 1f, 1f), new Point3(3f, 3f, 3f), 0.5f);
        private readonly Point3 _Point = new(2f, 2f, 2f);

        [Benchmark]
        public AABB Construct()
        {
            return new AABB(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f));
        }

        [Benchmark]
        public float Volume()
        {
            return _AABB.Volume;
        }

        [Benchmark]
        public float SurfaceArea()
        {
            return _AABB.SurfaceArea;
        }

        [Benchmark]
        public AABB Scale()
        {
            return _AABB.Scale(2f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _AABB.Contains(_Point);
        }

        [Benchmark]
        public bool IntersectsAABB()
        {
            return _AABB.Intersects(_AABB);
        }

        [Benchmark]
        public bool IntersectsCube()
        {
            return _AABB.Intersects(_Cube);
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _AABB.Intersects(_Sphere);
        }

        [Benchmark]
        public bool IntersectsCapsule()
        {
            return _AABB.Intersects(_Capsule);
        }

        [Benchmark]
        public bool IntersectsCylinder()
        {
            return _AABB.Intersects(_Cylinder);
        }
    }
}
