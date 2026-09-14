namespace FastG
{
    /// <summary>
    /// Implemented by two-dimensional shapes that have a measurable perimeter and area.
    /// </summary>
    public interface I2d
    {
        /// <summary>
        /// The length of the shape's boundary.
        /// </summary>
        float Perimeter { get; }

        /// <summary>
        /// The area enclosed by the shape.
        /// </summary>
        float Area { get; }
    }
}
