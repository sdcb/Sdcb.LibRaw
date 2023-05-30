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

/// <summary>
/// Struct containing lens maker notes data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawLensMakerNotes
{
    /// <summary>Lens ID.</summary>

    public ulong LensID;

    /// <summary>Lens maker name.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Lens;

    /// <summary>Lens format.</summary>
    public ushort LensFormat;

    /// <summary>Lens mount.</summary>
    public ushort LensMount;

    /// <summary>Camera ID.</summary>
    public ulong CameraId;

    /// <summary>Camera format.</summary>
    public ushort CameraFormat;

    /// <summary>Camera mount.</summary>
    public ushort CameraMount;

    /// <summary>Camera body.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Body;

    /// <summary>Focal type.</summary>
    public short FocalType;

    /// <summary>Lens features prefix.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string LensFeaturesPre;

    /// <summary>Lens features suffix.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string LensFeaturesSuffix;

    /// <summary>Minimum focal length.</summary>
    public float MinFocal;

    /// <summary>Maximum focal length.</summary>
    public float MaxFocal;

    /// <summary>Maximum aperture for minimum focal length.</summary>
    public float MaxAperture4MinFocal;

    /// <summary>Maximum aperture for maximum focal length.</summary>
    public float MaxAperture4MaxFocal;

    /// <summary>Minimum aperture for minimum focal length.</summary>
    public float MinAperture4MinFocal;

    /// <summary>Minimum aperture for maximum focal length.</summary>
    public float MinAperture4MaxFocal;

    /// <summary>Maximum aperture.</summary>
    public float MaxAperture;

    /// <summary>Minimum aperture.</summary>
    public float MinAperture;

    /// <summary>Current focal length.</summary>
    public float CurrentFocal;

    /// <summary>Current aperture.</summary>
    public float CurrentAperture;

    /// <summary>Maximum aperture for current focal length.</summary>
    public float MaxAperture4CurrentFocal;

    /// <summary>Minimum aperture for current focal length.</summary>
    public float MinAperture4CurrentFocal;

    /// <summary>Minimum focus distance.</summary>
    public float MinFocusDistance;

    /// <summary>Focus range index.</summary>
    public float FocusRangeIndex;

    /// <summary>Lens f-stops.</summary>
    public float LensFStops;

    /// <summary>Teleconverter ID.</summary>
    public ulong TeleconverterID;

    /// <summary>Teleconverter maker name.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Teleconverter;

    /// <summary>Adapter ID.</summary>
    public ulong AdapterID;

    /// <summary>Adapter maker name.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Adapter;

    /// <summary>Attachment ID.</summary>
    public ulong AttachmentID;

    /// <summary>Attachment maker name.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Attachment;

    /// <summary>Focal units.</summary>
    public ushort FocalUnits;

    /// <summary>Focal length in 35mm format.</summary>
    public float FocalLengthIn35mmFormat;
}

/// <summary>
/// Struct representing Nikon lens information
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawNikonLens
{
    /// <summary>
    /// The maximum effective aperture value of the lens.
    /// </summary>
    public float EffectiveMaxAp;

    /// <summary>
    /// The identification number of the lens.
    /// </summary>
    public byte LensIDNumber;

    /// <summary>
    /// The number of F-stops of the lens.
    /// </summary>
    public byte LensFStops;

    /// <summary>
    /// The version of microcontroller unit.
    /// </summary>
    public byte MCUVersion;

    /// <summary>
    /// The type of lens.
    /// </summary>
    public byte LensType;
};

/// <summary>
/// Struct representing lens information for DNG files.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawDngLens
{
    /// <summary>
    /// The minimum focal length of the lens.
    /// </summary>
    public float MinFocal;

    /// <summary>
    /// The maximum focal length of the lens.
    /// </summary>
    public float MaxFocal;

    /// <summary>
    /// The maximum aperture for the minimum focal length of the lens.
    /// </summary>
    public float MaxAp4MinFocal;

    /// <summary>
    /// The maximum aperture for the maximum focal length of the lens.
    /// </summary>
    public float MaxAp4MaxFocal;
};

/// <summary>
/// Structure containing lens metadata information.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawLensInfo
{
    /// <summary>
    /// Minimum focal length of the lens.
    /// </summary>
    public float MinFocal;

    /// <summary>
    /// Maximum focal length of the lens.
    /// </summary>
    public float MaxFocal;

    /// <summary>
    /// Maximum aperture of the lens for the minimum focal length.
    /// </summary>
    public float MaxAperture4MinFocal;

    /// <summary>
    /// Maximum aperture of the lens for the maximum focal length.
    /// </summary>
    public float MaxAperture4MaxFocal;

    /// <summary>
    /// Maximum aperture value from image metadata.
    /// </summary>
    public float ExifMaxAperture;

    /// <summary>
    /// Lens make information.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string LensMake;

    /// <summary>
    /// Lens information.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Lens;

    /// <summary>
    /// Lens serial number.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string LensSerial;

    /// <summary>
    /// Internal lens serial (Metabones adapter serial number or similar).
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string InternalLensSerial;

    /// <summary>
    /// Focal length in 35mm format.
    /// </summary>
    public ushort FocalLengthIn35mmFormat;

    /// <summary>
    /// Nikon lens information.
    /// </summary>
    public LibRawNikonLens Nikon;

    /// <summary>
    /// DNG lens information.
    /// </summary>
    public LibRawDngLens Dng;

    /// <summary>
    /// Lens maker notes.
    /// </summary>
    public LibRawLensMakerNotes MakerNotes;
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawGPS
{
    public float LatitudeDegrees;
    public float LatitudeMinutes;
    public float LatitudeSeconds;
    public float LongitudeDegrees;
    public float LongitudeMinutes;
    public float LongitudeSeconds;
    public float GPSTimeStampDegrees;
    public float GPSTimeStampMinutes;
    public float GPSTimeStampSeconds;
    public float Altitude;
    public byte AltitudeReference;
    public byte LatitudeReference;
    public byte LongitudeReference;
    public char GPSStatus;
    public byte GPSParsed;
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawImageOtherParams
{
    public float IsoSpeed;
    public float Shutter;
    public float Aperture;
    public float FocalLength;
    public long Timestamp;
    public uint ShotOrder;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public uint[] GpsData;
    public LibRawGPS ParsedGPS;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
    public string Description;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Artist;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] AnalogBalance;
};