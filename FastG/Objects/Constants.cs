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

namespace FastG
{
    public static class Constants
    {
        /// <summary>
        /// Float math can be a little inaccurate, so this is the margin of error we will use when comparing floats.
        /// </summary>
        public const float FLOAT_ERROR_MARGIN = 1e-6f;

        /// <summary>
        /// Ratio of a circle's circumference to its diameter. Same value as <see cref="MathF.PI"/>, provided here for convenience.
        /// </summary>
        public const float PI = 3.14159265f;

        /// <summary>
        /// A full turn, in radians (360 degrees). Useful for wrapping angles and full-circle rotations.
        /// </summary>
        public const float TWO_PI = 6.283185307f;

        /// <summary>
        /// Alternate alias for <see cref="TWO_PI"/>, should the user prefer that name.
        /// </summary>
        public const float TAU = 6.283185307f;

        /// <summary>
        /// A quarter turn, in radians (90 degrees). Half of <see cref="PI"/>.
        /// </summary>
        public const float HALF_PI = PI * 0.5f;

        /// <summary>
        /// An eighth of a turn, in radians (45 degrees). Quarter of <see cref="PI"/>.
        /// </summary>
        public const float QUARTER_PI = PI * 0.25f;

        /// <summary>
        /// Multiply a degree value by this to convert it to radians.
        /// </summary>
        public const float DEG_TO_RAD = PI / 180f;

        /// <summary>
        /// Multiply a radian value by this to convert it to degrees.
        /// </summary>
        public const float RAD_TO_DEG = 180f / PI;

        /// <summary>
        /// The square root of 2. Commonly used for diagonal distances/movement, e.g. on a grid.
        /// </summary>
        public const float SQRT_2 = 1.41421356237f;

        /// <summary>
        /// The square root of 3. Shows up in equilateral triangle and hexagon math (e.g. isometric/hex grids).
        /// </summary>
        public const float SQRT_3 = 1.73205080757f;

        // Precomputed inverses below. A multiply is cheaper than a divide on most hardware, so prefer
        // "value * INV_X" over "value / X" on hot paths (normalizing angles, per-frame trig, etc).

        /// <summary>
        /// 1 / PI. Multiply by this instead of dividing by <see cref="PI"/>.
        /// </summary>
        public const float INV_PI = 1f / PI;

        /// <summary>
        /// 1 / (2 * PI). Multiply by this instead of dividing by <see cref="TWO_PI"/>. Handy for wrapping an angle into [0, 1) turns.
        /// </summary>
        public const float INV_TWO_PI = 1f / TWO_PI;

        /// <summary>
        /// 1 / (PI / 2), i.e. 2 / PI. Multiply by this instead of dividing by <see cref="HALF_PI"/>.
        /// </summary>
        public const float INV_HALF_PI = 1f / HALF_PI;

        /// <summary>
        /// 1 / sqrt(2). Commonly used to normalize diagonal movement/vectors (e.g. 8-directional grid movement).
        /// </summary>
        public const float INV_SQRT_2 = 1f / SQRT_2;

        /// <summary>
        /// 1 / sqrt(3). Inverse of <see cref="SQRT_3"/>, for the same hex/triangle math without a divide.
        /// </summary>
        public const float INV_SQRT_3 = 1f / SQRT_3;
    }
}
