using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Sdcb.LibRaw.Natives;

public static class LibRawNative
{
    static LibRawNative()
    {
        LibRawNativeLoader.Init();
        if (OpenMPThreadCount == null)
        {
            OpenMPThreadCount = Environment.ProcessorCount.ToString();
        }
    }

    public const string Dll = "libraw.dll";

    /// <summary> Gets or sets the number of OpenMP threads uses. </summary>
    /// <remarks>
    /// <para>This property is implemented by using OMP_NUM_THREADS environment variable.</para>
    /// OMP_NUM_THREADS environment variable can be set to multiple numbers separated by commas. 
    /// This setting allows you to specify the number of threads for different nested levels. 
    /// Each number corresponds to a parallel region from outermost to innermost.
    /// </remarks>
    public static string? OpenMPThreadCount
    {
        get => Environment.GetEnvironmentVariable("OMP_NUM_THREADS");
        set => Environment.SetEnvironmentVariable("OMP_NUM_THREADS", value);
    }

    #region consts/macros
    /// <summary>
    /// The original C API macro: LIBRAW_DEFAULT_ADJUST_MAXIMUM_THRESHOLD
    /// Represents the default adjust maximum threshold, which is 0.75f.
    /// </summary>
    public const float DefaultAdjustMaximumThreshold = 0.75f;

    /// <summary>
    /// The original C API macro: LIBRAW_DEFAULT_AUTO_BRIGHTNESS_THRESHOLD
    /// Represents the default auto brightness threshold, which is 0.01f.
    /// </summary>
    public const float DefaultAutoBrightnessThreshold = 0.01f;

    /// <summary>
    /// The original C API macro: LIBRAW_MAX_ALLOC_MB_DEFAULT
    /// Represents the default maximum memory allocation in megabytes, which is 2048L.
    /// </summary>
    public const long MaxAllocMbDefault = 2048L;

    /// <summary>
    /// The original C API macro: LIBRAW_MAX_NONDNG_RAW_FILE_SIZE
    /// Represents the maximum non-DNG raw file size, which is 2147483647ULL.
    /// </summary>
    public const ulong MaxNonDngRawFileSize = 2147483647UL;

    /// <summary>
    /// The original C API macro: LIBRAW_MAX_DNG_RAW_FILE_SIZE
    /// Represents the maximum DNG raw file size, which is 2147483647ULL.
    /// </summary>
    public const ulong MaxDngRawFileSize = 2147483647UL;

    /// <summary>
    /// The original C API macro: LIBRAW_MAX_THUMBNAIL_MB
    /// Represents the maximum thumbnail size in megabytes, which is 512L.
    /// </summary>
    public const long MaxThumbnailMb = 512L;

    /// <summary>
    /// The original C API macro: LIBRAW_MAX_METADATA_BLOCKS
    /// Represents the maximum number of metadata blocks, which is 1024.
    /// </summary>
    public const int MaxMetadataBlocks = 1024;

    /// <summary>
    /// The original C API macro: LIBRAW_CBLACK_SIZE
    /// Represents the size of the cblack array, which is 4104.
    /// </summary>
    public const int CBlackSize = 4104;

    /// <summary>
    /// The original C API macro: LIBRAW_IFD_MAXCOUNT
    /// Represents the maximum number of IFD (Image File Directory) entries, which is 10.
    /// </summary>
    public const int IfdMaxCount = 10;

    /// <summary>
    /// The original C API macro: LIBRAW_THUMBNAIL_MAXCOUNT
    /// Represents the maximum number of thumbnails, which is 8.
    /// </summary>
    public const int ThumbnailMaxCount = 8;

    /// <summary>
    /// The original C API macro: LIBRAW_CRXTRACKS_MAXCOUNT
    /// Represents the maximum number of CRX tracks, which is 16.
    /// </summary>
    public const int CrxTracksMaxCount = 16;

    /// <summary>
    /// The original C API macro: LIBRAW_AFDATA_MAXCOUNT
    /// Represents the maximum number of AF data entries, which is 4.
    /// </summary>
    public const int AfDataMaxCount = 4;

