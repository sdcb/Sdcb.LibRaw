using System.Collections.Generic;

namespace Sdcb.LibRaw;

/// <summary>
/// Interface for creating a generic indexer data structure that supports read and write access.
/// </summary>
/// <typeparam name="T">The type of the elements in the indexer.</typeparam>
public interface IIndexer<T> : IReadOnlyList<T>
{
    /// <summary>
    /// Indexer for accessing or updating an element at a specific index in the indexer.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    new T this[int index] { get; set; }
}
