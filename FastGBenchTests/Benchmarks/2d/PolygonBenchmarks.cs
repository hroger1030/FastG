using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class PolygonBenchmarks
    {
        private readonly Polygon _Polygon = Polygon.HEXAGON;
        private readonly Point2 _Point = new(0.5f, 0.5f);

        [Benchmark]
        public Polygon Construct()
        {
            return new Polygon([new(0.5f, 0f), new(1f, 0.25f), new(1f, 0.75f), new(0.5f, 1f), new(0f, 0.75f), new(0f, 0.25f)]);
        }

        [Benchmark]
        public float Area()
        {
            return _Polygon.Area;
        }

        [Benchmark]
        public float Perimeter()
        {
            return _Polygon.Perimeter;
        }

        [Benchmark]
        public Point2 Centroid()
        {
            return _Polygon.Centroid;
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _Polygon.Contains(_Point);
        }

        [Benchmark]
        public Polygon Scale()
        {
            return _Polygon.Scale(2f);
        }
    }
}
