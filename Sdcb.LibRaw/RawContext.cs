using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdcb.LibRaw;

/// <summary>
/// A C# wrapper for the Libraw C API, providing high-level features to work with raw images.
/// </summary>
public class RawContext : IDisposable
{
    private IntPtr _librawContext;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RawContext"/> class.
    /// </summary>
    public RawContext()
    {
        _librawContext = LibRawNative.Initialize(0);
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="RawContext"/> class.
    /// </summary>
    ~RawContext()
    {
        Dispose(false);
    }

    /// <summary>
    /// Gets the error message related to the error code.
    /// Corresponds to the C API function: libraw_strerror
    /// </summary>
    /// <param name="errorcode">The error code.</param>
    /// <returns>The error message.</returns>
    public static string GetErrorMessage(int errorcode)
    {
        IntPtr messagePtr = LibRawNative.GetErrorMessage(errorcode);
        return Marshal.PtrToStringAnsi(messagePtr);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="RawContext"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="RawContext"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (_librawContext != IntPtr.Zero)
            {
                LibRawNative.Recycle(_librawContext);
                _librawContext = IntPtr.Zero;
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Opens a raw file for processing.
    /// Corresponds to the C API function: libraw_open_file
    /// </summary>
    /// <param name="filePath">The file path to the raw file.</param>
    /// <returns>The error code from libraw.</returns>
    public int OpenFile(string filePath)
    {
        return LibRawNative.OpenFile(_librawContext, filePath);
    }

    /// <summary>
    /// Unpacks the raw data from the opened file into memory.
    /// Corresponds to the C API function: libraw_unpack
    /// </summary>
    /// <returns>The error code from libraw.</returns>
    public int Unpack()
    {
        return LibRawNative.Unpack(_librawContext);
    }

    /// <summary>
    /// Converts the raw data into a processed image.
    /// Corresponds to the C API function: libraw_dcraw_process
    /// </summary>
    /// <returns>The error code from libraw.</returns>
    public int ProcessDcraw()
    {
        return LibRawNative.ProcessDcraw(_librawContext);
    }

    /// <summary>
    /// Retrieves the processed image data from the LibRaw context.
    /// Corresponds to the C API function: libraw_dcraw_make_mem_image
    /// </summary>
    /// <returns>Processed image data.</returns>
    public IntPtr GetDcrawMemoryImage()
    {
        IntPtr memoryImage;
        LibRawNative.MakeDcrawMemoryImage(_librawContext, out memoryImage);
        return memoryImage;
    }

    /// <summary>
    /// Frees the memory of the processed image data.
    /// Corresponds to the C API function: libraw_dcraw_clear_mem
    /// </summary>
    /// <param name="imageData">Image data to be cleared.</param>
    public void ClearDcrawMemory(IntPtr imageData)
    {
        LibRawNative.ClearDcrawMemory(imageData);
    }

    // TODO: add all other functions in C API PInvoke list
}