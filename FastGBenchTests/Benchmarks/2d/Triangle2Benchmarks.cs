using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Triangle2Benchmarks
    {
        private readonly Triangle2 _TriangleA = new(new Point2(0f, 0f), new Point2(4f, 0f), new Point2(0f, 4f));
        private readonly Triangle2 _TriangleB = new(new Point2(1f, 1f), new Point2(5f, 1f), new Point2(1f, 5f));
        private readonly Point2 _Point = new(1f, 1f);

        [Benchmark]
        public Triangle2 Construct()
        {
            return new Triangle2(new Point2(0f, 0f), new Point2(4f, 0f), new Point2(0f, 4f));
        }

        [Benchmark]
        public float Area()
        {
            return _TriangleA.Area;
        }

        [Benchmark]
        public float Perimeter()
        {
            return _TriangleA.Perimeter;
        }

        [Benchmark]
        public Point2 Centroid()
        {
            return _TriangleA.Centroid;
        }

        [Benchmark]
        public Triangle2.Type TriangleType()
        {
            return _TriangleA.TriangleType;
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _TriangleA.Contains(_Point);
        }

        [Benchmark]
        public bool IntersectsTriangle2()
        {
            return _TriangleA.Intersects(_TriangleB);
        }

        [Benchmark]
        public Triangle2 Scale()
        {
            return _TriangleA.Scale(2f);
        }
    }
}
