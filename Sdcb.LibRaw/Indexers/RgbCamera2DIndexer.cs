﻿using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sdcb.LibRaw.Indexers;

internal class RgbCamera2DIndexer : IReadOnly2DIndexer<float>
{
    private readonly IntPtr _r;
    private readonly bool _disposed;

    public RgbCamera2DIndexer(IntPtr r, bool disposed)
    {
        _r = r;
        _disposed = disposed;
    }

    /// <seealso cref="LibRawNative.GetRgbCameraMatrix(IntPtr, int, int)"/>
    public float this[int index]
    {
        get
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            int x = index % Width;
            int y = index / Width;
            return LibRawNative.GetRgbCameraMatrix(_r, y, x);
        }
    }

    /// <seealso cref="LibRawNative.GetRgbCameraMatrix(IntPtr, int, int)"/>
    public float this[int y, int x]
    {
        get
        {
            CheckDisposed();
            if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), "Index is out of range.");
            if (y < 0 || y >= Width) throw new ArgumentOutOfRangeException(nameof(x), "Index is out of range.");

            return LibRawNative.GetRgbCameraMatrix(_r, y, x);
        }
    }

    public int Count => Width * Height;
    public int Width => 4;
    public int Height => 3;

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