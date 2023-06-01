using Sdcb.LibRaw.Natives;
using System;
using System.Runtime.InteropServices;

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
    public RawContext(IntPtr libRawContext)
    {
        _librawContext = libRawContext;
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
    public static string GetErrorMessage(LibRawError errorcode) => Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(errorcode))!;

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
    /// <param name="flags">The flags to use when initialize the libraw context.</param>
    /// <returns></returns>
    public static RawContext OpenFile(string filePath, LibRawInitFlags flags = LibRawInitFlags.None)
    {
        IntPtr raw = LibRawNative.Initialize(flags);
        LibRawError error = Environment.OSVersion.Platform switch
        {
#pragma warning disable CA1416 // Validate platform compatibility
            PlatformID.Win32NT => LibRawNative.OpenFileW(raw, filePath),
#pragma warning restore CA1416 // Validate platform compatibility
            _ => LibRawNative.OpenFile(raw, filePath),
        };

        if (error == LibRawError.Success)
        {
            return new RawContext(raw);
        }
        else
        {
            LibRawNative.Recycle(raw);
            throw new LibRawException(error, $"Failed opening file: {filePath}");
        }
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="RawContext"/> class from an input buffer.
    /// </summary>
    /// <param name="buffer">The input buffer of raw image data.</param>
    /// <param name="flags">The flags to use when initialize the libraw context.</param>
    /// <returns>A new instance of the <see cref="RawContext"/> class.</returns>
    public static unsafe RawContext FromBuffer(ReadOnlySpan<byte> buffer, LibRawInitFlags flags = LibRawInitFlags.None)
    {
        IntPtr raw = LibRawNative.Initialize(flags);

        LibRawError error;
        fixed (byte* p = buffer)
        {
            error = LibRawNative.OpenBuffer(raw, (IntPtr)p, buffer.Length);
        }

        if (error == LibRawError.Success)
        {
            return new RawContext(raw);
        }
        else
        {
            LibRawNative.Recycle(raw);
            throw new LibRawException(error, $"Failed opening buffer");
        }
    }

    /// <summary>
    /// Unpacks the raw data from the opened file into memory.
    /// Corresponds to the C API function: libraw_unpack
    /// </summary>
    /// <exception cref="LibRawException" />
    public void Unpack()
    {
        LibRawException.ThrowIfFailed(LibRawNative.Unpack(_librawContext));
    }

    /// <summary>
    /// Unpacks the thumbnail image from the opened file into memory.
    /// Corresponds to the C API function: libraw_unpack_thumb.
    /// </summary>
    /// <param name="index">The index of the thumbnail to unpack (default 0).</param>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    public void UnpackThunbnail(int index = 0)
    {
        LibRawException.ThrowIfFailed(LibRawNative.UnpackThumbnailExtended(_librawContext, index));
    }

    /// <summary>
    /// Converts the raw data into a processed image.
    /// Corresponds to the C API function: libraw_dcraw_process
    /// </summary>
    /// <exception cref="LibRawException" />
    public void ProcessDcraw()
    {
        LibRawException.ThrowIfFailed(LibRawNative.ProcessDcraw(_librawContext));
    }

    /// <summary>
    /// Converts the raw data into a processed image.
    /// Corresponds to the C API function: libraw_dcraw_process
    /// </summary>
    /// <returns>A <see cref="ProcessedImage"/> object representing the processed image.</returns>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    public unsafe ProcessedImage MakeDcrawMemoryImage()
    {
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryImage(_librawContext, out LibRawError errorCode);
        LibRawException.ThrowIfFailed(errorCode);

        LibRawProcessedImage* image = (LibRawProcessedImage*)rawImage;
        return new ProcessedImage(image); // need to dispose by user
    }
}