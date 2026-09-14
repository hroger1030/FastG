namespace FastG
{
    /// <summary>
    /// Implemented by one-dimensional shapes (e.g. line segments) that have a measurable length.
    /// </summary>
    public interface I1d
    {
        /// <summary>
        /// The length of the shape.
        /// </summary>
        float Length { get; }
    }
}
