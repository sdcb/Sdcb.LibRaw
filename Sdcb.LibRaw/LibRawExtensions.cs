using Sdcb.LibRaw.Natives;
using System;
using System.IO;

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
    public static ProcessedImage ExportRawImage(this RawContext r, Action<OutputParams>? configure = null)
    {
        if (r is null) throw new ArgumentNullException(nameof(r));

        if (((int)r.RawData.ProgressFlags & LibRawNative.ProgressThumbMask) < (int)LibRawProgress.LoadRaw)
        {
            r.Unpack();
        }
        r.DcrawProcess(configure);
        return r.MakeDcrawMemoryImage();
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
    /// <param name="configure">The OutputParams configuration action.</param>
    public static void SaveRawImage(this RawContext r, string fileName, Action<OutputParams>? configure = null)
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

        if (((int)r.RawData.ProgressFlags & LibRawNative.ProgressThumbMask) < (int)LibRawProgress.LoadRaw)
        {
            r.Unpack();
        }

        r.DcrawProcess(configure);
        r.WriteDcrawPpmTiff(fileName);
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
