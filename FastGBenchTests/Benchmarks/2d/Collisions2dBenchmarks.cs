using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Collisions2dBenchmarks
    {
        private readonly Circle _CircleA = new(0f, 0f, 1f);
        private readonly Circle _CircleB = new(1.5f, 0f, 1f);
        private readonly AARectangle _RectangleA = new(0f, 0f, 4f, 4f);
        private readonly AARectangle _RectangleB = new(2f, 2f, 4f, 4f);
        private readonly Triangle2 _TriangleA = new(new Point2(0f, 0f), new Point2(4f, 0f), new Point2(0f, 4f));
        private readonly Triangle2 _TriangleB = new(new Point2(1f, 1f), new Point2(5f, 1f), new Point2(1f, 5f));
        private readonly Line2 _LineA = new(new Point2(0f, 0f), new Point2(4f, 4f));
        private readonly Line2 _LineB = new(new Point2(0f, 4f), new Point2(4f, 0f));
        private readonly Ellipse _Ellipse = new(Point2.ZERO, 2f, 1f);
        private readonly Polygon _Polygon = Polygon.HEXAGON;
        private readonly Point2 _Point = new(0.5f, 0.5f);

        [Benchmark]
        public bool IntersectsCircleCircle()
        {
            return Collisions2d.Intersects(_CircleA, _CircleB);
        }

        [Benchmark]
        public bool IntersectsCircleAARectangle()
        {
            return Collisions2d.Intersects(_CircleA, _RectangleA);
        }

        [Benchmark]
        public bool IntersectsAARectangleAARectangle()
        {
            return Collisions2d.Intersects(_RectangleA, _RectangleB);
        }

        [Benchmark]
        public bool ContainsCirclePoint()
        {
            return Collisions2d.Contains(_CircleA, _Point);
        }

        [Benchmark]
        public bool ContainsCircleAARectangle()
        {
            return Collisions2d.Contains(_CircleA, _RectangleA);
        }

        [Benchmark]
        public bool ContainsCircleTriangle2()
        {
            return Collisions2d.Contains(_CircleA, _TriangleA);
        }

        [Benchmark]
        public bool ContainsAARectanglePoint()
        {
            return Collisions2d.Contains(_RectangleA, _Point);
        }

        [Benchmark]
        public bool ContainsAARectangleAARectangle()
        {
            return Collisions2d.Contains(_RectangleA, _RectangleB);
        }

        [Benchmark]
        public bool IntersectsLine2Line2()
        {
            return Collisions2d.Intersects(_LineA, _LineB);
        }

        [Benchmark]
        public bool ContainsLine2Point()
        {
            return Collisions2d.Contains(_LineA, _Point);
        }

        [Benchmark]
        public bool ContainsTriangle2Point()
        {
            return Collisions2d.Contains(_TriangleA, _Point);
        }

        [Benchmark]
        public bool IntersectsTriangle2Triangle2()
        {
            return Collisions2d.Intersects(_TriangleA, _TriangleB);
        }

        [Benchmark]
        public bool ContainsEllipsePoint()
        {
            return Collisions2d.Contains(_Ellipse, _Point);
        }

        [Benchmark]
        public bool ContainsPolygonPoint()
        {
            return Collisions2d.Contains(_Polygon, _Point);
        }
    }
}
