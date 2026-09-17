using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class CapsuleBenchmarks
    {
        private readonly Capsule _CapsuleA = new(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        private readonly Capsule _CapsuleB = new(new Point3(0f, 2f, 0f), new Point3(4f, 2f, 0f), 1f);
        private readonly Sphere _Sphere = new(new Point3(2f, 0f, 0f), 1f);
        private readonly AABB _AABB = new(new Point3(1f, -1f, -1f), new Point3(3f, 1f, 1f));
        private readonly Cube _Cube = new(1f, -1f, -1f, 3f, 1f, 1f);
        private readonly Point3 _Point = new(2f, 0f, 0f);

        [Benchmark]
        public Capsule Construct()
        {
            return new Capsule(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        }

        [Benchmark]
        public float Volume()
        {
            return _CapsuleA.Volume;
        }

        [Benchmark]
        public float SurfaceArea()
        {
            return _CapsuleA.SurfaceArea;
        }

        [Benchmark]
        public Capsule Scale()
        {
            return _CapsuleA.Scale(2f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _CapsuleA.Contains(_Point);
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _CapsuleA.Intersects(_Sphere);
        }

        [Benchmark]
        public bool IntersectsAABB()
        {
            return _CapsuleA.Intersects(_AABB);
        }

        [Benchmark]
        public bool IntersectsCube()
        {
            return _CapsuleA.Intersects(_Cube);
        }

        [Benchmark]
        public bool IntersectsCapsule()
        {
            return _CapsuleA.Intersects(_CapsuleB);
        }
    }
}
