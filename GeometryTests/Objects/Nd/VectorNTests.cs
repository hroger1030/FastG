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
    public class VectorNTests
    {
        [Test]
        [Category("VectorN")]
        public void VectorN_OperatorAndEquality_Pass()
        {
            var v1 = new VectorN(3);
            v1.Axis[0] = 1f;
            v1.Axis[1] = 2f;
            v1.Axis[2] = 3f;

            var v2 = new VectorN(3);
            v2.Axis[0] = 3f;
            v2.Axis[1] = 2f;
            v2.Axis[2] = 1f;

            var sum = v1 + v2;
            Assert.That(sum.Axis[0], Is.EqualTo(4f));
            Assert.That(sum.Axis[1], Is.EqualTo(4f));
            Assert.That(sum.Axis[2], Is.EqualTo(4f));

            var diff = v1 - v2;
            Assert.That(diff.Axis[0], Is.EqualTo(-2f));
            Assert.That(diff.Axis[2], Is.EqualTo(2f));

            Assert.That((v1 * 2f).Axis[0], Is.EqualTo(2f));
            Assert.That((v1 / 2f).Axis[0], Is.EqualTo(0.5f));
            Assert.That(v1.Equals(new VectorN(v1)), Is.True);
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_DifferentOrder_Fail()
        {
            var v1 = new VectorN(2);
            var v2 = new VectorN(3);

            Assert.Throws<InvalidOperationException>(() => { var _ = v1 + v2; });
            Assert.Throws<InvalidOperationException>(() => { var _ = v1 - v2; });
            Assert.Throws<InvalidOperationException>(() => VectorN.Dot(v1, v2));
            Assert.Throws<InvalidOperationException>(() => v1.Dot(v2));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_Dot_Pass()
        {
            var v1 = new VectorN(4) { Axis = [1f, 2f, 3f, 4f] };
            var v2 = new VectorN(4) { Axis = [5f, -6f, 7f, 8f] };

            Assert.That(VectorN.Dot(v1, v2), Is.EqualTo(46f));
            Assert.That(v1.Dot(v2), Is.EqualTo(46f));
            Assert.That(v1.Dot(v1), Is.EqualTo(30f));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_EqualsHashCodeAndOperators_Pass()
        {
            var v1 = new VectorN(3) { Axis = [1f, 2f, 3f] };
            var same = new VectorN(3) { Axis = [1f, 2f, 3f] };
            var different = new VectorN(3) { Axis = [1f, 2f, 4f] };
            var shorter = new VectorN(2) { Axis = [1f, 2f] };

            Assert.That(v1.Equals(same), Is.True);
            Assert.That(v1.Equals(different), Is.False);
            Assert.That(v1.Equals(shorter), Is.False);
            Assert.That(v1.Equals(null), Is.False);
            Assert.That(v1.Equals((object)same), Is.True);
            Assert.That(v1.Equals((object)null), Is.False);
            Assert.That(v1.Equals("not a vector"), Is.False);

            // equal vectors must produce equal hash codes
            Assert.That(v1.GetHashCode(), Is.EqualTo(same.GetHashCode()));

            Assert.That(v1 == same, Is.True);
            Assert.That(v1 != different, Is.True);
            Assert.That((VectorN)null == (VectorN)null, Is.True);
            Assert.That(v1 == null, Is.False);
            Assert.That(null == v1, Is.False);
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_CopyConstructorAndToString_Pass()
        {
            var original = new VectorN(3) { Axis = [1f, 2f, 3f] };
            var copy = new VectorN(original);

            Assert.That(copy, Is.EqualTo(original));
            Assert.That(copy.Axis, Is.Not.SameAs(original.Axis));

            Assert.That(original.ToString(), Is.EqualTo("VectorN[1, 2, 3]"));
        }
    }
}



