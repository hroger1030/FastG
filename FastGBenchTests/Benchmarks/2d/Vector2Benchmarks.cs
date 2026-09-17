using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Vector2Benchmarks
    {
        private readonly Vector2 _VectorA = new(3f, 4f);
        private readonly Vector2 _VectorB = new(1f, 2f);

        [Benchmark]
        public Vector2 Normalize()
        {
            return _VectorA.Normalize();
        }

        [Benchmark]
        public float Length()
        {
            return _VectorA.Length;
        }

        [Benchmark]
        public float LengthSquared()
        {
            return _VectorA.LengthSquared;
        }

        [Benchmark]
        public float Dot()
        {
            return Vector2.Dot(_VectorA, _VectorB);
        }

        [Benchmark]
        public float Cross()
        {
            return Vector2.Cross(_VectorA, _VectorB);
        }

        [Benchmark]
        public float VectorToRotation()
        {
            return _VectorA.VectorToRotation();
        }

        [Benchmark]
        public Vector2 Add()
        {
            return _VectorA + _VectorB;
        }

        [Benchmark]
        public Vector2 Scale()
        {
            return _VectorA * 2.5f;
        }
    }
}
