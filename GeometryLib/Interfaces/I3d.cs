namespace Geometry
{
    /// <summary>
    /// Implemented by three-dimensional shapes that have a measurable volume and surface area.
    /// </summary>
    public interface I3d
    {
        /// <summary>
        /// The volume enclosed by the shape.
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// The total area of the shape's surface.
        /// </summary>
        float SurfaceArea { get; }
    }
}
