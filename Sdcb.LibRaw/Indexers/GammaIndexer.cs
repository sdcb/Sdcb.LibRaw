using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Indexers;

internal class GammaIndexer : IIndexer<float>
{
    private readonly IntPtr _r;
    private bool _disposed;

    public GammaIndexer(IntPtr r, bool disposed)
    {
        _r = r;
        _disposed = disposed;
    }

    /// <seealso cref="LibRawNative.SetGamma(IntPtr, int, float)"/> 
    /// <seealso cref="LibRawData"/>
    public float this[int index]
    {
        get
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            LibRawData data = Marshal.PtrToStructure<LibRawData>(_r);
            return (float)data.OutputParams.Gamma[index];
        }
        set
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            LibRawNative.SetGamma(_r, index, value);
        }
    }

    public int Count => 6;

    public IEnumerator<float> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RawContext));
    }
}