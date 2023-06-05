using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives;

/// <summary>
/// Represents an image that has been processed by LibRaw.
/// </summary>
/// <remarks>Original C API struct: libraw_processed_image_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawProcessedImage
{
    /// <summary>
    /// The format of the image.
    /// </summary>
    public ProcessedImageType Type;

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

/// <summary>Represents the parameters for a LibRaw image.</summary>
/// <remarks>Original C API struct: libraw_iparams_t</remarks>
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

/// <summary>Struct containing lens maker notes data.</summary>
/// <remarks>Original C API struct: libraw_makernotes_lens_t</remarks>
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

/// <summary>Represents a LibRaw decoder information structure.</summary>
/// <remarks>Provides details about specific LibRaw decoder, including the decoder's name and associated flags.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawDecoderInfo
{
    /// <summary>
    /// The name of the LibRaw decoder.
    /// </summary>
    public IntPtr DecoderName;

    /// <summary>
    /// The flags associated with the LibRaw decoder.
    /// </summary>
    public DecoderFlag DecoderFlags;
}

/// <remarks>Original C API struct: libraw_raw_inset_crop_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawInsetCrop
{
    public ushort CLeft;
    public ushort CTop;
    public ushort CWidth;
    public ushort CHeight;
}

/// <remarks>Original C API struct: libraw_image_sizes_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawImageSizes
{
    public ushort RawHeight;
    public ushort RawWidth;
    public ushort Height;
    public ushort Width;
    public ushort TopMargin;
    public ushort LeftMargin;
    public ushort IHeight;
    public ushort IWidth;
    public uint RawPitch;
    public double PixelAspect;
    public int Flip;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 4)]
    public int[] Mask;

    public ushort RawAspect;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public LibRawInsetCrop[] RawInsetCrops;
}

/// <remarks>Original C API struct: libraw_shootinginfo_t</remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawShootingInfo
{
    public short DriveMode;
    public short FocusMode;
    public short MeteringMode;
    public short AFPoint;
    public short ExposureMode;
    public short ExposureProgram;
    public short ImageStabilization;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string BodySerial;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string InternalBodySerial;
}

/// <remarks>Original C API struct: libraw_area_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawArea
{
    public short Top;
    public short Left;
    public short Bottom;
    public short Right;
}

/// <remarks>Original C API struct: libraw_canon_makernotes_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawCanonMakerNotes
{
    public int ColorDataVer;
    public int ColorDataSubVer;
    public int SpecularWhiteLevel;
    public int NormalWhiteLevel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] ChannelBlackLevel;
    public int AverageBlackLevel;

    // Multishot
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] Multishot;

    // Metering
    public short MeteringMode;
    public short SpotMeteringMode;
    public byte FlashMeteringMode;
    public short FlashExposureLock;
    public short ExposureMode;
    public short AESetting;

    // Stabilization
    public short ImageStabilization;

    // Flash
    public short FlashMode;
    public short FlashActivity;
    public short FlashBits;
    public short ManualFlashOutput;
    public short FlashOutput;
    public short FlashGuideNumber;

    // Drive
    public short ContinuousDrive;

    // Sensor
    public short SensorWidth;
    public short SensorHeight;

    public int AFMicroAdjMode;
    public float AFMicroAdjValue;
    public short MakernotesFlip;
    public short RecordMode;
    public short SRAWQuality;
    public uint Wbi;
    public short RFLensID;
    public int AutoLightingOptimizer;
    public int HighlightTonePriority;

    /// <summary>
    /// -1 = n/a, 1 = Economy, 2 = Normal, 3 = Fine, 4 = RAW, 5 = Superfine, 7 = CRAW, 130 = Normal Movie, CRM LightRaw, 131 = CRM StandardRaw
    /// </summary>
    public short Quality;

    /// <summary>
    /// Data compression curve: 0 = OFF, 1 = CLogV1, 2 = CLogV2?, 3 = CLogV3
    /// </summary>
    public int CanonLog;

    public LibRawArea DefaultCropAbsolute;
    public LibRawArea RecommendedImageArea;   // Contains the image in proper aspect ratio?
    public LibRawArea LeftOpticalBlack;       // Use this, when present, to estimate black levels?
    public LibRawArea UpperOpticalBlack;
    public LibRawArea ActiveArea;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public short[] ISOGain; // AutoISO & BaseISO per ExifTool
}

