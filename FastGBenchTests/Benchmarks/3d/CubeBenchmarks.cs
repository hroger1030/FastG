using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class CubeBenchmarks
    {
        private readonly Cube _CubeA = new(0f, 0f, 0f, 4f, 4f, 4f);
        private readonly Cube _CubeB = new(1f, 1f, 1f, 3f, 3f, 3f);
        private readonly Sphere _Sphere = new(new Point3(2f, 2f, 2f), 3f);
        private readonly AABB _AABB = new(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f));
        private readonly Capsule _Capsule = new(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f), 0.5f);
        private readonly Cylinder _Cylinder = new(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f), 0.5f);
        private readonly Point3 _Point = new(2f, 2f, 2f);

        [Benchmark]
        public Cube Construct()
        {
            return new Cube(0f, 0f, 0f, 4f, 4f, 4f);
        }

        [Benchmark]
        public float Volume()
        {
            return _CubeA.Volume;
        }

        [Benchmark]
        public float SurfaceArea()
        {
            return _CubeA.SurfaceArea;
        }

        [Benchmark]
        public Point3 Center()
        {
            return _CubeA.Center;
        }

        [Benchmark]
        public Point3 Corner()
        {
            return _CubeA[3];
        }

        [Benchmark]
        public Cube Scale()
        {
            return _CubeA.Scale(2f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _CubeA.Contains(_Point);
        }

        [Benchmark]
        public bool ContainsCube()
        {
            return _CubeA.Contains(_CubeB);
        }

        [Benchmark]
        public bool IntersectsCube()
        {
            return _CubeA.Intersects(_CubeB);
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _CubeA.Intersects(_Sphere);
        }

        [Benchmark]
        public bool IntersectsAABB()
        {
            return _CubeA.Intersects(_AABB);
        }

        [Benchmark]
        public bool IntersectsCapsule()
        {
            return _CubeA.Intersects(_Capsule);
        }

        [Benchmark]
        public bool IntersectsCylinder()
        {
            return _CubeA.Intersects(_Cylinder);
        }
    }
}
