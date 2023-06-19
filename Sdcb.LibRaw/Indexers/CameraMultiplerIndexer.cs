using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Indexers;

internal class CameraMultiplerIndexer : IReadOnlyList<float>
{
    private readonly IntPtr _r;
    private readonly bool _disposed;

    public CameraMultiplerIndexer(IntPtr r, bool disposed)
    {
        _r = r;
        _disposed = disposed;
    }

    /// <summary>Gets the white balance coefficients for the camera.</summary>
    /// <seealso cref="LibRawNative.GetCameraMultiplier(IntPtr, int)"/> 
    public float this[int index]
    {
        get
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            return LibRawNative.GetCameraMultiplier(_r, index);
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