using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class VectorNBenchmarks
    {
        private readonly VectorN _VectorA = new([1f, 2f, 3f, 4f, 5f]);
        private readonly VectorN _VectorB = new([5f, 4f, 3f, 2f, 1f]);

        [Benchmark]
        public VectorN Construct()
        {
            return new VectorN([1f, 2f, 3f, 4f, 5f]);
        }

        [Benchmark]
        public VectorN Add()
        {
            return _VectorA + _VectorB;
        }

        [Benchmark]
        public VectorN Subtract()
        {
            return _VectorA - _VectorB;
        }

        [Benchmark]
        public VectorN Scale()
        {
            return _VectorA * 2.5f;
        }

        [Benchmark]
        public VectorN Divide()
        {
            return _VectorA / 2.5f;
        }

        [Benchmark]
        public float Dot()
        {
            return VectorN.Dot(_VectorA, _VectorB);
        }
    }
}
