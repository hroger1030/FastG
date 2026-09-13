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
            var v1 = new VectorN([1f, 2f, 3f]);
            var v2 = new VectorN([3f, 2f, 1f]);

            var sum = v1 + v2;
            Assert.That(sum.Axis[0], Is.EqualTo(4f));
            Assert.That(sum.Axis[1], Is.EqualTo(4f));
            Assert.That(sum.Axis[2], Is.EqualTo(4f));

            var diff = v1 - v2;
            Assert.That(diff.Axis[0], Is.EqualTo(-2f));
            Assert.That(diff.Axis[2], Is.EqualTo(2f));

            Assert.That((v1 * 2f).Axis[0], Is.EqualTo(2f));
            Assert.That((v1 / 2f).Axis[0], Is.EqualTo(0.5f));
            Assert.That(v1.Equals(new VectorN([1f, 2f, 3f])), Is.True);
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_Zero_Pass()
        {
            var zero = VectorN.Zero(3);

            Assert.That(zero.Axis.Length, Is.EqualTo(3));
            Assert.That(zero.Axis[0], Is.EqualTo(0f));
            Assert.That(zero.Axis[1], Is.EqualTo(0f));
            Assert.That(zero.Axis[2], Is.EqualTo(0f));

            var v = new VectorN([1f, 2f, 3f]);
            Assert.That((v + VectorN.Zero(3)).Equals(v), Is.True);
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_ZeroInvalidLength_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => VectorN.Zero(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => VectorN.Zero(-1));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_NullConstructor_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new VectorN((float[])null));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_EmptyConstructor_Fail()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VectorN([]));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_DifferentOrder_Fail()
        {
            var v1 = VectorN.Zero(2);
            var v2 = VectorN.Zero(3);

            Assert.Throws<InvalidOperationException>(() => { var _ = v1 + v2; });
            Assert.Throws<InvalidOperationException>(() => { var _ = v1 - v2; });
            Assert.Throws<InvalidOperationException>(() => VectorN.Dot(v1, v2));
            Assert.Throws<InvalidOperationException>(() => v1.Dot(v2));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_Dot_Pass()
        {
            var v1 = new VectorN([1f, 2f, 3f, 4f]);
            var v2 = new VectorN([5f, -6f, 7f, 8f]);

            Assert.That(VectorN.Dot(v1, v2), Is.EqualTo(46f));
            Assert.That(v1.Dot(v2), Is.EqualTo(46f));
            Assert.That(v1.Dot(v1), Is.EqualTo(30f));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_EqualsHashCodeAndOperators_Pass()
        {
            var v1 = new VectorN([1f, 2f, 3f]);
            var same = new VectorN([1f, 2f, 3f]);
            var different = new VectorN([1f, 2f, 4f]);
            var shorter = new VectorN([1f, 2f]);

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
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_ConstructorCopiesInputArray_Pass()
        {
            // VectorN is an immutable value type: the constructor must copy its input, so mutating the
            // caller's array afterward cannot leak through and change an already-constructed VectorN.
            var source = new float[] { 1f, 2f, 3f };
            var vector = new VectorN(source);

            source[0] = 99f;

            Assert.That(vector.Axis[0], Is.EqualTo(1f));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_StructAssignment_CopiesTheValue_Pass()
        {
            var original = new VectorN([1f, 2f, 3f]);
            var copy = original;

            Assert.That(copy, Is.EqualTo(original));
            Assert.That(copy.Axis.Length, Is.EqualTo(3));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_ToString_Pass()
        {
            var vector = new VectorN([1f, 2f, 3f]);

            Assert.That(vector.ToString(), Is.EqualTo("VectorN[1, 2, 3]"));
        }

        [Test]
        [Category("VectorN")]
        public void VectorN_Default_ThrowsOnUse_Fail()
        {
            // VectorN has no parameterless constructor, but a struct's default(...) state can't be routed
            // through any constructor - it bypasses the "at least one axis" guard entirely. Rather than covering
            // for that with a graceful fallback, Axis reuses the same guards as the constructor and throws the
            // moment anyone touches it.
            Assert.Throws<ArgumentNullException>(() => { var _ = default(VectorN).Axis; });
        }
    }
}
