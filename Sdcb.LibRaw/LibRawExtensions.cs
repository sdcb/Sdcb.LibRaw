using Sdcb.LibRaw.Natives;
using System;
using System.IO;
using System.Runtime.Versioning;

namespace Sdcb.LibRaw;

/// <summary>
/// Provides extension methods for the LibRaw library.
/// </summary>
public static class LibRawExtensions
{
    /// <summary>
    /// Exports a Raw image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <param name="configure">The OutputParams configuration action.</param>
    /// <returns>The ProcessedImage object.</returns>
    public static ProcessedImage ExportRawImage(this RawContext r, Action<OutputParams> configure)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        if (((int)r.Progress & LibRawNative.ProgressThumbMask) < (int)LibRawProgress.LoadRaw)
        {
            r.Unpack();
        }
        r.DcrawProcess(configure);
        return r.MakeDcrawMemoryImage();
    }

    /// <summary>
    /// Exports a Raw image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <returns>The ProcessedImage object.</returns>
    public static ProcessedImage ExportRawImage(this RawContext r)
    {
        PreCheckAndUnpack(r);
        r.DcrawProcess();
        return r.MakeDcrawMemoryImage();
    }

    private static void PreCheckAndUnpack(RawContext r)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        if (((int)r.Progress & LibRawNative.ProgressThumbMask) < (int)LibRawProgress.LoadRaw)
        {
            r.Unpack();
        }
    }

    /// <summary>
    /// Exports a thumbnail image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <param name="thumbnailIndex">The thumbnail index.</param>
    /// <returns>The ProcessedImage object.</returns>
    public static ProcessedImage ExportThumbnail(this RawContext r, int thumbnailIndex = 0)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        r.UnpackThumbnail(thumbnailIndex);
        return r.MakeDcrawMemoryThumbnail();
    }

    /// <summary>
    /// Saves a Raw image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <param name="fileName">The output file name.</param>
    /// <param name="configure">The OutputParams configuration action, only supported in windows.</param>
    public static void SaveRawImage(this RawContext r, string fileName, Action<OutputParams> configure)
    {
        PreCheckAndUnpack(r, fileName);

        r.DcrawProcess(configure);
        r.WriteDcrawPpmTiff(fileName);
    }

    /// <summary>
    /// Saves a Raw image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <param name="fileName">The output file name.</param>
    public static void SaveRawImage(this RawContext r, string fileName)
    {
        PreCheckAndUnpack(r, fileName);

        r.DcrawProcess();
        r.WriteDcrawPpmTiff(fileName);
    }

    private static void PreCheckAndUnpack(RawContext r, string fileName)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        string ext = Path.GetExtension(fileName);
        if (ext.Equals(".tiff", StringComparison.OrdinalIgnoreCase) || ext.Equals(".tif", StringComparison.OrdinalIgnoreCase))
        {
            r.OutputTiff = true;
        }
        else if (!ext.Equals(".ppm", StringComparison.OrdinalIgnoreCase))
        {
            throw new LibRawException($"Unsupported file extension {ext}, supported formats: .ppm|.tif|.tiff");
        }

        if (((int)r.Progress & LibRawNative.ProgressThumbMask) < (int)LibRawProgress.LoadRaw)
        {
            r.Unpack();
        }
    }

    /// <summary>
    /// Saves a thumbnail image.
    /// </summary>
    /// <param name="r">The RawContext object.</param>
    /// <param name="fileName">The output file name.</param>
    /// <param name="thumbnailIndex">The thumbnail index.</param>
    public static void SaveThumbnail(this RawContext r, string fileName, int thumbnailIndex = 0)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        r.UnpackThumbnail(thumbnailIndex);
        r.WriteDcrawThumbnail(fileName);
    }
}
