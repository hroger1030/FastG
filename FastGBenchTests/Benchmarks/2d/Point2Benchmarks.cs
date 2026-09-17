using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Point2Benchmarks
    {
        private readonly Point2 _PointA = new(0f, 0f);
        private readonly Point2 _PointB = new(3f, 4f);
        private readonly Vector2 _Vector = new(1f, 2f);

        [Benchmark]
        public float DistanceTo()
        {
            return _PointA.DistanceTo(_PointB);
        }

        [Benchmark]
        public float DistanceSquaredTo()
        {
            return _PointA.DistanceSquaredTo(_PointB);
        }

        [Benchmark]
        public Vector2 DisplacementTo()
        {
            return _PointA.DisplacementTo(_PointB);
        }

        [Benchmark]
        public Point2 RotateAround()
        {
            return _PointB.RotateAround(_PointA, Constants.QUARTER_PI);
        }

        [Benchmark]
        public Point2 Add()
        {
            return _PointA + _Vector;
        }

        [Benchmark]
        public Point2 Subtract()
        {
            return _PointA - _Vector;
        }
    }
}
