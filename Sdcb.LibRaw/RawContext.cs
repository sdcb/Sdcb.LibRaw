﻿using Sdcb.LibRaw.Natives;
using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;

/// <summary>
/// A C# wrapper for the Libraw C API, providing high-level features to work with raw images.
/// </summary>
public class RawContext : IDisposable
{
    private IntPtr _r;
    private bool _disposed;

    /// <summary>The width of the raw image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_raw_width</remarks>
    public int RawWidth => LibRawNative.GetRawImageWidth(_r);

    /// <summary>The height of the raw image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_raw_height</remarks>
    public int RawHeight => LibRawNative.GetRawImageHeight(_r);

    /// <summary>The width of the processed image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_iwidth</remarks>
    public int Width => LibRawNative.GetProcessedImageWidth(_r);

    /// <summary>The height of the processed image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_iheight</remarks>
    public int Height => LibRawNative.GetProcessedImageHeight(_r);

    /// <summary>
    /// Initializes a new instance of the <see cref="RawContext"/> class.
    /// </summary>
    public RawContext(IntPtr libRawContext)
    {
        _r = libRawContext;
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
    /// <remarks>Corresponds to the C API function: libraw_recycle</remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (_r != IntPtr.Zero)
            {
                LibRawNative.Recycle(_r);
                _r = IntPtr.Zero;
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

    /// <summary>Opens Bayer data from an <seealso cref="ReadOnlySpan{Byte}"/> and initializes a new instance of <see cref="RawContext"/>.</summary>
    /// <param name="bayerData">The array containing the Bayer data.</param>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="leftMargin">The size of the left margin. Default is 0.</param>
    /// <param name="topMargin">The size of the top margin. Default is 0.</param>
    /// <param name="rightMargin">The size of the right margin. Default is 0.</param>
    /// <param name="bottomMargin">The size of the bottom margin. Default is 0.</param>
    /// <param name="procFlags">Processing flags. Default is 0. For 10-bit format: 1 means "4 pixels in 5 bytes" packing, 0 means "6 pixels in 8 bytes" packing. For 16-bit format: 1 means Big-endian data.</param>
    /// <param name="bayerPattern">The pattern of the Bayer filter. Default is <see cref="OpenBayerPattern.BGGR"/>. Possible values: RGGB, BGGR, GRBG, GBRG.</param>
    /// <param name="unusedBits">The number of unused bits. Default is 0. Represents the count of upper zero bits.</param>
    /// <param name="otherFlags">Other flags. Default is 0. Bit 1: filter (average neighbors) for pixels with values of zero. Bits 2-4: the orientation of the image (0=do not rotate, 3=180, 5=90CCW, 6=90CW).</param>
    /// <param name="blackLevel">The black level of the image. Default is 0.</param>
    /// <param name="flags">Initialization flags. Default is LibRawInitFlags.None.</param>
    /// <returns>A new instance of <see cref="RawContext"/>.</returns>
    /// <remarks>Corresponds to the C API function: libraw_open_bayer</remarks>
    public unsafe static RawContext OpenBayerData<T>(ReadOnlySpan<T> bayerData, int width, int height,
        int leftMargin = 0, int topMargin = 0, int rightMargin = 0, int bottomMargin = 0,
        byte procFlags = 0, OpenBayerPattern bayerPattern = OpenBayerPattern.BGGR,
        int unusedBits = 0, int otherFlags = 0, int blackLevel = 0,
        LibRawInitFlags flags = LibRawInitFlags.None) where T : struct
    {
        if (bayerData == null) throw new ArgumentNullException(nameof(bayerData));
        if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), "Width must be at least 1.");
        if (height < 1) throw new ArgumentOutOfRangeException(nameof(height), "Height must be at least 1.");
        if (leftMargin < 0) throw new ArgumentOutOfRangeException(nameof(leftMargin), "Left margin cannot be negative.");
        if (topMargin < 0) throw new ArgumentOutOfRangeException(nameof(topMargin), "Top margin cannot be negative.");
        if (rightMargin < 0) throw new ArgumentOutOfRangeException(nameof(rightMargin), "Right margin cannot be negative.");
        if (bottomMargin < 0) throw new ArgumentOutOfRangeException(nameof(bottomMargin), "Bottom margin cannot be negative.");
        if (unusedBits < 0) throw new ArgumentOutOfRangeException(nameof(unusedBits), "Unused bits cannot be negative.");
        if (otherFlags < 0) throw new ArgumentOutOfRangeException(nameof(otherFlags), "Other flags cannot be negative.");
        if (blackLevel < 0) throw new ArgumentOutOfRangeException(nameof(blackLevel), "Black level cannot be negative.");
        if (bayerData.Length < width * height) throw new ArgumentException("The length of the bayer data array must be at least width * height.", nameof(bayerData));

        IntPtr raw = LibRawNative.Initialize(flags);

        LibRawError error;
#pragma warning disable CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
        fixed (void* p = &bayerData[0])
        {
            error = LibRawNative.OpenBayerData(raw, (IntPtr)p, (uint)(bayerData.Length * sizeof(T)), (ushort)width, (ushort)height,
                (ushort)leftMargin, (ushort)topMargin, (ushort)rightMargin, (ushort)bottomMargin,
                procFlags, bayerPattern, (uint)unusedBits, (uint)otherFlags, (uint)blackLevel);
        }
#pragma warning restore CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针

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

    /// <summary>Initializes a new instance of the <see cref="RawContext"/> class from an input buffer.</summary>
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
        LibRawException.ThrowIfFailed(LibRawNative.Unpack(_r));
    }

    /// <summary>Unpacks the thumbnail image from the opened file into memory.</summary>
    /// <param name="index">The index of the thumbnail to unpack (default 0).</param>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_unpack_thumb</remarks>
    public void UnpackThunbnail(int index = 0)
    {
        LibRawException.ThrowIfFailed(LibRawNative.UnpackThumbnailExtended(_r, index));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public void ProcessDcraw()
    {
        LibRawException.ThrowIfFailed(LibRawNative.ProcessDcraw(_r));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <returns>A <see cref="ProcessedImage"/> object representing the processed image.</returns>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public unsafe ProcessedImage MakeDcrawMemoryImage()
    {
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryImage(_r, out LibRawError errorCode);
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
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryThumbnail(_r, out LibRawError errorCode);
        LibRawException.ThrowIfFailed(errorCode);

        LibRawProcessedImage* image = (LibRawProcessedImage*)rawImage;
        return new ProcessedImage(image); // need to dispose by user
    }
}