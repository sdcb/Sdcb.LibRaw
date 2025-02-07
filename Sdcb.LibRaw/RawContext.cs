﻿using Sdcb.LibRaw.Indexers;
using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Sdcb.LibRaw;

/// <summary>
/// A C# wrapper for the Libraw C API, providing high-level features to work with raw images.
/// </summary>
public class RawContext : IDisposable
{
    private IntPtr _r;
    private bool _disposed;

    /// <summary>
    /// The converted C structure from libraw_data_t pointer, it's unsafe.
    /// </summary>
    internal protected LibRawData RawData
    {
        get => Marshal.PtrToStructure<LibRawData>(_r);
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        set => Marshal.StructureToPtr(value, _r, fDeleteOld: false);
    }

    /// <summary>
    /// Checks if the object has been disposed and throws an exception if it has.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the object has been disposed.</exception>
    protected void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RawContext));
    }

    #region properties
    /// <summary>The width of the raw image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_raw_width</remarks>
    public int RawWidth
    {
        get
        {
            CheckDisposed();
            return LibRawNative.GetRawImageWidth(_r);
        }
    }

    /// <summary>The height of the raw image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_raw_height</remarks>
    public int RawHeight
    {
        get
        {
            CheckDisposed();
            return LibRawNative.GetRawImageHeight(_r);
        }
    }

    /// <summary>The width of the processed image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_iwidth</remarks>
    public int Width
    {
        get
        {
            CheckDisposed();
            return LibRawNative.GetProcessedImageWidth(_r);
        }
    }

    /// <summary>The height of the processed image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_iheight</remarks>
    public int Height
    {
        get
        {
            CheckDisposed();
            return LibRawNative.GetProcessedImageHeight(_r);
        }
    }

    internal LibRawProgress Progress
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe LibRawProgress GetX86() => ((LibRawDataX86*)_r)->Progress;
            unsafe LibRawProgress GetX64() => ((LibRawDataX64*)_r)->Progress;
        }
    }

    /// <summary>Property representing whether to output tiff.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_output_tif</remarks>
    public bool OutputTiff
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe bool GetX86() => ((LibRawDataX86*)_r)->OutputParams.OutputTiff != 0;
            unsafe bool GetX64() => ((LibRawDataX64*)_r)->OutputParams.OutputTiff != 0;
        }

        set
        {
            CheckDisposed();
            LibRawNative.SetOutputTiff(_r, value ? 1 : 0);
        }
    }

    /// <summary>Property representing output bits per sample.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_output_bps</remarks>
    public int OutputBitsPerSample
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe int GetX86() => ((LibRawDataX86*)_r)->OutputParams.OutputBps;
            unsafe int GetX64() => ((LibRawDataX64*)_r)->OutputParams.OutputBps;
        }

        set
        {
            CheckDisposed();
            if (value % 8 != 0) throw new ArgumentOutOfRangeException(nameof(value), "value must be a multiple of 8");
            LibRawNative.SetOutputBitsPerSample(_r, value);
        }
    }

    /// <summary>Property representing the output color space.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_output_color</remarks>
    public LibRawColorSpace OutputColorSpace
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe LibRawColorSpace GetX86() => (LibRawColorSpace)((LibRawDataX86*)_r)->OutputParams.OutputColor;
            unsafe LibRawColorSpace GetX64() => (LibRawColorSpace)((LibRawDataX64*)_r)->OutputParams.OutputColor;
        }

        set
        {
            CheckDisposed();
            LibRawNative.SetOutputColorSpace(_r, value);
        }
    }

    /// <summary>Camera multiplier indexer</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_cam_mul</remarks>
    public IReadOnlyList<float> CameraMultipler => new CameraMultiplerIndexer(_r, _disposed);

    /// <summary>Pre multiplier indexer</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_pre_mul</remarks>
    public IReadOnlyList<float> PreMultipler => new PreMultiplerIndexer(_r, _disposed);

    /// <summary>Gets the <see cref="IReadOnly2DIndexer{T}"/> instance for the RGB camera.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_rgb_cam</remarks>
    public IReadOnly2DIndexer<float> RgbCamera => new RgbCamera2DIndexer(_r, _disposed);

    /// <summary>Gets the maximum color.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_color_maximum</remarks>
    public int ColorMaximum
    {
        get
        {
            CheckDisposed();
            return LibRawNative.GetColorMaximum(_r);
        }
    }

    /// <summary>Gets or set the user multiplier.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_user_mul</remarks>
    public IIndexer<float> UserMultiplier => new UserMultiplierIndexer(_r, _disposed);

    /// <summary>Gets or sets the demosaic algorithm used to convert the raw data to an image.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_demosaic</remarks>
    public DemosaicAlgorithm DemosaicAlgorithm
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe DemosaicAlgorithm GetX86() => (DemosaicAlgorithm)((LibRawDataX86*)_r)->OutputParams.UserQual;
            unsafe DemosaicAlgorithm GetX64() => (DemosaicAlgorithm)((LibRawDataX64*)_r)->OutputParams.UserQual;
        }
        set
        {
            CheckDisposed();
            LibRawNative.SetDemosaicAlgorithm(_r, value);
        }
    }

    /// <summary>Gets or sets the automatic maximum adjustment threshold for the raw image data.</summary>
    /// <remarks>
    /// When set to 0, the automatic maximum adjustment is disabled. When set to a non-zero value, 
    /// it represents the threshold relative to the sensor's maximum value. If the image's maximum value
    /// exceeds this threshold, LibRaw will automatically reduce the image's brightness to prevent overexposure.
    /// <para>Corresponds to the C API function: libraw_set_adjust_maximum_thr</para>
    /// </remarks>
    /// <value>A float representing the automatic maximum adjustment threshold.</value>
    public float AdjustMaximumThreshold
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe float GetX86() => ((LibRawDataX86*)_r)->OutputParams.AdjustMaximumThr;
            unsafe float GetX64() => ((LibRawDataX64*)_r)->OutputParams.AdjustMaximumThr;
        }
        set
        {
            CheckDisposed();
            LibRawNative.SetAdjustMaximumThreshold(_r, value);
        }
    }

    /// <summary>Output gamma indexer</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_gamma</remarks>
    public IIndexer<float> Gamma => new GammaIndexer(_r, _disposed);

    /// <summary>Gets or sets a value indicating whether automatic brightness correction is enabled.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_no_auto_bright</remarks>
    public bool AutoBright
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe bool GetX86() => ((LibRawDataX86*)_r)->OutputParams.NoAutoBright == 0;
            unsafe bool GetX64() => ((LibRawDataX64*)_r)->OutputParams.NoAutoBright == 0;
        }
        set
        {
            CheckDisposed();
            LibRawNative.SetAutoBrightnessCorrection(_r, value ? 0 : 1);
        }
    }

    /// <summary>Gets or sets the brightness property.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_bright</remarks>
    public float Brightness
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe float GetX86() => ((LibRawDataX86*)_r)->OutputParams.Bright;
            unsafe float GetX64() => ((LibRawDataX64*)_r)->OutputParams.Bright;
        }
        set
        {
            CheckDisposed();
            LibRawNative.SetBrightness(_r, value);
        }
    }

    /// <summary>Gets or sets the highlight mode.</summary>
    /// <remarks>Corresponds to the C API function: libraw_set_highlight</remarks>
    public int HighlightMode
    {
        get
        {
            CheckDisposed();
            if (IntPtr.Size == 8)
            {
                return GetX64();
            }
            else
            {
                return GetX86();
            }

            unsafe int GetX86() => ((LibRawDataX86*)_r)->OutputParams.Highlight;
            unsafe int GetX64() => ((LibRawDataX64*)_r)->OutputParams.Highlight;
        }
        set
        {
            CheckDisposed();
            LibRawNative.SetHighlightMode(_r, value);
        }
    }
    #endregion

    /// <summary>Returns a pointer to the underlying native object.</summary>
    /// <returns>A pointer to the underlying native object.</returns>
    public IntPtr UnsafeGetHandle()
    {
        CheckDisposed();
        return _r;
    }


    /// <summary>Gets the decoder information for the current RawContext object.</summary>
    /// <remarks>Corresponds to the C API function: libraw_get_decoder_info</remarks>
    public unsafe DecoderInfo DecoderInfo
    {
        get
        {
            CheckDisposed();
            LibRawDecoderInfo d = new();
            LibRawException.ThrowIfFailed(LibRawNative.GetDecoderInfo(_r, (IntPtr)(&d)));
            return DecoderInfo.FromNative(d);
        }
    }

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
                LibRawNative.Close(_r);
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

    #region init methods
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
            LibRawNative.Close(raw);
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
            LibRawNative.Close(raw);
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
            LibRawNative.Close(raw);
            throw new LibRawException(error, $"Failed opening buffer");
        }
    }
    #endregion

    /// <summary>Unpacks the raw data from the opened file into memory.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_unpack</remarks>
    public void Unpack()
    {
        CheckDisposed();
        LibRawException.ThrowIfFailed(LibRawNative.Unpack(_r));
    }

    /// <summary>Unpacks the thumbnail image from the opened file into memory.</summary>
    /// <param name="thumbnailIndex">The index of the thumbnail to unpack (default 0).</param>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_unpack_thumb</remarks>
    public void UnpackThumbnail(int thumbnailIndex = 0)
    {
        CheckDisposed();
        LibRawException.ThrowIfFailed(LibRawNative.UnpackThumbnailExtended(_r, thumbnailIndex));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public void DcrawProcess(Action<OutputParams> configure)
    {
        CheckDisposed();
        OutputParams p = OutputParams.FromLibRaw(_r);
        configure(p);
        p.Commit(_r);

        try
        {
            LibRawException.ThrowIfFailed(LibRawNative.ProcessDcraw(_r));
        }
        finally
        {
            OutputParams.FreeLibRawStrings(_r);
        }
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <exception cref="LibRawException" />
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public void DcrawProcess()
    {
        CheckDisposed();
        LibRawException.ThrowIfFailed(LibRawNative.ProcessDcraw(_r));
    }

    /// <summary>Converts the raw data into a processed image.</summary>
    /// <returns>A <see cref="ProcessedImage"/> object representing the processed image.</returns>
    /// <exception cref="LibRawException">Thrown if there is an error during the dcraw process.</exception>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_process</remarks>
    public unsafe ProcessedImage MakeDcrawMemoryImage()
    {
        CheckDisposed();
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
        CheckDisposed();
        IntPtr rawImage = LibRawNative.MakeDcrawMemoryThumbnail(_r, out LibRawError errorCode);
        LibRawException.ThrowIfFailed(errorCode);

        LibRawProcessedImage* image = (LibRawProcessedImage*)rawImage;
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            LibRawData data = RawData;
            if (image->Width == 0)
            {
                image->Width = data.Thumbnail.Width;
            }
            if (image->Height == 0)
            {
                image->Height = data.Thumbnail.Height;
            }
            if (image->Colors == 0)
            {
                image->Colors = (ushort)data.Thumbnail.Colors;
            }
        }

        return new ProcessedImage(image); // need to dispose by user
    }

    /// <summary>Writes the image in PPM or TIFF format using Dcraw.</summary>
    /// <param name="fileName">The output file path.</param>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_ppm_tiff_writer</remarks>
    public void WriteDcrawPpmTiff(string fileName)
    {
        CheckDisposed();
        LibRawException.ThrowIfFailed(LibRawNative.WriteDcrawPpmTiff(_r, fileName));
    }

    /// <summary>Writes the thumbnail using Dcraw.</summary>
    /// <param name="fileName">The output file path.</param>
    /// <remarks>Corresponds to the C API function: libraw_dcraw_thumb_writer</remarks>
    public void WriteDcrawThumbnail(string fileName)
    {
        CheckDisposed();
        LibRawException.ThrowIfFailed(LibRawNative.WriteDcrawThumbnail(_r, fileName));
    }

    /// <summary>
    /// Get the image parameters of the raw file.
    /// </summary>
    /// <returns>The image parameters of the raw file.</returns>
    public LibRawImageParams ImageParams
    {
        get
        {
            CheckDisposed();
            IntPtr ptr = LibRawNative.GetImageParameters(_r);
            return Marshal.PtrToStructure<LibRawImageParams>(ptr);
        }
    }

    /// <summary>
    /// Gets the others image parameters.
    /// </summary>
    /// <returns>The others image parameters of the raw file.</returns>
    public LibRawImageOtherParams ImageOtherParams
    {
        get
        {
            CheckDisposed();
            IntPtr ptr = LibRawNative.GetImageOtherParameters(_r);
            return Marshal.PtrToStructure<LibRawImageOtherParams>(ptr);
        }
    }

    /// <summary>
    /// Gets the lens information used to take the photograph.
    /// </summary>
    public LibRawLensInfo LensInfo
    {
        get
        {
            CheckDisposed();
            IntPtr ptr = LibRawNative.GetLensInformation(_r);
            return Marshal.PtrToStructure<LibRawLensInfo>(ptr);
        }
    }
}