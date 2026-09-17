using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Triangle3Benchmarks
    {
        private readonly Triangle3 _Triangle = new(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), new Point3(0f, 4f, 0f));
        private readonly Point3 _Point = new(1f, 1f, 0f);

        [Benchmark]
        public Triangle3 Construct()
        {
            return new Triangle3(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), new Point3(0f, 4f, 0f));
        }

        [Benchmark]
        public float Area()
        {
            return _Triangle.Area;
        }

        [Benchmark]
        public float Perimeter()
        {
            return _Triangle.Perimeter;
        }

        [Benchmark]
        public Point3 Centroid()
        {
            return _Triangle.Centroid;
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _Triangle.Contains(_Point);
        }

        [Benchmark]
        public Triangle3 Scale()
        {
            return _Triangle.Scale(2f);
        }
    }
}