    /// <summary>
    /// The original C API macro: LIBRAW_AHD_TILE
    /// Represents the AHD tile size, which is 512. AHD stands for Average Height Difference.
    /// </summary>
    public const int AhdTile = 512;

    /// <summary>
    /// The original C API macro: LIBRAW_LENS_NOT_SET
    /// Represents the value indicating that the lens is not set, which is 0xffffffffffffffffULL.
    /// </summary>
    public const ulong LensNotSet = 0xffffffffffffffffUL;

    /// <summary>
    /// The original C API macro: LIBRAW_PROGRESS_THUMB_MASK
    /// Represents the thumbnail processing progress mask, which is 0x0fffffff.
    /// </summary>
    public const int ProgressThumbMask = 0x0fffffff;

    /// <summary>
    /// The original C API macro: LIBRAW_FATAL_ERROR(ec)
    /// Determines whether the error code is a fatal error.
    /// </summary>
    /// <param name="errorCode">The error code to evaluate.</param>
    /// <returns>True if the error code is a fatal error, otherwise false.</returns>
    public static bool IsFatalError(int errorCode)
    {
        return errorCode < -100000;
    }

    /// <summary>
    /// The original C API macro: LIBRAW_FATAL_ERROR(ec)
    /// Determines whether the error code is a fatal error.
    /// </summary>
    /// <param name="errorCode">The error code to evaluate.</param>
    /// <returns>True if the error code is a fatal error, otherwise false.</returns>
    public static bool IsFatalError(LibRawError errorCode)
    {
        return IsFatalError((int)errorCode);
    }

    /// <summary>
    /// The original C API macro: LIBRAW_XTRANS
    /// Represents the X-Trans sensor type, which has a value of 9.
    /// </summary>
    public const int XTrans = 9;
    #endregion

    #region functions
    /// <summary>
    /// Get the error message related to the error code.
    /// </summary>
    /// <param name="errorcode">The error code.</param>
    /// <returns>The error message.</returns>
    [DllImport(Dll, EntryPoint = "libraw_strerror")]
    public static extern IntPtr GetErrorMessage(LibRawError errorcode);

    /// <summary>
    /// Get the progress message related to the progress status.
    /// </summary>
    /// <param name="progress">The progress status.</param>
    /// <returns>The progress message.</returns>
    [DllImport(Dll, EntryPoint = "libraw_strprogress")]
    public static extern IntPtr GetProgressMessage(LibRawProgress progress);

    /// <summary>
    /// Initialize LibRaw with the specified flags.
    /// </summary>
    /// <param name="flags">The flags to initialize LibRaw with.</param>
    /// <returns>An IntPtr to the initialized LibRaw data.</returns>
    [DllImport(Dll, EntryPoint = "libraw_init")]
    public static extern IntPtr Initialize(LibRawInitFlags flags = LibRawInitFlags.None);

    /// <summary>
    /// Open the specified file for use with LibRaw.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="fileName">The file name to open.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_open_file")]
    public static extern LibRawError OpenFile(IntPtr data, string fileName);

