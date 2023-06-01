using Sdcb.LibRaw.Natives;
using System;
using System.Collections;
using System.Collections.Generic;
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
    /// <remarks>
    /// Corresponds to the C API function: libraw_recycle
    /// </remarks>
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

    #region static methods
    /// <summary>Returns an array of supported cameras.</summary>
    /// <returns>An array of strings representing supported cameras.</returns>
    /// <remarks>Corresponds to the C API function: libraw_cameraCount, libraw_cameraList</remarks>
    public unsafe static string[] SupportedCameras
    {
        get
        {
            int count = LibRawNative.GetCameraCount();
            IntPtr* list = (IntPtr*)LibRawNative.GetCameraList(); // char**
            string[] cameras = new string[count];
            for (int i = 0; i < count; i++)
            {
                cameras[i] = Marshal.PtrToStringAnsi(list[i])!;
            }
            return cameras;
        }
    }

    /// <summary>Gets the version of the underlying LibRaw library.</summary>
    /// <remarks>Corresponds to the C API function: libraw_version</remarks>
    public static string Version => Marshal.PtrToStringAnsi(LibRawNative.GetVersion())!;

    /// <summary>Gets the version number of the LibRaw native library used by the current instance of RawContext.</summary>
    /// <remarks>Corresponds to the C API function: libraw_versionNumber</remarks>
    public static Version VersionNumber
    {
        get
        {
            int num = LibRawNative.GetVersionNumber();
            int major = num >> 16;
            int minor = num >> 8;
            int patch = num & 0xFF;
            return new Version(major, minor, patch);
        }
    }
    #endregion

    /// <summary>Opens a RAW file for processing.</summary>
    /// <param name="filePath">The path to the file to open.</param>
    /// <param name="flags">Flags to modify library behavior during opening.</param>
    /// <returns>A new RawContext instance representing the opened file.</returns>
    /// <remarks>Corresponds to the C API function: libraw_open_file</remarks>
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
    /// <remarks>Corresponds to the C API function: libraw_open_buffer</remarks>
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

    /// <summary>Unpacks the raw data from the opened file into memory.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_unpack</remarks>
    public void Unpack()
    {
        LibRawException.ThrowIfFailed(LibRawNative.Unpack(_librawContext));
    }

    /// <summary>Unpacks the thumbnail image from the opened file into memory.</summary>
    /// <param name="index">The index of the thumbnail to unpack (default 0).</param>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_unpack_thumb</remarks>
    public void UnpackThunbnail(int index = 0)
    {
        LibRawException.ThrowIfFailed(LibRawNative.UnpackThumbnailExtended(_librawContext, index));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public void ProcessDcraw()
    {
        LibRawException.ThrowIfFailed(LibRawNative.ProcessDcraw(_librawContext));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <returns>A <see cref="ProcessedImage"/> object representing the processed image.</returns>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public unsafe ProcessedImage MakeDcrawMemoryImage()
    {
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryImage(_librawContext, out LibRawError errorCode);
        LibRawException.ThrowIfFailed(errorCode);

        LibRawProcessedImage* image = (LibRawProcessedImage*)rawImage;
        return new ProcessedImage(image); // need to dispose by user
    }

    /// <summary>
    /// Retrieves a thumbnail image from the Dcraw memory.
    /// </summary>
    /// <returns>The thumbnail image retrieved from the Dcraw memory.</returns>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_make_mem_thumb</remarks>
    public unsafe ProcessedImage MakeDcrawMemoryThumbnail()
    {
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryThumbnail(_librawContext, out LibRawError errorCode);
        LibRawException.ThrowIfFailed(errorCode);

        LibRawProcessedImage* image = (LibRawProcessedImage*)rawImage;
        return new ProcessedImage(image); // need to dispose by user
    }
}