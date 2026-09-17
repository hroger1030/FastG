using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class CircleBenchmarks
    {
        private readonly Circle _CircleA = new(0f, 0f, 1f);
        private readonly Circle _CircleB = new(1.5f, 0f, 1f);
        private readonly AARectangle _Rectangle = new(0f, 0f, 2f, 2f);
        private readonly Triangle2 _Triangle = new(new Point2(-0.5f, -0.5f), new Point2(0.5f, -0.5f), new Point2(0f, 0.5f));
        private readonly Point2 _Point = new(0.5f, 0.5f);

        [Benchmark]
        public Circle Construct()
        {
            return new Circle(0f, 0f, 1f);
        }

        [Benchmark]
        public bool ContainsPoint()
        {
            return _CircleA.Contains(_Point);
        }

        [Benchmark]
        public bool ContainsAARectangle()
        {
            return _CircleA.Contains(_Rectangle);
        }

        [Benchmark]
        public bool ContainsTriangle2()
        {
            return _CircleA.Contains(_Triangle);
        }

        [Benchmark]
        public bool IntersectsCircle()
        {
            return _CircleA.Intersects(_CircleB);
        }

        [Benchmark]
        public bool IntersectsAARectangle()
        {
            return _CircleA.Intersects(_Rectangle);
        }

        [Benchmark]
        public Circle Scale()
        {
            return _CircleA.Scale(2f);
        }
    }
}
