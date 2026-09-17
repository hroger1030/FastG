using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class AARectangleBenchmarks
    {
        private readonly AARectangle _RectangleA = new(0f, 0f, 4f, 4f);
        private readonly AARectangle _RectangleB = new(2f, 2f, 4f, 4f);
        private readonly Circle _Circle = new(1f, 1f, 1f);
        private readonly Point2 _Point = new(1f, 1f);

        [Benchmark]
        public AARectangle Construct()
        {
            return new AARectangle(0f, 0f, 4f, 4f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _RectangleA.Contains(_Point);
        }

        [Benchmark]
        public bool ContainsAARectangle()
        {
            return _RectangleA.Contains(_RectangleB);
        }

        [Benchmark]
        public bool IntersectsAARectangle()
        {
            return _RectangleA.Intersects(_RectangleB);
        }

        [Benchmark]
        public bool IntersectsCircle()
        {
            return _RectangleA.Intersects(_Circle);
        }

        [Benchmark]
        public AARectangle Scale()
        {
            return _RectangleA.Scale(2f);
        }

        [Benchmark]
        public AARectangle Union()
        {
            return AARectangle.Union(_RectangleA, _RectangleB);
        }
    }
}
