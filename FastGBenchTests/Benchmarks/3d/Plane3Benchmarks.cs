using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Plane3Benchmarks
    {
        private readonly Plane3 _Plane = new(new Vector3(0f, 0f, 1f), -2f);
        private readonly Sphere _Sphere = new(new Point3(0f, 0f, 1f), 2f);
        private readonly Point3 _Point = new(1f, 1f, 5f);

        [Benchmark]
        public Plane3 Construct()
        {
            return new Plane3(new Vector3(0f, 0f, 1f), -2f);
        }

        [Benchmark]
        public float DistanceTo()
        {
            return _Plane.DistanceTo(_Point);
        }

        [Benchmark]
        public Plane3 Normalize()
        {
            return _Plane.Normalize();
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _Plane.Intersects(_Sphere);
        }
    }
}
