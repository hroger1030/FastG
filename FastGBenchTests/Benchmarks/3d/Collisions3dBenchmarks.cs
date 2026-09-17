using BenchmarkDotNet.Attributes;
using FastG;

namespace FastGBenchTests.Benchmarks
{
    [MemoryDiagnoser]
    public class Collisions3dBenchmarks
    {
        private readonly Sphere _SphereA = new(new Point3(0f, 0f, 0f), 2f);
        private readonly Sphere _SphereB = new(new Point3(2f, 0f, 0f), 2f);
        private readonly Cube _CubeA = new(0f, 0f, 0f, 4f, 4f, 4f);
        private readonly Cube _CubeB = new(1f, 1f, 1f, 3f, 3f, 3f);
        private readonly AABB _AABBA = new(new Point3(0f, 0f, 0f), new Point3(4f, 4f, 4f));
        private readonly AABB _AABBB = new(new Point3(1f, 1f, 1f), new Point3(3f, 3f, 3f));
        private readonly Plane3 _Plane = new(new Vector3(0f, 0f, 1f), 0f);
        private readonly Capsule _CapsuleA = new(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        private readonly Capsule _CapsuleB = new(new Point3(0f, 2f, 0f), new Point3(4f, 2f, 0f), 1f);
        private readonly Cylinder _CylinderA = new(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
        private readonly Cylinder _CylinderB = new(new Point3(0f, 2f, 0f), new Point3(4f, 2f, 0f), 1f);
        private readonly Ray _Ray = new(Point3.ZERO, new Vector3(1f, 0f, 0f));
        private readonly Triangle3 _Triangle = new(new Point3(5f, -1f, -1f), new Point3(5f, 1f, -1f), new Point3(5f, 0f, 1f));
        private readonly Point3 _Point = new(1f, 1f, 1f);

        [Benchmark]
        public bool IntersectsSphereSphere()
        {
            return Collisions3d.Intersects(_SphereA, _SphereB);
        }

        [Benchmark]
        public bool IntersectsCubeSphere()
        {
            return Collisions3d.Intersects(_CubeA, _SphereA);
        }

        [Benchmark]
        public bool IntersectsCubeCube()
        {
            return Collisions3d.Intersects(_CubeA, _CubeB);
        }

        [Benchmark]
        public bool IntersectsAABBAABB()
        {
            return Collisions3d.Intersects(_AABBA, _AABBB);
        }

        [Benchmark]
        public bool IntersectsAABBCube()
        {
            return Collisions3d.Intersects(_AABBA, _CubeA);
        }

        [Benchmark]
        public bool IntersectsAABBSphere()
        {
            return Collisions3d.Intersects(_AABBA, _SphereA);
        }

        [Benchmark]
        public bool IntersectsPlane3Sphere()
        {
            return Collisions3d.Intersects(_Plane, _SphereA);
        }

        [Benchmark]
        public bool IntersectsCapsuleSphere()
        {
            return Collisions3d.Intersects(_CapsuleA, _SphereA);
        }

        [Benchmark]
        public bool IntersectsAABBCapsule()
        {
            return Collisions3d.Intersects(_AABBA, _CapsuleA);
        }

        [Benchmark]
        public bool IntersectsCapsuleCube()
        {
            return Collisions3d.Intersects(_CapsuleA, _CubeA);
        }

        [Benchmark]
        public bool IntersectsCapsuleCapsule()
        {
            return Collisions3d.Intersects(_CapsuleA, _CapsuleB);
        }

        [Benchmark]
        public bool IntersectsCylinderSphere()
        {
            return Collisions3d.Intersects(_CylinderA, _SphereA);
        }

        [Benchmark]
        public bool IntersectsAABBCylinder()
        {
            return Collisions3d.Intersects(_AABBA, _CylinderA);
        }

        [Benchmark]
        public bool IntersectsCubeCylinder()
        {
            return Collisions3d.Intersects(_CubeA, _CylinderA);
        }

        [Benchmark]
        public bool IntersectsCylinderCylinder()
        {
            return Collisions3d.Intersects(_CylinderA, _CylinderB);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRaySphere()
        {
            return Collisions3d.Intersects(_Ray, _SphereA);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayAABB()
        {
            return Collisions3d.Intersects(_Ray, _AABBA);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayCube()
        {
            return Collisions3d.Intersects(_Ray, _CubeA);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayPlane3()
        {
            return Collisions3d.Intersects(_Ray, _Plane);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayTriangle3()
        {
            return Collisions3d.Intersects(_Ray, _Triangle);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayCapsule()
        {
            return Collisions3d.Intersects(_Ray, _CapsuleA);
        }

        [Benchmark]
        public (bool Hit, float Distance) IntersectsRayCylinder()
        {
            return Collisions3d.Intersects(_Ray, _CylinderA);
        }

        [Benchmark]
        public bool ContainsSpherePoint()
        {
            return Collisions3d.Contains(_SphereA, _Point);
        }

        [Benchmark]
        public bool ContainsSphereCube()
        {
            return Collisions3d.Contains(_SphereA, _CubeB);
        }

        [Benchmark]
        public bool ContainsSphereAABB()
        {
            return Collisions3d.Contains(_SphereA, _AABBB);
        }

        [Benchmark]
        public bool ContainsCubePoint()
        {
            return Collisions3d.Contains(_CubeA, _Point);
        }

        [Benchmark]
        public bool ContainsCubeCube()
        {
            return Collisions3d.Contains(_CubeA, _CubeB);
        }

        [Benchmark]
        public bool ContainsAABBPoint()
        {
            return Collisions3d.Contains(_AABBA, _Point);
        }

        [Benchmark]
        public bool ContainsCapsulePoint()
        {
            return Collisions3d.Contains(_CapsuleA, _Point);
        }

        [Benchmark]
        public bool ContainsCylinderPoint()
        {
            return Collisions3d.Contains(_CylinderA, _Point);
        }

        [Benchmark]
        public bool ContainsTriangle3Point()
        {
            return Collisions3d.Contains(_Triangle, new Point3(5f, 0f, -0.5f));
        }
    }
}
