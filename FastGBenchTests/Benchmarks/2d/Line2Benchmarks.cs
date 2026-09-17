using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Line2Benchmarks
    {
        private readonly Line2 _LineA = new(new Point2(0f, 0f), new Point2(4f, 4f));
        private readonly Line2 _LineB = new(new Point2(0f, 4f), new Point2(4f, 0f));
        private readonly Point2 _Point = new(2f, 2f);

        [Benchmark]
        public Line2 Construct()
        {
            return new Line2(new Point2(0f, 0f), new Point2(4f, 4f));
        }

        [Benchmark]
        public float Length()
        {
            return _LineA.Length;
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _LineA.Contains(_Point);
        }

        [Benchmark]
        public bool IntersectsLine2()
        {
            return _LineA.Intersects(_LineB);
        }

        [Benchmark]
        public Line2 Scale()
        {
            return _LineA.Scale(2f);
        }
    }
}
