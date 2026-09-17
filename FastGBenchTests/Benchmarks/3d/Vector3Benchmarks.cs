using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Vector3Benchmarks
    {
        private readonly Vector3 _VectorA = new(3f, 4f, 0f);
        private readonly Vector3 _VectorB = new(1f, 2f, 3f);

        [Benchmark]
        public Vector3 Normalize()
        {
            return Vector3.Normalize(_VectorA);
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
            return Vector3.Dot(_VectorA, _VectorB);
        }

        [Benchmark]
        public Vector3 Cross()
        {
            return Vector3.Cross(_VectorA, _VectorB);
        }

        [Benchmark]
        public float DistanceTo()
        {
            return _VectorA.DistanceTo(_VectorB);
        }

        [Benchmark]
        public float DistanceSquaredTo()
        {
            return _VectorA.DistanceSquaredTo(_VectorB);
        }

        [Benchmark]
        public Vector3 Add()
        {
            return _VectorA + _VectorB;
        }

        [Benchmark]
        public Vector3 Scale()
        {
            return _VectorA * 2.5f;
        }
    }
}
