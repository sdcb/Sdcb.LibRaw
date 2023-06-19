using System.Collections.Generic;

namespace Sdcb.LibRaw;

public interface IIndexer<T> : IReadOnlyList<T>
{
    new T this[int index] { get; set; }
}

public interface IReadOnly2DIndexer<T> : IReadOnlyList<T>
{
    T this[int y, int x] { get; }
    int Width { get; }
    int Height { get; }
}