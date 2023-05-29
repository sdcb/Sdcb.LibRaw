using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives;

/// <summary>
/// Represents an image that has been processed by LibRaw.
/// </summary>
/// <remarks></remarks>
[StructLayout(LayoutKind.Sequential)]
public ref struct LibRawProcessedImage
{
    /// <summary>
    /// The format of the image.
    /// </summary>
    public ImageFormat Type;

    /// <summary>
    /// The height of the image in pixels.
    /// </summary>
    public ushort Height;

    /// <summary>
    /// The width of the image in pixels.
    /// </summary>
    public ushort Width;

    /// <summary>
    /// The number of colors in the image.
    /// </summary>
    public ushort Colors;

    /// <summary>
    /// The number of bits per pixel in the image.
    /// </summary>
    public ushort Bits;

    /// <summary>
    /// The size of the image data in bytes.
    /// </summary>
    public int DataSize;

    /// <summary>
    /// The first byte of the image data.
    /// </summary>
    public byte FirstData;

    /// <summary>
    /// Gets the image data in the specified format.
    /// </summary>
    /// <typeparam name="T">The type of the image data.</typeparam>
    /// <returns>A span containing the image data in the specified format.</returns>
    public unsafe Span<T> GetData<T>()
    {
        fixed (void* pfirstData = &FirstData)
        {
            return new Span<T>(pfirstData, DataSize / Marshal.SizeOf<T>());
        }
    }
}


/// <summary>
/// Represents the parameters for a LibRaw image.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawImageParams
{
    /// <summary>
    /// Verify the integrity of the LibRawImageParams structure.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
    public string Guard;

    /// <summary>
    /// Camera manufacturer name.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Make;

    /// <summary>
    /// Camera model name.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Model;

    /// <summary>
    /// Software used to acquire the raw data.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Software;

    /// <summary>
    /// Normalized representation of the manufacturer name.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string NormalizedMake;

    /// <summary>
    /// Normalized representation of the model name.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string NormalizedModel;

    /// <summary>
    /// Used internally by the raw processing software, and does not have any intrinsic meaning.
    /// </summary>
    public uint MakerIndex;

    /// <summary>
    /// Specifies the number of raw files that were used in the image.
    /// </summary>
    public uint RawCount;

    /// <summary>
    /// Version number of the DNG specification that was used to create the image.
    /// </summary>
    public uint DngVersion;

    /// <summary>
    /// Indicates whether the image was taken with a Foveon sensor.
    /// </summary>
    public uint IsFoveon;

    /// <summary>
    /// Number of distinct colors that can be represented by the image's data. This value is a power of two.
    /// </summary>
    public int Colors;

    /// <summary>
    /// Color filter array pattern used to obtain the image.
    /// </summary>
    public uint Filters;

    /// <summary>
    /// 6x6 Bayer pattern of the color filter array for Fujifilm X-Trans sensors. The values are defined as follows (in the order R,G,R,G..., etc.): R-G, G, B-G, G-R, B-G, G.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
    public byte[] XTrans;

    /// <summary>
    /// Same as XTrans, but each value is given such that it is relative to the top-left pixel of the sensor.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
    public byte[] XTransAbs;

    /// <summary>
    /// Used internally by the raw processing software. It does not have any intrinsic meaning.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
    public string Cdesc;

    /// <summary>
    /// The size of the XmpData field in bytes.
    /// </summary>
    public int XmpLength;

    /// <summary>
    /// Pointer to the XMP metadata for the image.
    /// </summary>
    public IntPtr XmpDataPtr;

    /// <summary>
    /// Span object that contains the XMP metadata for the image, obtained by dereferencing the pointer in XmpDataPtr.
    /// </summary>
    public unsafe Span<byte> XmpData => XmpDataPtr != IntPtr.Zero ? new Span<byte>(XmpDataPtr.ToPointer(), XmpLength) : null;
}
