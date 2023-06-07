using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Indexers;

internal class PreMultiplerIndexer : IIndexer<float>
{
    private readonly IntPtr _r;
    private bool _disposed;

    public PreMultiplerIndexer(IntPtr r, bool disposed)
    {
        _r = r;
        _disposed = disposed;
    }

    /// <summary>Gets or sets the white balance coefficients for the camera.</summary>
    /// <seealso cref="LibRawNative.GetPreMultiplier(IntPtr, int)"/> 
    /// <seealso cref="LibRawData"/>
    public float this[int index]
    {
        get
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            return LibRawNative.GetPreMultiplier(_r, index);
        }
        set
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            LibRawData data = Marshal.PtrToStructure<LibRawData>(_r);
            data.ColorData.PreMul[index] = value;
            Marshal.StructureToPtr(data, _r, fDeleteOld: false);
        }
    }

    public int Count => 4;

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