using System.Collections.Generic;

namespace Sdcb.LibRaw;

public interface IIndexer<T> : IReadOnlyList<T>
{
    new T this[int index] { get; set; }
}

public interface I2DIndexer<T> : IReadOnlyList<T>
{
    new T this[int index] { get; set; }
    T this[int y, int x] { get; set; }
    int Width { get; }
    int Height { get; }
}