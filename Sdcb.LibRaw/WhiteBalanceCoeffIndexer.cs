using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;

public class WhiteBalanceCoeffIndexer : IReadOnlyList<float>
{
    private readonly IntPtr _r;

    public WhiteBalanceCoeffIndexer(IntPtr r)
    {
        _r = r;
    }

    /// <summary>Gets or sets the white balance coefficients for the camera.</summary>
    /// <seealso cref="LibRawNative.GetCameraMultiplier(IntPtr, int)"/> 
    /// <seealso cref="LibRawData"/>
    public float this[int index]
    {
        get
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            return LibRawNative.GetCameraMultiplier(_r, index);
        }
        set
        {
            CheckDisposed();
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            LibRawData data = Marshal.PtrToStructure<LibRawData>(_r);
            data.ColorData.CamMul[index] = value;
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
        // 之前的 CheckDisposed 方法的实现应该放在这里
    }
}