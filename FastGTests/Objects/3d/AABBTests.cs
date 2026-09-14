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
    public class AABBTests
    {
        [Test]
        [Category("AABB")]
        public void AABB_VolumeAndSurfaceArea_Pass()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));
            Assert.That(box.Volume, Is.EqualTo(8f));
            Assert.That(box.SurfaceArea, Is.EqualTo(24f));
        }

        [Test]
        [Category("AABB")]
        public void AABB_InvalidBounds_Fail()
        {
            Assert.Throws<ArgumentException>(() => new AABB(new Point3(1f, 1f, 1f), new Point3(0f, 0f, 0f)));
        }

        [Test]
        [Category("AABB")]
        public void AABB_UnitAABB_Pass()
        {
            Assert.That(AABB.UNIT_AABB.Min, Is.EqualTo(new Point3(0f, 0f, 0f)));
            Assert.That(AABB.UNIT_AABB.Max, Is.EqualTo(new Point3(1f, 1f, 1f)));
            Assert.That(AABB.UNIT_AABB.Volume, Is.EqualTo(1f));
        }

        [Test]
        [Category("AABB")]
        public void AABB_RawFloatConstructor_Pass()
        {
            var fromFloats = new AABB(0f, 0f, 0f, 1f, 2f, 3f);
            var fromPoints = new AABB(new Point3(0f, 0f, 0f), new Point3(1f, 2f, 3f));

            Assert.That(fromFloats, Is.EqualTo(fromPoints));
            Assert.Throws<ArgumentException>(() => new AABB(0f, 0f, 0f, -1f, 1f, 1f));
        }

        [Test]
        [Category("AABB")]
        public void AABB_Scale_Pass()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));
            var scaled = box.Scale(2f);

            Assert.That(scaled.Min, Is.EqualTo(new Point3(-1f, -1f, -1f)));
            Assert.That(scaled.Max, Is.EqualTo(new Point3(3f, 3f, 3f)));
        }

        [Test]
        [Category("AABB")]
        public void AABB_Scale_ThrowsForNonPositiveScale_Fail()
        {
            var box = new AABB(new Point3(0f, 0f, 0f), new Point3(2f, 2f, 2f));

            Assert.Throws<ArgumentOutOfRangeException>(() => box.Scale(0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => box.Scale(-1f));
        }

        [Test]
        [Category("AABB")]
        public void AABB_ToString_Pass()
        {
            var box = new AABB(0f, 0f, 0f, 1f, 2f, 3f);

            Assert.That(box.ToString(), Is.EqualTo("AABB(Min: (0, 0, 0), Max: (1, 2, 3))"));
        }
    }
}