/// <remarks>Original C API struct: libraw_sensor_highspeed_crop_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawSensorHighSpeedCrop
{
    public ushort CLeft;
    public ushort CTop;
    public ushort CWidth;
    public ushort CHeight;
}

/// <remarks>Original C API struct: libraw_nikon_makernotes_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawNikonMakerNotes
{
    public double ExposureBracketValue;
    public ushort ActiveDLighting;
    public ushort ShootingMode;

    // Stabilization
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public byte[] ImageStabilization;
    public byte VibrationReduction;
    public byte VRMode;

    // Flash
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public char[] FlashSetting;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public char[] FlashType;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] FlashExposureCompensation;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] ExternalFlashExposureComp;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] FlashExposureBracketValue;
    public byte FlashMode;
    public sbyte FlashExposureCompensation2;
    public sbyte FlashExposureCompensation3;
    public sbyte FlashExposureCompensation4;
    public byte FlashSource;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] FlashFirmware;
    public byte ExternalFlashFlags;
    public byte FlashControlCommanderMode;
    public byte FlashOutputAndCompensation;
    public byte FlashFocalLength;
    public byte FlashGNDistance;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] FlashGroupControlMode;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] FlashGroupOutputAndCompensation;
    public byte FlashColorFilter;

    // NEF Compression
    public ushort NEFCompression;

    public int ExposureMode;
    public int ExposureProgram;
    public int NMEshots;
    public int MEgainOn;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public double[] ME_WB;
    public byte AFFineTune;
    public byte AFFineTuneIndex;
    public sbyte AFFineTuneAdj;
    public uint LensDataVersion;
    public uint FlashInfoVersion;
    public uint ColorBalanceVersion;
    public byte Key;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] NEFBitDepth;
    public ushort HighSpeedCropFormat;
    public LibRawSensorHighSpeedCrop SensorHighSpeedCrop;
    public ushort SensorWidth;
    public ushort SensorHeight;
    public ushort Active_D_Lighting;
    public uint ShotInfoVersion;
    public short MakerNotesFlip;
    public double RollAngle;  // Positive is clockwise, CW
    public double PitchAngle; // Positive is upwards
    public double YawAngle;   // Positive is to the right
}

/// <remarks>Original C API struct: libraw_hasselblad_makernotes_t</remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawHasselbladMakerNotes
{
    public int BaseISO;
    public double Gain;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
    public string Sensor;

    /// <summary>
    /// Sensor Unit
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string SensorUnit;

    /// <summary>
    /// Host Body
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string HostBody;

    public int SensorCode;
    public int SensorSubCode;
    public int CoatingCode;
    public int Uncropped;

    /// <summary>
    /// Capture Sequence Initiator is based on the content of the 'model' tag
    /// - values like 'Pinhole', 'Flash Sync', '500 Mech.' etc in .3FR 'model' tag
    /// come from MAIN MENU > SETTINGS > Camera;
    /// - otherwise 'model' contains:
    /// 1. if CF/CFV/CFH, SU enclosure, can be with SU type if '-' is present
    /// 2. else if '-' is present, HB + SU type;
    /// 3. HB;
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string CaptureSequenceInitiator;

    /// <summary>
    /// Sensor Unit Connector, makernotes 0x0015 tag:
    /// - in .3FR - SU side
    /// - in .FFF - HB side
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string SensorUnitConnector;

    public int Format; // 3FR, FFF, Imacon (H3D-39 and maybe others), Hasselblad/Phocus DNG, Adobe DNG
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public int[] NIFD_CM; // number of IFD containing CM
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public int[] RecommendedCrop;

    /// <summary>
    /// mnColorMatrix is in makernotes tag 0x002a;
    /// not present in .3FR files and Imacon/H3D-39 .FFF files;
    /// when present in .FFF and Phocus .DNG files, it is a copy of CM1 from .3FR;
    /// available samples contain all '1's in the first 3 elements
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public double[] MnColorMatrix;
}

