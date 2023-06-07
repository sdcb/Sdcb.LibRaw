using System.Collections.Generic;

namespace Sdcb.LibRaw;

public interface IIndexer<T> : IReadOnlyList<T>
{
    new T this[int index] { get; set; }
}
