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

using FastG;
using NUnit.Framework;

namespace FastGTests
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
        [Category("AABB")]
        [Category("Sphere")]
        public void AABB_IntersectsSphere_Pass()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));
            var centerInside = new Sphere(new Point3(1f, 1f, 1f), 1f);
            var touching = new Sphere(new Point3(3f, 1f, 1f), 1.5f);
            var separate = new Sphere(new Point3(5f, 5f, 5f), 1f);

            Assert.That(box.Intersects(centerInside), Is.True);
            Assert.That(box.Intersects(touching), Is.True);
            Assert.That(box.Intersects(separate), Is.False);

            // both forwarders (AABB.Intersects(Sphere) and Sphere.Intersects(AABB)) must agree
            Assert.That(centerInside.Intersects(box), Is.True);
            Assert.That(separate.Intersects(box), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Sphere")]
        [Category("AABB")]
        public void Sphere_ContainsAABB_Pass()
        {
            var bigSphere = new Sphere(new Point3(0f, 0f, 0f), 10f);
            var smallSphere = new Sphere(new Point3(0f, 0f, 0f), 1f);
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(bigSphere.Contains(box), Is.True);
            Assert.That(smallSphere.Contains(box), Is.False); // box corners stick out past the sphere
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        [Category("Cube")]
        public void AABB_IntersectsCube_Pass()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));
            var overlapping = new Cube(1f, 1f, 1f, 3f, 3f, 3f);
            var separate = new Cube(10f, 10f, 10f, 11f, 11f, 11f);

            Assert.That(box.Intersects(overlapping), Is.True);
            Assert.That(box.Intersects(separate), Is.False);

            // both forwarders must agree
            Assert.That(overlapping.Intersects(box), Is.True);
            Assert.That(separate.Intersects(box), Is.False);
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
        [Category("Cylinder")]
        public void Cylinder_Contains_Pass()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            // inside the body, on the axis
            Assert.That(cylinder.Contains(new Point3(0f, 0f, 1f)), Is.True);

            // inside the body, off the axis but within the radius
            Assert.That(cylinder.Contains(new Point3(0.5f, 0.5f, 1f)), Is.True);

            // exactly on the curved surface at the midpoint of the axis
            Assert.That(cylinder.Contains(new Point3(1f, 0f, 1f)), Is.True);

            // outside the radius
            Assert.That(cylinder.Contains(new Point3(0f, 2f, 1f)), Is.False);

            // beyond the flat end caps, even though it's on the axis (this is the key difference from Capsule,
            // which would treat the same point as inside its rounded hemispherical cap)
            Assert.That(cylinder.Contains(new Point3(0f, 0f, -0.1f)), Is.False);
            Assert.That(cylinder.Contains(new Point3(0f, 0f, 2.1f)), Is.False);

            // exactly on a flat end cap, within the radius
            Assert.That(cylinder.Contains(new Point3(0.5f, 0f, 0f)), Is.True);
            Assert.That(cylinder.Contains(new Point3(0.5f, 0f, 2f)), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cylinder")]
        public void Cylinder_IntersectsSphere_Pass()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);

            // overlapping the lateral body
            Assert.That(cylinder.Intersects(new Sphere(new Point3(2f, 0f, 2f), 1.5f)), Is.True);

            // far from the lateral body
            Assert.That(cylinder.Intersects(new Sphere(new Point3(5f, 0f, 2f), 0.5f)), Is.False);

            // beyond the flat top cap, overlapping the disk's rim (radial excess and axial overshoot combined)
            Assert.That(cylinder.Intersects(new Sphere(new Point3(1.5f, 0f, 4.5f), 0.8f)), Is.True);
            Assert.That(cylinder.Intersects(new Sphere(new Point3(1.5f, 0f, 4.5f), 0.5f)), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        [Category("Cylinder")]
        public void AABB_IntersectsCylinder_Pass()
        {
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));
            var throughTheBox = new Cylinder(new Point3(-5f, 0f, 0f), new Point3(5f, 0f, 0f), 0.5f);
            var farAway = new Cylinder(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f), 0.5f);

            Assert.That(box.Intersects(throughTheBox), Is.True);
            Assert.That(box.Intersects(farAway), Is.False);

            // both forwarders must agree
            Assert.That(throughTheBox.Intersects(box), Is.True);
            Assert.That(farAway.Intersects(box), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        [Category("Cylinder")]
        public void AABB_IntersectsCylinder_DiagonalCylinder_Pass()
        {
            // exercises the sqrt(1 - direction^2) per-axis padding for a non-axis-aligned cylinder
            var diagonalCylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 10f, 10f), 1f);

            var overlapping = new AABB(new Point3(-0.5f, 4f, 4f), new Point3(0.5f, 6f, 6f)); // straddles the segment's midpoint
            var farAway = new AABB(new Point3(50f, 50f, 50f), new Point3(51f, 51f, 51f));

            Assert.That(overlapping.Intersects(diagonalCylinder), Is.True);
            Assert.That(farAway.Intersects(diagonalCylinder), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cylinder")]
        [Category("Cube")]
        public void Cylinder_IntersectsCube_Pass()
        {
            var cube = new Cube(-1f, -1f, -1f, 1f, 1f, 1f);
            var throughTheCube = new Cylinder(new Point3(-5f, 0f, 0f), new Point3(5f, 0f, 0f), 0.5f);
            var farAway = new Cylinder(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f), 0.5f);

            Assert.That(throughTheCube.Intersects(cube), Is.True);
            Assert.That(farAway.Intersects(cube), Is.False);

            // both forwarders must agree
            Assert.That(cube.Intersects(throughTheCube), Is.True);
            Assert.That(cube.Intersects(farAway), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cylinder")]
        public void Cylinder_IntersectsCylinder_Crossing_Pass()
        {
            var a = new Cylinder(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Cylinder(new Point3(2f, -5f, 0f), new Point3(2f, 5f, 0f), 1f);

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cylinder")]
        public void Cylinder_IntersectsCylinder_ParallelOverlapping_Pass()
        {
            var a = new Cylinder(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Cylinder(new Point3(0f, 1.5f, 0f), new Point3(4f, 1.5f, 0f), 1f);

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Cylinder")]
        public void Cylinder_IntersectsCylinder_Separate_Fail()
        {
            var a = new Cylinder(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Cylinder(new Point3(100f, 100f, 100f), new Point3(101f, 101f, 101f), 0.5f);

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        [Category("Capsule")]
        public void AABB_IntersectsCapsule_Pass()
        {
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));
            var throughTheBox = new Capsule(new Point3(-5f, 0f, 0f), new Point3(5f, 0f, 0f), 0.5f);
            var farAway = new Capsule(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f), 0.5f);

            Assert.That(box.Intersects(throughTheBox), Is.True);
            Assert.That(box.Intersects(farAway), Is.False);

            // both forwarders must agree
            Assert.That(throughTheBox.Intersects(box), Is.True);
            Assert.That(farAway.Intersects(box), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("AABB")]
        [Category("Capsule")]
        public void AABB_IntersectsCapsule_DiagonalCapsule_Pass()
        {
            // exercises the sqrt(1 - direction^2) per-axis padding for a non-axis-aligned capsule
            var diagonalCapsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 10f, 10f), 1f);

            var overlapping = new AABB(new Point3(-0.5f, 4f, 4f), new Point3(0.5f, 6f, 6f)); // straddles the segment's midpoint
            var farAway = new AABB(new Point3(50f, 50f, 50f), new Point3(51f, 51f, 51f));

            Assert.That(overlapping.Intersects(diagonalCapsule), Is.True);
            Assert.That(farAway.Intersects(diagonalCapsule), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Capsule")]
        [Category("Cube")]
        public void Capsule_IntersectsCube_Pass()
        {
            var cube = new Cube(-1f, -1f, -1f, 1f, 1f, 1f);
            var throughTheCube = new Capsule(new Point3(-5f, 0f, 0f), new Point3(5f, 0f, 0f), 0.5f);
            var farAway = new Capsule(new Point3(10f, 10f, 10f), new Point3(11f, 11f, 11f), 0.5f);

            Assert.That(throughTheCube.Intersects(cube), Is.True);
            Assert.That(farAway.Intersects(cube), Is.False);

            // both forwarders must agree
            Assert.That(cube.Intersects(throughTheCube), Is.True);
            Assert.That(cube.Intersects(farAway), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Capsule")]
        public void Capsule_IntersectsCapsule_Crossing_Pass()
        {
            var a = new Capsule(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Capsule(new Point3(2f, -5f, 0f), new Point3(2f, 5f, 0f), 1f);

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Capsule")]
        public void Capsule_IntersectsCapsule_ParallelOverlapping_Pass()
        {
            var a = new Capsule(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Capsule(new Point3(0f, 1.5f, 0f), new Point3(4f, 1.5f, 0f), 1f);

            Assert.That(a.Intersects(b), Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Capsule")]
        public void Capsule_IntersectsCapsule_Separate_Fail()
        {
            var a = new Capsule(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), 1f);
            var b = new Capsule(new Point3(100f, 100f, 100f), new Point3(101f, 101f, 101f), 0.5f);

            Assert.That(a.Intersects(b), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Sphere")]
        public void Ray_IntersectsSphere_ReturnsExpectedDistance()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(0f, 0f, 1f));
            var sphere = new Sphere(new Point3(0f, 0f, 3f), 1f);

            var (hit, distance) = ray.Intersects(sphere);
            Assert.That(hit, Is.True);
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

            var (hit, distance) = ray.Intersects(box);
            Assert.That(hit, Is.True);
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

            var (hit, distance) = ray.Intersects(cube);
            Assert.That(hit, Is.True);
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

            var (hit, distance) = ray.Intersects(plane);
            Assert.That(hit, Is.True);
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

            var (hit, distance) = ray.Intersects(farSphere);
            Assert.That(hit, Is.False);
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

            var (hit, distance) = ray.Intersects(enclosing);
            Assert.That(hit, Is.True);
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

            var (hit, distance) = ray.Intersects(behind);
            Assert.That(hit, Is.False);
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

            Assert.That(ray.Intersects(farBox).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_ParallelAxes_Pass()
        {
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(box).Hit, Is.True);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("AABB")]
        public void Ray_IntersectsAABB_ParallelAxesOutsideSlab_Fail()
        {
            var ray = new Ray(new Point3(0f, 5f, 0f), new Vector3(1f, 0f, 0f));
            var box = new AABB(new Point3(-1f, -1f, -1f), new Point3(1f, 1f, 1f));

            Assert.That(ray.Intersects(box).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Plane3")]
        public void Ray_IntersectsPlane_Parallel_Fail()
        {
            var ray = new Ray(new Point3(0f, 0f, -5f), new Vector3(1f, 0f, 0f));
            var plane = new Plane3(new Vector3(0f, 0f, 1f), 0f);

            var (hit, distance) = ray.Intersects(plane);
            Assert.That(hit, Is.False);
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

            Assert.That(ray.Intersects(plane).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Plane3")]
        [Category("Sphere")]
        public void Plane3_IntersectsSphere_Pass()
        {
            var plane = new Plane3(new Vector3(0f, 0f, 1f), 0f); // the z = 0 plane

            var crossing = new Sphere(new Point3(0f, 0f, 0f), 1f);
            var tangent = new Sphere(new Point3(0f, 0f, 1f), 1f);
            var farAway = new Sphere(new Point3(0f, 0f, 10f), 1f);

            Assert.That(plane.Intersects(crossing), Is.True);
            Assert.That(plane.Intersects(tangent), Is.True);
            Assert.That(plane.Intersects(farAway), Is.False);

            // both forwarders must agree
            Assert.That(crossing.Intersects(plane), Is.True);
            Assert.That(farAway.Intersects(plane), Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Triangle3")]
        [TestCase(1f, 1f, 0f, true)] // inside, coplanar
        [TestCase(10f, 10f, 0f, false)] // outside, coplanar
        [TestCase(2f, 0f, 0f, true)] // on an edge
        [TestCase(1f, 1f, 5f, false)] // not coplanar
        public void Triangle3_Contains_Pass(float x, float y, float z, bool expected)
        {
            var triangle = new Triangle3(new Point3(0f, 0f, 0f), new Point3(4f, 0f, 0f), new Point3(0f, 4f, 0f));

            Assert.That(triangle.Contains(new Point3(x, y, z)), Is.EqualTo(expected));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Triangle3")]
        public void Ray_IntersectsTriangle3_ReturnsExpectedDistance()
        {
            var triangle = new Triangle3(new Point3(-1f, -1f, 5f), new Point3(1f, -1f, 5f), new Point3(0f, 1f, 5f));

            var hit = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, 1f));
            var (didHit, distance) = hit.Intersects(triangle);
            Assert.That(didHit, Is.True);
            Assert.That(distance, Is.EqualTo(5f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Triangle3")]
        public void Ray_IntersectsTriangle3_BehindRay_Fail()
        {
            var triangle = new Triangle3(new Point3(-1f, -1f, 5f), new Point3(1f, -1f, 5f), new Point3(0f, 1f, 5f));
            var ray = new Ray(new Point3(0f, 0f, 0f), new Vector3(0f, 0f, -1f));

            Assert.That(ray.Intersects(triangle).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Triangle3")]
        public void Ray_IntersectsTriangle3_LateralMiss_Fail()
        {
            var triangle = new Triangle3(new Point3(-1f, -1f, 5f), new Point3(1f, -1f, 5f), new Point3(0f, 1f, 5f));
            var ray = new Ray(new Point3(10f, 10f, 0f), new Vector3(0f, 0f, 1f));

            Assert.That(ray.Intersects(triangle).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Triangle3")]
        public void Ray_IntersectsTriangle3_ParallelToPlane_Fail()
        {
            var triangle = new Triangle3(new Point3(-1f, -1f, 5f), new Point3(1f, -1f, 5f), new Point3(0f, 1f, 5f));
            var ray = new Ray(new Point3(0f, 0f, 5f), new Vector3(1f, 0f, 0f));

            Assert.That(ray.Intersects(triangle).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Capsule")]
        public void Ray_IntersectsCapsule_Body_ReturnsExpectedDistance()
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(5f, 0f, 2f), new Vector3(-1f, 0f, 0f));

            var (hit, distance) = ray.Intersects(capsule);
            Assert.That(hit, Is.True);
            Assert.That(distance, Is.EqualTo(4f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Capsule")]
        public void Ray_IntersectsCapsule_EndCap_ReturnsExpectedDistance()
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(0f, 0f, 10f), new Vector3(0f, 0f, -1f));

            var (hit, distance) = ray.Intersects(capsule);
            Assert.That(hit, Is.True);
            Assert.That(distance, Is.EqualTo(5f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Capsule")]
        public void Ray_IntersectsCapsule_Miss_Fail()
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(5f, 0f, 10f), new Vector3(-1f, 0f, 0f));

            Assert.That(ray.Intersects(capsule).Hit, Is.False);
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Cylinder")]
        public void Ray_IntersectsCylinder_Body_ReturnsExpectedDistance()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(5f, 0f, 2f), new Vector3(-1f, 0f, 0f));

            var (hit, distance) = ray.Intersects(cylinder);
            Assert.That(hit, Is.True);
            Assert.That(distance, Is.EqualTo(4f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Cylinder")]
        public void Ray_IntersectsCylinder_FlatEndCap_ReturnsExpectedDistance()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(0f, 0f, 10f), new Vector3(0f, 0f, -1f));

            // unlike the equivalent Capsule test, the cap is flat at z = 4 (not a hemisphere bulging to z = 5),
            // so the hit distance is 10 - 4 = 6, not 5.
            var (hit, distance) = ray.Intersects(cylinder);
            Assert.That(hit, Is.True);
            Assert.That(distance, Is.EqualTo(6f).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Collisions3d")]
        [Category("Ray")]
        [Category("Cylinder")]
        public void Ray_IntersectsCylinder_Miss_Fail()
        {
            var cylinder = new Cylinder(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var ray = new Ray(new Point3(5f, 0f, 10f), new Vector3(-1f, 0f, 0f));

            Assert.That(ray.Intersects(cylinder).Hit, Is.False);
        }
    }
}