/// <remarks>Original C API struct: libraw_fuji_info_t</remarks> 
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LibRawFujiInfo
{
    public float ExpoMidPointShift;
    public ushort DynamicRange;
    public ushort FilmMode;
    public ushort DynamicRangeSetting;
    public ushort DevelopmentDynamicRange;
    public ushort AutoDynamicRange;
    public ushort DRangePriority;
    public ushort DRangePriorityAuto;
    public ushort DRangePriorityFixed;

    /// <summary>
    /// tag 0x9200, converted to BrightnessCompensation
    /// F700, S3Pro, S5Pro, S20Pro, S200EXR
    /// E550, E900, F810, S5600, S6500fd, S9000, S9500, S100FS
    /// </summary>
    public float BrightnessCompensation; // in EV, if =4, raw data * 2^4

    public ushort FocusMode;
    public ushort AFMode;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public ushort[] FocusPixel;
    public ushort PrioritySettings;
    public uint FocusSettings;
    public uint AF_C_Settings;
    public ushort FocusWarning;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public ushort[] ImageStabilization;
    public ushort FlashMode;
    public ushort WB_Preset;

    /// <summary>
    /// ShutterType:
    /// 0 - mechanical
    /// 1 = electronic
    /// 2 = electronic, long shutter speed
    /// 3 = electronic, front curtain
    /// </summary>
    public ushort ShutterType;
    public ushort ExrMode;
    public ushort Macro;
    public uint Rating;

    /// <summary>
    /// CropMode:
    /// 1 - FF on GFX,
    /// 2 - sports finder (mechanical shutter),
    /// 4 - 1.25x crop (electronic shutter, continuous high)
    /// </summary>
    public ushort CropMode;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x0c + 1)]
    public string SerialSignature;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4 + 1)]
    public string SensorID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4 + 1)]
    public string RAFVersion;
    public int RAFDataGeneration; // 0 (none), 1..4, 4096
    public ushort RAFDataVersion;
    public int IsTSNERDTS;

    /// <summary>
    /// DriveMode:
    /// 0 - single frame
    /// 1 - continuous low
    /// 2 - continuous high
    /// </summary>
    public short DriveMode;

    /// <summary>
    /// tag 0x4000 BlackLevel:
    /// S9100, S9000, S7000, S6000fd, S5200, S5100, S5000,
    /// S5Pro, S3Pro, S2Pro, S20Pro,
    /// S200EXR, S100FS,
    /// F810, F700,
    /// E900, E550,
    /// DBP, and aliases for all of the above
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
    public ushort[] BlackLevel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public uint[] RAFData_ImageSizeTable;
    public int AutoBracketing;
    public int SequenceNumber;
    public int SeriesLength;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] PixelShiftOffset;
    public int ImageCount;
}

/// <remarks>Original C API struct: libraw_makernotes_t</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct LibRawMakerNotes
{
    public LibRawCanonMakerNotes Canon;
    public LibRawNikonMakerNotes Nikon;
    public LibRawHasselbladMakerNotes Hasselblad;
    public LibRawFujiInfo Fuji;
    public LibRawOlympusMakerNotes Olympus;
    public LibRawSonyInfo Sony;
    public LibRawKodakMakerNotes Kodak;
    public LibRawPanasonicMakerNotes Panasonic;
    public LibRawPentaxMakerNotes Pentax;
    public LibRawP1MakerNotes PhaseOne;
    public LibRawRicohMakerNotes Ricoh;
    public LibRawSamsungMakerNotes Samsung;
    public LibRawMetadataCommon Common;
}

[StructLayout(LayoutKind.Sequential)]
public struct LibRawData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] Image;

    public LibRawImageSizes ImageSizes;

    public LibRawImageParams ImageParams;

    public LibRawLensInfo LensInfo;

    public LibRawMakerNotes MakerNotes;

    //public LibRawShootingInfo ShootingInfo;

    //public LibRawOutputParams Params;

    //public LibRawRawUnpackParams RawParams;

    //public uint ProgressFlags;

    //public uint ProcessWarnings;

    //public LibRawColorData Color;

    //public LibRawImgOther Other;

    //public LibRawThumbnail Thumbnail;

    //public LibRawThumbnailList ThumbsList;

    //public LibRawRawData RawData;

    //public IntPtr ParentClass;
}