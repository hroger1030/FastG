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
using System;

namespace FastGTests
{
    [TestFixture]
    public class CapsuleTests
    {
        [Test]
        [Category("Capsule")]
        public void Capsule_RawFloatConstructor_Pass()
        {
            var fromFloats = new Capsule(0f, 0f, 0f, 0f, 0f, 2f, 1f);
            var fromPoints = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            Assert.That(fromFloats, Is.EqualTo(fromPoints));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Capsule(0f, 0f, 0f, 0f, 0f, 2f, -1f));
        }

        [Test]
        [Category("Capsule")]
        public void Capsule_EqualEndpoints_Fail()
        {
            Assert.Throws<ArgumentException>(() => new Capsule(new Point3(1f, 1f, 1f), new Point3(1f, 1f, 1f), 1f));
        }

        [Test]
        [Category("Capsule")]
        public void Capsule_VolumeAndSurfaceArea_Pass()
        {
            // cylinder body of height 2 plus a unit sphere from the two hemispherical caps
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 2f), 1f);

            float expectedVolume = (MathF.PI * 1f * 1f * 2f) + ((4f / 3f) * MathF.PI * 1f * 1f * 1f);
            float expectedSurfaceArea = (2f * MathF.PI * 1f * 2f) + (4f * MathF.PI * 1f * 1f);

            Assert.That(capsule.Volume, Is.EqualTo(expectedVolume).Within(1e-4f));
            Assert.That(capsule.SurfaceArea, Is.EqualTo(expectedSurfaceArea).Within(1e-4f));

            I3d asI3d = capsule;
            Assert.That(asI3d.Volume, Is.EqualTo(expectedVolume).Within(1e-4f));
            Assert.That(asI3d.SurfaceArea, Is.EqualTo(expectedSurfaceArea).Within(1e-4f));
        }

        [Test]
        [Category("Capsule")]
        [TestCase(2f)]
        [TestCase(0.5f)]
        public void Capsule_Scale_Pass(float scale)
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);
            var scaled = capsule.Scale(scale);

            Assert.That(scaled.Radius, Is.EqualTo(1f * scale).Within(Constants.FLOAT_ERROR_MARGIN));

            var originalHeight = new Vector3(capsule.PointA, capsule.PointB).Length;
            var scaledHeight = new Vector3(scaled.PointA, scaled.PointB).Length;
            Assert.That(scaledHeight, Is.EqualTo(originalHeight * scale).Within(Constants.FLOAT_ERROR_MARGIN));

            // the midpoint - the center it scales about - never moves
            var originalMid = new Point3((capsule.PointA.X + capsule.PointB.X) / 2f, (capsule.PointA.Y + capsule.PointB.Y) / 2f, (capsule.PointA.Z + capsule.PointB.Z) / 2f);
            var scaledMid = new Point3((scaled.PointA.X + scaled.PointB.X) / 2f, (scaled.PointA.Y + scaled.PointB.Y) / 2f, (scaled.PointA.Z + scaled.PointB.Z) / 2f);
            Assert.That(scaledMid.Z, Is.EqualTo(originalMid.Z).Within(Constants.FLOAT_ERROR_MARGIN));
        }

        [Test]
        [Category("Capsule")]
        [TestCase(0f)]
        [TestCase(-1f)]
        public void Capsule_Scale_ThrowsForNonPositiveScale_Fail(float scale)
        {
            var capsule = new Capsule(new Point3(0f, 0f, 0f), new Point3(0f, 0f, 4f), 1f);

            Assert.Throws<ArgumentOutOfRangeException>(() => capsule.Scale(scale));
        }

        [Test]
        [Category("Capsule")]
        public void Capsule_ToString_Pass()
        {
            var capsule = new Capsule(0f, 0f, 0f, 0f, 0f, 2f, 1f);

            Assert.That(capsule.ToString(), Is.EqualTo("Capsule(PointA: (0, 0, 0), PointB: (0, 0, 2), Radius: 1)"));
        }
    }
}


