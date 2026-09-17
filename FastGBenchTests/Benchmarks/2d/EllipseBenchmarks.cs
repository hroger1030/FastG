using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class EllipseBenchmarks
    {
        private readonly Ellipse _Ellipse = new(Point2.ZERO, 2f, 1f);
        private readonly Point2 _Point = new(1f, 0.25f);

        [Benchmark]
        public Ellipse Construct()
        {
            return new Ellipse(Point2.ZERO, 2f, 1f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _Ellipse.Contains(_Point);
        }

        [Benchmark]
        public float Area()
        {
            return _Ellipse.Area;
        }

        [Benchmark]
        public float Perimeter()
        {
            return _Ellipse.Perimeter;
        }

        [Benchmark]
        public Ellipse Scale()
        {
            return _Ellipse.Scale(2f);
        }
    }
}
