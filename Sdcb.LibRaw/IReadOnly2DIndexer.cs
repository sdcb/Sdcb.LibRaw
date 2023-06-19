using System.Collections.Generic;

namespace Sdcb.LibRaw;

/// <summary>
/// An interface for a read-only two-dimensional indexer.
/// </summary>
/// <typeparam name="T">The type of element in the two-dimensional indexer.</typeparam>
public interface IReadOnly2DIndexer<T> : IReadOnlyList<T>
{
    /// <summary>
    /// Gets the element at the specified coordinates in the two-dimensional indexer. 
    /// </summary>
    /// <param name="x">The x-coordinate of the element to get.</param>
    /// <param name="y">The y-coordinate of the element to get.</param>
    /// <returns>The element at the specified coordinates.</returns>
    T this[int y, int x] { get; }

    /// <summary>
    /// Gets the width of the two-dimensional indexer.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Gets the height of the two-dimensional indexer.
    /// </summary>
    int Height { get; }
}
