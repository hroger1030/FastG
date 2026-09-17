using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class CylinderBenchmarks
    {
        private readonly Cylinder _CylinderA = new(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        private readonly Cylinder _CylinderB = new(new Point3(0f, 2f, 0f), new Point3(4f, 2f, 0f), 1f);
        private readonly Sphere _Sphere = new(new Point3(2f, 0f, 0f), 1f);
        private readonly AABB _AABB = new(new Point3(1f, -1f, -1f), new Point3(3f, 1f, 1f));
        private readonly Cube _Cube = new(1f, -1f, -1f, 3f, 1f, 1f);
        private readonly Point3 _Point = new(2f, 0f, 0f);

        [Benchmark]
        public Cylinder Construct()
        {
            return new Cylinder(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        }

        [Benchmark]
        public float Volume()
        {
            return _CylinderA.Volume;
        }

        [Benchmark]
        public float SurfaceArea()
        {
            return _CylinderA.SurfaceArea;
        }

        [Benchmark]
        public Cylinder Scale()
        {
            return _CylinderA.Scale(2f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _CylinderA.Contains(_Point);
        }

        [Benchmark]
        public bool IntersectsSphere()
        {
            return _CylinderA.Intersects(_Sphere);
        }

        [Benchmark]
        public bool IntersectsAABB()
        {
            return _CylinderA.Intersects(_AABB);
        }

        [Benchmark]
        public bool IntersectsCube()
        {
            return _CylinderA.Intersects(_Cube);
        }

        [Benchmark]
        public bool IntersectsCylinder()
        {
            return _CylinderA.Intersects(_CylinderB);
        }
    }
}
