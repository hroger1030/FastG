/*
The MIT License (MIT)

Copyright (c) 2017 Roger Hill

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Geometry;
using NUnit.Framework;

namespace GeometryTests
{
    /// <summary>
    /// Tests for the pairwise intersection/containment tests in <see cref="Collisions3d"/> (including ray casts),
    /// exercised through each shape's forwarding instance methods (e.g. <see cref="Sphere.Intersects(Cube)"/>).
    /// Moved here from the individual shape test fixtures when the collision logic itself moved to <see cref="Collisions3d"/>.
    /// </summary>
    [TestFixture]
    public class Collisions3dTests
    {
        [Test]
        [Category("Collisions3d")]
        [Category("Sphere")]
        public void Sphere_ContainsAndIntersectsSphere_Pass()
        {
            var sphere = new Sphere(new Point3(0f, 0f, 0f), 2f);
            Assert.That(sphere.Contains(new Point3(1f, 0f, 0f)), Is.True);
            Assert.That(sphere.Contains(new Point3(3f, 0f, 0f)), Is.False);

            var nearby = new Sphere(new Point3(3f, 0f, 0f), 1.5f);
            Assert.That(sphere.Intersects(nearby), Is.True);
            Assert.That(sphere.Intersects(new Sphere(new Point3(5f, 0f, 0f), 1f)), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Sphere")]
        [Category("Cube")]
        public void Sphere_IntersectsCube_Pass()
        {
            var sphere = new Sphere(new Point3(0f, 0f, 0f), 1f);
            var overlapping = new Cube(0.5f, 0.5f, 0.5f, 2f, 2f, 2f);
            var separate = new Cube(10f, 10f, 10f, 11f, 11f, 11f);

            Assert.That(sphere.Intersects(overlapping), Is.True);
            Assert.That(sphere.Intersects(separate), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Sphere")]
        [Category("Cube")]
        public void Sphere_ContainsCube_Pass()
        {
            var sphere = new Sphere(new Point3(0f, 0f, 0f), 10f);
            var containedCube = new Cube(-1f, -1f, -1f, 1f, 1f, 1f);
            var farCube = new Cube(50f, 50f, 50f, 51f, 51f, 51f);

            Assert.That(sphere.Contains(containedCube), Is.True);
            Assert.That(sphere.Contains(farCube), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cube")]
        public void Cube_ContainsAndIntersects_Pass()
        {
            var inner = new Cube(new Point3(0f, 0f, 0f), new Point3(1f, 1f, 1f));
            var outer = new Cube(new Point3(-1f, -1f, -1f), new Point3(2f, 2f, 2f));

            Assert.That(outer.Contains(inner), Is.True);
            Assert.That(inner.Intersects(outer), Is.True);
            Assert.That(outer.Intersects(new Cube(new Point3(3f, 3f, 3f), new Point3(4f, 4f, 4f))), Is.False);

            // partial overlap and full-miss must not count as containment
            Assert.That(inner.Contains(outer), Is.False);
            Assert.That(outer.Contains(new Cube(new Point3(1f, 1f, 1f), new Point3(3f, 3f, 3f))), Is.False);
            Assert.That(outer.Contains(new Cube(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f))), Is.False);
            Assert.That(outer.Contains(outer), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cube")]
        public void Cube_ContainsPointAndCoordinates_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);

            Assert.That(cube.Contains(new Point3(0.5f, 0.5f, 0.5f)), Is.True);
            Assert.That(cube.Contains(2f, 0.5f, 0.5f), Is.False);
            Assert.That(cube.Contains(0.5f, 0.5f, 0.5f), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cube")]
        [Category("Sphere")]
        public void Cube_IntersectsSphere_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            var overlapping = new Sphere(new Point3(1.5f, 0.5f, 0.5f), 1f);
            var separate = new Sphere(new Point3(10f, 10f, 10f), 1f);

            Assert.That(cube.Intersects(overlapping), Is.True);
            Assert.That(cube.Intersects(separate), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        public void AABB_ContainsAndIntersects_Pass()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));

            Assert.That(box.Contains(new Point3(1f, 1f, 1f)), Is.True);
            Assert.That(box.Contains(new Point3(3f, 1f, 1f)), Is.False);
            Assert.That(box.Intersects(new AABB(new Point3(1.5f, 1.5f, 1.5f), new Point3(3f, 3f, 3f))), Is.True);
            Assert.That(box.Intersects(new AABB(new Point3(3f, 3f, 3f), new Point3(4f, 4f, 4f))), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Capsule")]
        public void Capsule_ContainsAndIntersects_Pass()
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);
            Assert.That(capsule.Contains(new Point3(0f, 0f, 1f)), Is.True);
            Assert.That(capsule.Contains(new Point3(1f, 0f, 1f)), Is.True);
            Assert.That(capsule.Contains(new Point3(0f, 2f, 1f)), Is.False);

            var sphere = new Sphere(new Point3(0f, 0f, 3f), 1f);
            Assert.That(capsule.Intersects(sphere), Is.True);
            Assert.That(capsule.Intersects(new Sphere(new Point3(0f, 0f, 5f), 0.5f)), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Sphere")]
        public void Ray_IntersectsSphere_ReturnsExpectedDistance()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var sphere = new Sphere(new Point3(0f, 0f, 3f), 1f);

            Assert.That(ray.Intersects(sphere, out float distance), Is.True);
            Assert.That(distance, Is.EqualTo(7f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_ReturnsExpectedDistance()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(box, out float distance), Is.True);
            Assert.That(distance, Is.EqualTo(4f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Cube")]
        public void Ray_IntersectsCube_ReturnsExpectedDistance()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var cube = new Cube(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(cube, out float distance), Is.True);
            Assert.That(distance, Is.EqualTo(4f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Plane3")]
        public void Ray_IntersectsPlane_ReturnsExpectedDistance()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var plane = new Plane3(new Vector3(0f, 0f, 1f), 0f);

            Assert.That(ray.Intersects(plane, out float distance), Is.True);
            Assert.That(distance, Is.EqualTo(5f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Sphere")]
        public void Ray_IntersectsSphere_Miss_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var farSphere = new Sphere(new Point3(10f, 10f, 10f), 1f);

            Assert.That(ray.Intersects(farSphere, out float distance), Is.False);
            Assert.That(distance, Is.EqualTo(0f));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Sphere")]
        public void Ray_IntersectsSphere_OriginInsideSphere_Pass()
        {
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 1f));
            var enclosing = new Sphere(new Point3(0f, 0f, 0f), 5f);

            Assert.That(ray.Intersects(enclosing, out float distance), Is.True);
            Assert.That(distance, Is.EqualTo(5f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Sphere")]
        public void Ray_IntersectsSphere_BehindRay_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 1f));
            var behind = new Sphere(new Point3(0f, 0f, -5f), 1f);

            Assert.That(ray.Intersects(behind, out float distance), Is.False);
            Assert.That(distance, Is.EqualTo(0f));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_Miss_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var farBox = new AABB(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f));

            Assert.That(ray.Intersects(farBox, out float distance), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_ParallelAxes_Pass()
        {
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(box, out float distance), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_ParallelAxesOutsideSlab_Fail()
        {
            var ray = new Ray(new Point3(0f, 5f, 0f), new Vector3(1f, 0f, 0f));
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(box, out float distance), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Plane3")]
        public void Ray_IntersectsPlane_Parallel_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(1f, 0f, 0f));
            var plane = new Plane3(new Vector3(0f, 0f, 1f), 0f);

            Assert.That(ray.Intersects(plane, out float distance), Is.False);
            Assert.That(distance, Is.EqualTo(0f));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Plane3")]
        public void Ray_IntersectsPlane_BehindRay_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, -1f));
            var plane = new Plane3(new Vector3(0f, 0f, 1f), 0f);

            Assert.That(ray.Intersects(plane, out float distance), Is.False);
        }
    }
}
