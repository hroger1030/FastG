using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Point3Benchmarks
    {
        private readonly Point3 _Point = new(1f, 2f, 3f);
        private readonly Vector3 _Vector = new(1f, 1f, 1f);

        [Benchmark]
        public Point3 Add()
        {
            return _Point + _Vector;
        }

        [Benchmark]
        public Point3 Subtract()
        {
            return _Point - _Vector;
        }
    }
}
