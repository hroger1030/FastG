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
using System;

namespace GeometryTests
{
    [TestFixture]
    public class CubeTests
    {
        [Test]
        [Category("Cube")]
        public void Cube_Indexer_OutOfRange_Fail()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            Assert.Throws<IndexOutOfRangeException>(() => { var point = cube[8]; });
        }

        [Test]
        [Category("Cube")]
        public void Cube_Indexer_Corners_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 2f, 3f);

            Assert.That(cube[0], Is.EqualTo(new Point3(0f, 0f, 0f)));
            Assert.That(cube[1], Is.EqualTo(new Point3(0f, 2f, 0f)));
            Assert.That(cube[2], Is.EqualTo(new Point3(1f, 0f, 0f)));
            Assert.That(cube[3], Is.EqualTo(new Point3(1f, 2f, 0f)));
            Assert.That(cube[4], Is.EqualTo(new Point3(0f, 0f, 3f)));
            Assert.That(cube[5], Is.EqualTo(new Point3(0f, 2f, 3f)));
            Assert.That(cube[6], Is.EqualTo(new Point3(1f, 0f, 3f)));
            Assert.That(cube[7], Is.EqualTo(new Point3(1f, 2f, 3f)));
        }

        [Test]
        [Category("Cube")]
        public void Cube_Dimensions_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 2f, 3f, 4f);

            Assert.That(cube.Width, Is.EqualTo(2f));
            Assert.That(cube.Height, Is.EqualTo(3f));
            Assert.That(cube.Depth, Is.EqualTo(4f));
            Assert.That(cube.Center, Is.EqualTo(new Point3(1f, 1.5f, 2f)));
            Assert.That(cube.Volume, Is.EqualTo(24f));
            Assert.That(cube.SurfaceArea, Is.EqualTo(2f * (2f * 3f) + 2f * (3f * 4f) + 2f * (4f * 2f)));
        }

        [Test]
        [Category("Cube")]
        public void Cube_UnitCube_Pass()
        {
            Assert.That(Cube.UNIT_CUBE.Width, Is.EqualTo(1f));
            Assert.That(Cube.UNIT_CUBE.Height, Is.EqualTo(1f));
            Assert.That(Cube.UNIT_CUBE.Depth, Is.EqualTo(1f));
        }

        [Test]
        [Category("Cube")]
        public void Cube_ScaleOperator_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            var scaled = cube * 2f;

            Assert.That(scaled.Width, Is.EqualTo(2f));
            Assert.That(scaled.Height, Is.EqualTo(2f));
            Assert.That(scaled.Depth, Is.EqualTo(2f));
            Assert.That(scaled.Center, Is.EqualTo(cube.Center), "Failed to stay centered on the same point");
        }

        [Test]
        [Category("Cube")]
        public void Cube_ScaleOperator_NegativeScale_Fail()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);

            Assert.Throws<ArgumentOutOfRangeException>(() => { var result = cube * -1f; });
        }

        [Test]
        [Category("Cube")]
        public void Cube_Scale_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            var scaled = cube.Scale(2f);

            Assert.That(scaled.Width, Is.EqualTo(2f));
            Assert.That(scaled.Height, Is.EqualTo(2f));
            Assert.That(scaled.Depth, Is.EqualTo(2f));
            Assert.That(scaled.Center, Is.EqualTo(cube.Center), "Failed to stay centered on the same point");
        }

        [Test]
        [Category("Cube")]
        public void Cube_Scale_ThrowsForNonPositiveScale_Fail()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);

            Assert.Throws<ArgumentOutOfRangeException>(() => cube.Scale(0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => cube.Scale(-1f));
        }

        [Test]
        [Category("Cube")]
        public void Cube_Equals_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            var same = new Cube(0f, 0f, 0f, 1f, 1f, 1f);
            var different = new Cube(0f, 0f, 0f, 2f, 1f, 1f);

            Assert.That(cube.Equals(cube), Is.True);
            Assert.That(cube.Equals(same), Is.True);
            Assert.That(cube.Equals(different), Is.False);

            Assert.That(cube.Equals((object)same), Is.True);
            Assert.That(cube.Equals(null), Is.False);
            Assert.That(cube.Equals("not a cube"), Is.False);

            Assert.That(cube.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        }

        [Test]
        [Category("Cube")]
        public void Cube_ToString_Pass()
        {
            var cube = new Cube(0f, 0f, 0f, 1f, 2f, 3f);

            Assert.That(cube.ToString(), Is.EqualTo("Cube(P1: (0, 0, 0), P2: (1, 2, 3))"));
        }
    }
}



