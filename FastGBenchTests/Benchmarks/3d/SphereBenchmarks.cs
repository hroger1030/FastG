using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class SphereBenchmarks
    {
        private readonly Sphere _SphereA = new(new Point3(0f, 0f, 0f), 2f);
        private readonly Sphere _SphereB = new(new Point3(2f, 0f, 0f), 2f);
        private readonly Cube _Cube = new(-1f, -1f, -1f, 1f, 1f, 1f);
        private readonly AABB _AABB = new(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));
        private readonly Plane3 _Plane = new(new Vector3(0f, 0f, 1f), 0f);
        private readonly Point3 _Point = new(1f, 0f, 0f);

        [Benchmark]
        public Sphere Construct()
        {
            return new Sphere(new Point3(0f, 0f, 0f), 2f);
        }

        [Benchmark]
        public float Volume()
        {
            return _SphereA.Volume;
        }

        [Benchmark]
        public float SurfaceArea()
        {
            return _SphereA.SurfaceArea;
        }

        [Benchmark]
        public Sphere Scale()
        {
            return _SphereA.Scale(2f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _SphereA.Contains(_Point);
        }

        [Benchmark]
        public bool ContainsCube()
        {
            return _SphereA.Contains(_Cube);
        }

        [Benchmark]
        public bool ContainsAABB()
        {
            return _SphereA.Contains(_AABB);
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _SphereA.Intersects(_SphereB);
        }

        [Benchmark]
        public bool IntersectsCube()
        {
            return _SphereA.Intersects(_Cube);
        }

        [Benchmark]
        public bool IntersectsAABB()
        {
            return _SphereA.Intersects(_AABB);
        }

        [Benchmark]
        public bool IntersectsPlane3()
        {
            return _SphereA.Intersects(_Plane);
        }
    }
}