    /// <summary>
    /// Open the specified wide-character file for use with LibRaw.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="fileName">The wide-character file name to open.</param>
    /// <returns>The status of the operation.</returns>
    [SupportedOSPlatform("windows")]
    [DllImport(Dll, EntryPoint = "libraw_open_wfile")]
    public static extern LibRawError OpenFileW(IntPtr data, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

    /// <summary>
    /// Open the buffer for use with LibRaw.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="buffer">The IntPtr to the buffer to open.</param>
    /// <param name="size">The size of the buffer.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_open_buffer")]
    public static extern LibRawError OpenBuffer(IntPtr data, IntPtr buffer, long size);

    /// <summary>
    /// Open the Bayer data for use with LibRaw.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="rawData">The IntPtr to the Bayer data.</param>
    /// <param name="rawDataLength">The length of the Bayer data.</param>
    /// <param name="rawWidth">The width of the Bayer data.</param>
    /// <param name="rawHeight">The height of the Bayer data.</param>
    /// <param name="leftMargin">The left margin of the Bayer data.</param>
    /// <param name="topMargin">The top margin of the Bayer data.</param>
    /// <param name="rightMargin">The right margin of the Bayer data.</param>
    /// <param name="bottomMargin">The bottom margin of the Bayer data.</param>
    /// <param name="procFlags">The single-byte processing flags.</param>
    /// <param name="bayerPattern">The single-byte Bayer pattern.</param>
    /// <param name="unusedBits">The number of unused bits in the Bayer data.</param>
    /// <param name="otherFlags">The other flags associated with the Bayer data.</param>
    /// <param name="blackLevel">The black level value of the Bayer data.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_open_bayer")]
    public static extern LibRawError OpenBayerData(IntPtr data, IntPtr rawData, uint rawDataLength, ushort rawWidth, ushort rawHeight, ushort leftMargin, ushort topMargin, ushort rightMargin, ushort bottomMargin, byte procFlags, OpenBayerPattern bayerPattern, uint unusedBits, uint otherFlags, uint blackLevel);

    /// <summary>
    /// Unpack the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_unpack")]
    public static extern LibRawError Unpack(IntPtr data);

    /// <summary>
    /// Unpack the thumbnail from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_unpack_thumb")]
    public static extern LibRawError UnpackThumbnail(IntPtr data);

    /// <summary>
    /// Unpack the thumbnail with the specified format from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="index">The index of the thumbnail to unpack (default 0).</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_unpack_thumb_ex")]
    public static extern LibRawError UnpackThumbnailExtended(IntPtr data, int index = 0);

    /// <summary>
    /// Recycle the LibRaw data stream.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_recycle_datastream")]
    public static extern void RecycleDataStream(IntPtr data);

    /// <summary>
    /// Recycle the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_recycle")]
    public static extern void Recycle(IntPtr data);

    /// <summary>
    /// Close the LibRaw data and free memory.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_close")]
    public static extern void Close(IntPtr data);

    /// <summary>
    /// Subtract black level from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_subtract_black")]
    public static extern void SubtractBlack(IntPtr data);

    /// <summary>
    /// Convert raw data to an image in memory.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_raw2image")]
    public static extern int RawToImage(IntPtr data);

    /// <summary>
    /// Free the memory allocated for the image data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_free_image")]
    public static extern void FreeImage(IntPtr data);

    /// <summary>
    /// Get the LibRaw version string.
    /// </summary>
    /// <returns>The LibRaw version string.</returns>
    [DllImport(Dll, EntryPoint = "libraw_version")]
    public static extern IntPtr GetVersion();

    /// <summary>
    /// Get the LibRaw version number.
    /// </summary>
    /// <returns>The LibRaw version number.</returns>
    [DllImport(Dll, EntryPoint = "libraw_versionNumber")]
    public static extern int GetVersionNumber();

    /// <summary>
    /// Get the list of supported cameras.
    /// </summary>
    /// <returns>An IntPtr pointing to the list of supported cameras.</returns>
    [DllImport(Dll, EntryPoint = "libraw_cameraList")]
    public static extern IntPtr GetCameraList();

    /// <summary>
    /// Get the number of supported cameras.
    /// </summary>
    /// <returns>The number of supported cameras.</returns>
    [DllImport(Dll, EntryPoint = "libraw_cameraCount")]
    public static extern int GetCameraCount();

    /// <summary>
    /// Set the Exif parser callback handler.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="cb">The Exif parser callback handler function.</param>
    /// <param name="datap">The LibRaw data handler IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_exifparser_handler")]
    public static unsafe extern void SetExifParserHandler(IntPtr data, delegate*<void*, int, int, int, uint, void*, long, void> cb, IntPtr datap);

    /// <summary>
    /// Set the data error callback handler.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="func">The data error callback handler function.</param>
    /// <param name="datap">The LibRaw data error handler IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_dataerror_handler")]
    public static unsafe extern void SetDataErrorHandler(IntPtr data, delegate*<void*, string, int, void> func, IntPtr datap);

    /// <summary>
    /// Set the progress callback handler.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="cb">The progress callback handler function.</param>
    /// <param name="datap">The LibRaw progress handler IntPtr.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_progress_handler")]
    public static unsafe extern void SetProgressHandler(IntPtr data, delegate*<void*, LibRawProgress, int, int, int> cb, IntPtr datap);

    /// <summary>
    /// Get the name of the unpack function used for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The name of the unpack function.</returns>
    [DllImport(Dll, EntryPoint = "libraw_unpack_function_name")]
    public static extern IntPtr GetUnpackFunctionName(IntPtr data);

    /// <summary>
    /// Get the decoder information for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="decoderInfo">The libraw_decoder_info_t struct to fill with decoder information.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_decoder_info")]
    public static extern int GetDecoderInfo(IntPtr data, out LibRawDecoderInfo decoderInfo);

    /// <summary>
    /// Get the color value for the specified row and column from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <returns>The color value for the specified row and column.</returns>
    [DllImport(Dll, EntryPoint = "libraw_COLOR")]
    public static extern int GetColor(IntPtr data, int row, int col);

    /// <summary>
    /// Get the capabilities of the LibRaw library.
    /// </summary>
    /// <returns>The capabilities of the LibRaw library.</returns>
    [DllImport(Dll, EntryPoint = "libraw_capabilities")]
    public static extern LibRawCapability GetCapabilities();

    /// <summary>
    /// Adjust the sizes of the LibRaw data for information purposes only.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_adjust_sizes_info_only")]
    public static extern int AdjustSizesInfoOnly(IntPtr data);

    /// <summary>
    /// Write the LibRaw data to a PPM/ TIFF file.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="fileName">The output file name.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_ppm_tiff_writer")]
    public static extern LibRawError WriteDcrawPpmTiff(IntPtr data, string fileName);

    /// <summary>
    /// Write the LibRaw thumbnail data to an output file.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="fileName">The output file name.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_thumb_writer")]
    public static extern int WriteDcrawThumbnail(IntPtr data, string fileName);

    /// <summary>
    /// Process the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The status of the operation.</returns>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_process")]
    public static extern LibRawError ProcessDcraw(IntPtr data);

    /// <summary>
    /// Create a memory-based image from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="errorCode">Pointer to an integer that will hold the error code (if any).</param>
    /// <returns>An IntPtr to the created memory-based image (null if an error occurred).</returns>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_make_mem_image")]
    public static extern IntPtr MakeDcrawMemoryImage(IntPtr data, out LibRawError errorCode);

    /// <summary>
    /// Create a memory-based thumbnail from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="errorCode">Pointer to an integer that will hold the error code (if any).</param>
    /// <returns>An IntPtr to the created memory-based thumbnail (null if an error occurred).</returns>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_make_mem_thumb")]
    public static extern IntPtr MakeDcrawMemoryThumbnail(IntPtr data, out LibRawError errorCode);

    /// <summary>
    /// Clear and free the memory allocated by libraw_dcraw_make_mem_image or libraw_dcraw_make_mem_thumb functions.
    /// </summary>
    /// <param name="processDataImage">An IntPtr to the memory-based image or thumbnail.</param>
    [DllImport(Dll, EntryPoint = "libraw_dcraw_clear_mem")]
    public static extern void ClearDcrawMemory(IntPtr processDataImage);

    /// <summary>
    /// Set the demosaic algorithm for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The demosaic algorithm value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_demosaic")]
    public static extern void SetDemosaicAlgorithm(IntPtr data, int value);

    /// <summary>
    /// Set the output color space for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The output color space value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_output_color")]
    public static extern void SetOutputColorSpace(IntPtr data, ColorSpace value);

    /// <summary>
    /// Set the adjust maximum threshold for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The adjust maximum threshold value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_adjust_maximum_thr")]
    public static extern void SetAdjustMaximumThreshold(IntPtr data, float value);

    /// <summary>
    /// Set the user multiplier value at a specified index for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="index">The index of the user multiplier value to set.</param>
    /// <param name="value">The user multiplier value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_user_mul")]
    public static extern void SetUserMultiplier(IntPtr data, int index, float value);

    /// <summary>
    /// Set the output bits-per-sample for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The output bits-per-sample value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_output_bps")]
    public static extern void SetOutputBitsPerSample(IntPtr data, int value);

    /// <summary>
    /// Set the gamma value at a specified index for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="index">The index of the gamma value to set.</param>
    /// <param name="value">The gamma value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_gamma")]
    public static extern void SetGamma(IntPtr data, int index, float value);

    /// <summary>
    /// Enable or disable automatic brightness correction for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">0 to enable automatic brightness correction, or 1 to disable it.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_no_auto_bright")]
    public static extern void SetAutoBrightnessCorrection(IntPtr data, int value);

    /// <summary>
    /// Set the brightness adjustment value for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The brightness adjustment value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_bright")]
    public static extern void SetBrightness(IntPtr data, float value);

    /// <summary>
    /// Set the highlight mode for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The highlight mode value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_highlight")]
    public static extern void SetHighlightMode(IntPtr data, int value);

    /// <summary>
    /// Set the noise reduction mode for the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="value">The noise reduction mode value.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_fbdd_noiserd")]
    public static extern void SetNoiseReductionMode(IntPtr data, int value);

    /// <summary>
    /// Get the raw image height from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The raw image height.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_raw_height")]
    public static extern int GetRawImageHeight(IntPtr data);

    /// <summary>
    /// Get the raw image width from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The raw image width.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_raw_width")]
    public static extern int GetRawImageWidth(IntPtr data);

    /// <summary>
    /// Get the processed image height from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The processed image height.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_iheight")]
    public static extern int GetProcessedImageHeight(IntPtr data);

    /// <summary>
    /// Get the processed image width from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <returns>The processed image width.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_iwidth")]
    public static extern int GetProcessedImageWidth(IntPtr data);

    /// <summary>
    /// Get the camera multiplier value at a specified index from the LibRaw data.
    /// </summary>
    /// <param name="data">The LibRaw data IntPtr.</param>
    /// <param name="index">The index of the camera multiplier value to get.</param>
    /// <returns>The camera multiplier value at the specified index.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_cam_mul")]
    public static extern float GetCameraMultiplier(IntPtr data, int index);

    /// <summary>
    /// Get the pre-multiplier value for a specified color channel.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <param name="index">Color channel index (0-3).</param>
    /// <returns>The pre-multiplier value as a float.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_pre_mul")]
    public static extern float GetPreMultiplier(IntPtr data, int index);

    /// <summary>
    /// Get the RGB camera matrix value for the specified indices.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <param name="index1">Row index (0-2).</param>
    /// <param name="index2">Column index (0-2).</param>
    /// <returns>The RGB camera matrix value as a float.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_rgb_cam")]
    public static extern float GetRgbCameraMatrix(IntPtr data, int index1, int index2);

    /// <summary>
    /// Get the maximum color value for image data.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <returns>The maximum color value as an integer.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_color_maximum")]
    public static extern int GetColorMaximum(IntPtr data);

    /// <summary>
    /// Set the output format to TIFF.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <param name="value">The TIFF value to set.</param>
    [DllImport(Dll, EntryPoint = "libraw_set_output_tif")]
    public static extern void SetOutputTiff(IntPtr data, int value);

    /// <summary>
    /// Get the image parameters stored in the LibRaw data structure.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <returns>Pointer to the image parameters structure.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_iparams")]
    public static extern IntPtr GetImageParameters(IntPtr data);

    /// <summary>
    /// Get the lens information stored in the LibRaw data structure.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <returns>Pointer to the lens information structure.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_lensinfo")]
    public static extern IntPtr GetLensInformation(IntPtr data);

    /// <summary>
    /// Get the image data stored in the LibRaw data structure.
    /// </summary>
    /// <param name="data">Pointer to the LibRaw data structure.</param>
    /// <returns>Pointer to the image data structure.</returns>
    [DllImport(Dll, EntryPoint = "libraw_get_imgother")]
    public static extern IntPtr GetImageOtherParameters(IntPtr data);

    /// <summary>
    /// A PInvoke wrapper for the native "default_data_callback" function in the LibRaw library.
    /// </summary>
    /// <param name="data">A pointer to the data buffer.</param>
    /// <param name="file">The file name associated with the data.</param>
    /// <param name="offset">The offset of the data within the file.</param>
    /// <remarks>
    /// The native "default_data_callback" function is used to process and handle raw image
    /// data from a specified file in a platform-specific manner.
    /// </remarks>
    [DllImport(Dll, EntryPoint = "default_data_callback")]
    public static extern void DefaultDataCallback(IntPtr data, string file, int offset);
    #endregion
}