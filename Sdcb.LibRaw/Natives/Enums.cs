using System;
using System.Collections.Generic;
using System.Text;

namespace Sdcb.LibRaw.Natives;

/// <summary>
/// Represents the open bayer patterns in LibRaw.
/// </summary>
public enum OpenBayerPattern
{
    Rggb = 0x94,
    Bggr = 0x16,
    Grbg = 0x61,
    Gbrg = 0x49
}

/// <summary>
/// Represents the DNG fields marks in LibRaw.
/// </summary>
public enum DngFieldMark
{
    ForwardMatrix = 1,
    Illuminant = 1 << 1,
    ColorMatrix = 1 << 2,
    Calibration = 1 << 3,
    AnalogBalance = 1 << 4,
    Black = 1 << 5,
    White = 1 << 6,
    OpCode2 = 1 << 7,
    LinTable = 1 << 8,
    CropOrigin = 1 << 9,
    CropSize = 1 << 10,
    PreviewCs = 1 << 11,
    AsShotNeutral = 1 << 12,
    BaselineExposure = 1 << 13,
    LinearResponseLimit = 1 << 14,
    UserCrop = 1 << 15,
    OpCode1 = 1 << 16,
    OpCode3 = 1 << 17,
}

/// <summary>
/// Represents the As Shot WB applied codes in LibRaw.
/// </summary>
public enum AsShotWbAppliedCode
{
    Applied = 1,
    Canon = 2,
    Nikon = 4,
    NikonSraw = 8,
    Pentax = 16
}

/// <summary>
/// Represents the Exif tag types in LibRaw.
/// </summary>
public enum ExifTagType
{
    Unknown = 0,
    Byte = 1,
    Ascii = 2,
    Short = 3,
    Long = 4,
    Rational = 5,
    Sbyte = 6,
    Undefined = 7,
    Sshort = 8,
    Slong = 9,
    Srational = 10,
    Float = 11,
    Double = 12,
    Ifd = 13,
    Unicode = 14,
    Complex = 15,
    Long8 = 16,
    Slong8 = 17,
    Ifd8 = 18
}

public static class ExifToolTagType
{
    public const ExifTagType Int8u = ExifTagType.Byte;
    public const ExifTagType String = ExifTagType.Ascii;
    public const ExifTagType Int16u = ExifTagType.Short;
    public const ExifTagType Int32u = ExifTagType.Long;
    public const ExifTagType Rational64u = ExifTagType.Rational;
    public const ExifTagType Int8s = ExifTagType.Sbyte;
    public const ExifTagType Undef = ExifTagType.Undefined;
    public const ExifTagType Binary = ExifTagType.Undefined;
    public const ExifTagType Int16s = ExifTagType.Sshort;
    public const ExifTagType Int32s = ExifTagType.Slong;
    public const ExifTagType Rational64s = ExifTagType.Srational;
    public const ExifTagType Float = ExifTagType.Float;
    public const ExifTagType Double = ExifTagType.Double;
    public const ExifTagType Ifd = ExifTagType.Ifd;
    public const ExifTagType Unicode = ExifTagType.Unicode;
    public const ExifTagType Complex = ExifTagType.Complex;
    public const ExifTagType Int64u = ExifTagType.Long8;
    public const ExifTagType Int64s = ExifTagType.Slong8;
    public const ExifTagType Ifd64 = ExifTagType.Ifd8;
}

/// <summary>
/// Represents the white balance codes in LibRaw.
/// </summary>
public enum WhiteBalanceCode
{
    Unknown = 0,
    Daylight = 1,
    Fluorescent = 2,
    Tungsten = 3,
    Flash = 4,
    FineWeather = 9,
    Cloudy = 10,
    Shade = 11,
    FlD = 12,
    FlN = 13,
    FlW = 14,
    FlWw = 15,
    FlL = 16,
    IllA = 17,
    IllB = 18,
    IllC = 19,
    D55 = 20,
    D65 = 21,
    D75 = 22,
    D50 = 23,
    StudioTungsten = 24,
    Sunset = 64,
    Underwater = 65,
    FluorescentHigh = 66,
    HtMercury = 67,
    AsShot = 81,
    Auto = 82,
    Custom = 83,
    Auto1 = 85,
    Auto2 = 86,
    Auto3 = 87,
    Auto4 = 88,
    Custom1 = 90,
    Custom2 = 91,
    Custom3 = 92,
    Custom4 = 93,
    Custom5 = 94,
    Custom6 = 95,
    PcSet1 = 96,
    PcSet2 = 97,
    PcSet3 = 98,
    PcSet4 = 99,
    PcSet5 = 100,
    Measured = 110,
    Bw = 120,
    Kelvin = 254,
    Other = 255,
    None = 0xffff
}

/// <summary>
/// Represents the multi-exposure related codes in LibRaw.
/// </summary>
public enum MultiExposureRelated
{
    None = 0,
    Simple = 1,
    Overlay = 2,
    Hdr = 3
}

/// <summary>
/// Represents the DNG processing codes in LibRaw.
/// </summary>
public enum DngProcessing
{
    None = 0,
    Float = 1,
    Linear = 2,
    Deflate = 4,
    Xtrans = 8,
    Other = 16,
    Bit8 = 32,
    All = Float | Linear | Deflate | Xtrans | Bit8 | Other,
    Default = Float | Linear | Deflate | Bit8
}

/// <summary>
/// Represents the output flags in LibRaw.
/// </summary>
public enum OutputFlag
{
    None = 0,
    PpmMeta = 1
}

/// <summary>
/// Represents the runtime capabilities in LibRaw.
/// </summary>
public enum RuntimeCapability
{
    RawSpeed = 1,
    DngSdk = 1 << 1,
    GprSdk = 1 << 2,
    UnicodePaths = 1 << 3,
    X3fTools = 1 << 4,
    Rpi6By9 = 1 << 5,
    Zlib = 1 << 6,
    Jpeg = 1 << 7,
    RawSpeed3 = 1 << 8,
    RawSpeedBits = 1 << 9,
}

/// <summary>
/// Represents the color spaces in LibRaw.
/// </summary>
public enum ColorSpace
{
    NotFound = 0,
    Srgb,
    AdobeRgb,
    WideGamutRgb,
    ProPhotoRgb,
    Icc,
    Uncalibrated,
    CameraLinearUniWb,
    CameraLinear,
    CameraGammaUniWb,
    CameraGamma,
    MonochromeLinear,
    MonochromeGamma,
    Unknown = 255
}

/// <summary>
/// Represents the camera maker indexes in LibRaw.
/// </summary>
public enum CameraMakerIndex
{
    Unknown = 0,
    Agfa,
    Alcatel,
    Apple,
    Aptina,
    Avt,
    Baumer,
    Broadcom,
    Canon,
    Casio,
    Cine,
    Clauss,
    Contax,
    Creative,
    Dji,
    Dxo,
    Epson,
    Foculus,
    Fujifilm,
    Generic,
    Gione,
    Gitup,
    Google,
    Gopro,
    Hasselblad,
    Htc,
    IMobile,
    Imacon,
    JkImaging,
    Kodak,
    Konica,
    Leaf,
    Leica,
    Lenovo,
    Lg,
    Logitech,
    Mamiya,
    Matrix,
    Meizu,
    Micron,
    Minolta,
    Motorola,
    Ngm,
    Nikon,
    Nokia,
    Olympus,
    OmniVison,
    Panasonic,
    Parrot,
    Pentax,
    PhaseOne,
    PhotoControl,
    Photron,
    Pixelink,
    Polaroid,
    Red,
    Ricoh,
    Rollei,
    RoverShot,
    Samsung,
    Sigma,
    Sinar,
    SmaL,
    Sony,
    StMicro,
    Thl,
    Vluu,
    Xiaomi,
    Xiaoyi,
    Yi,
    Yuneec,
    Zeiss,
    OnePlus,
    Isg,
    Vivo,
    HmdGlobal,
    Huawei,
    RaspberryPi,
    OmDigital,
    TheLastOne
}

/// <summary>
/// Represents the camera mounts in LibRaw.
/// </summary>
public enum CameraMount
{
    Unknown = 0,
    Alpa,
    C,
    CanonEfM,
    CanonEfS,
    CanonEf,
    CanonRf,
    ContaxN,
    Contax645,
    Ft,
    Mft,
    FujiGf,
    FujiGx,
    FujiX,
    HasselbladH,
    HasselbladV,
    HasselbladXcd,
    LeicaM,
    LeicaR,
    LeicaS,
    LeicaSl,
    LeicaTl,
    LpsL,
    Mamiya67,
    Mamiya645,
    MinoltaA,
    NikonCx,
    NikonF,
    NikonZ,
    PhaseOneIxmMv,
    PhaseOneIxmRs,
    PhaseOneIxm,
    Pentax645,
    PentaxK,
    PentaxQ,
    RicohModule,
    RolleiBayonet,
    SamsungNxM,
    SamsungNx,
    SigmaX3f,
    SonyE,
    Lf,
    DigitalBack,
    FixedLens,
    IlUm,
    TheLastOne
}

/// <summary>
/// Represents the camera formats in LibRaw.
/// </summary>
public enum CameraFormat
{
    Unknown = 0,
    Apsc,
    Ff,
    Mf,
    Apsh,
    OneInch,
    OneDiv2p3Inch,
    OneDiv1p7Inch,
    Ft,
    Crop645,
    LeicaS,
    Format645,
    Format66,
    Format69,
    Lf,
    LeicaDmr,
    Format67,
    SigmaApsc,
    SigmaMerrill,
    SigmaApsh,
    Format3648,
    Format68,
    TheLastOne
}

/// <summary>
/// Represents the image aspects in LibRaw.
/// </summary>
public enum ImageAspect
{
    Unknown = 0,
    Other = 1,
    MinimalRealAspectValue = 99,
    MaximalRealAspectValue = 10000,
    Aspect3to2 = (1000 * 3) / 2,
    Aspect1to1 = 1000,
    Aspect4to3 = (1000 * 4) / 3,
    Aspect16to9 = (1000 * 16) / 9,
    Aspect5to4 = (1000 * 5) / 4,
    Aspect7to6 = (1000 * 7) / 6,
    Aspect6to5 = (1000 * 6) / 5,
    Aspect7to5 = (1000 * 7) / 5
}

/// <summary>
/// Represents the lens focal types in LibRaw.
/// </summary>
public enum LensFocalType
{
    Undefined = 0,
    PrimeLens = 1,
    ZoomLens = 2,
    ZoomLensConstantAperture = 3,
    ZoomLensVariableAperture = 4
}

/// <summary>
/// Represents the Canon record modes in LibRaw.
/// </summary>
public enum CanonRecordMode
{
    Undefined = 0,
    Jpeg,
    CrwThm,
    AviThm,
    Tif,
    TifJpeg,
    Cr2,
    Cr2Jpeg,
    Unknown,
    Mov,
    Mp4,
    Crm,
    Cr3,
    Cr3Jpeg,
    Heif,
    Cr3Heif,
    TheLastOne
}

/// <summary>
/// Represents the Minolta storage methods in LibRaw.
/// </summary>
public enum MinoltaStorageMethod
{
    Unpacked = 0x52,
    Packed = 0x59
}

/// <summary>
/// Represents the Minolta bayer patterns in LibRaw.
/// </summary>
public enum MinoltaBayerPattern
{
    Rggb = 0x01,
    G2brg1 = 0x04
}

/// <summary>
/// Represents the Sony camera types in LibRaw.
/// </summary>
public enum SonyCameraType
{
    Dsc = 1,
    Dslr = 2,
    Nex = 3,
    Slt = 4,
    Ilce = 5,
    Ilca = 6,
    Unknown = 0xffff
}

/// <summary>
/// Represents the Sony 0x2010 types in LibRaw.
/// </summary>
public enum Sony0x2010Type
{
    None = 0,
    TypeA,
    TypeB,
    TypeC,
    TypeD,
    TypeE,
    TypeF,
    TypeG,
    TypeH,
    TypeI
}

/// <summary>
/// Represents the Sony 0x9050 types in LibRaw.
/// </summary>
public enum Sony0x9050Type
{
    None = 0,
    TypeA,
    TypeB,
    TypeC
}

/// <summary>
/// Represents the Sony focus mode modes in LibRaw.
/// </summary>
public enum SonyFocusMode
{
    Mf = 0,
    AfS = 2,
    AfC = 3,
    AfA = 4,
    Dmf = 6,
    AfD = 7,
    Af = 101,
    PermanentAf = 104,
    SemiMf = 105,
    Unknown = -1
}

/// <summary>
/// Represents the Kodak sensors in LibRaw.
/// </summary>
public enum KodakSensor
{
    UnknownSensor = 0,
    M1 = 1,
    M15 = 2,
    M16 = 3,
    M17 = 4,
    M2 = 5,
    M23 = 6,
    M24 = 7,
    M3 = 8,
    M5 = 9,
    M6 = 10,
    C14 = 11,
    X14 = 12,
    M11 = 13
}

/// <summary>
/// Represents the Hasselblad format codes in LibRaw.
/// </summary>
public enum HasselbladFormatCode
{
    Unknown = 0,
    ThreeFr,
    Fff,
    Imacon,
    HasselbladDng,
    AdobeDng,
    AdobeDngFromPhocusDng
}

/// <summary>
/// Represents the raw special types in LibRaw.
/// </summary>
public enum RawSpecialType
{
    SonyArw2None = 0,
    SonyArw2BaseOnly = 1,
    SonyArw2DeltaOnly = 1 << 1,
    SonyArw2DeltaZeroBase = 1 << 2,
    SonyArw2DeltaToValue = 1 << 3,
    SonyArw2AllFlags = SonyArw2BaseOnly + SonyArw2DeltaOnly + SonyArw2DeltaZeroBase + SonyArw2DeltaToValue,
    NoDp2qInterpolateRg = 1 << 4,
    NoDp2qInterpolateAf = 1 << 5,
    SRawNoRgb = 1 << 6,
    SRawNoInterpolate = 1 << 7
}

/// <summary>
/// Represents the raw speed bits in LibRaw.
/// </summary>
public enum RawSpeedBits
{
    RawSpeedV1Use = 1,
    RawSpeedV1FailOnUnknown = 1 << 1,
    RawSpeedV1IgnoreErrors = 1 << 2,
    RawSpeedV3Use = 1 << 8,
    RawSpeedV3FailOnUnknown = 1 << 9,
    RawSpeedV3IgnoreErrors = 1 << 10,
}

/// <summary>
/// Represents the processing options in LibRaw.
/// </summary>
public enum ProcessingOption
{
    PentaxPsAllFrames = 1,
    ConvertFloatToInt = 1 << 1,
    ArqSkipChannelSwap = 1 << 2,
    NoRotateForKodakThumbnails = 1 << 3,
    UsePpm16Thumbs = 1 << 5,
    DontCheckDngIlluminant = 1 << 6,
    DngSdkZeroCopy = 1 << 7,
    ZeroFiltersForMonochromeTiffs = 1 << 8,
    DngAddEnhanced = 1 << 9,
    DngAddPreviews = 1 << 10,
    DngPreferLargestImage = 1 << 11,
    DngStage2 = 1 << 12,
    DngStage3 = 1 << 13,
    DngAllowSizeChange = 1 << 14,
    DngDisableWbAdjust = 1 << 15,
    ProvideNonStandardWb = 1 << 16,
    CameraWbFallbackToDaylight = 1 << 17,
    CheckThumbnailsKnownVendors = 1 << 18,
    CheckThumbnailsAllVendors = 1 << 19,
    DngStage2IfPresent = 1 << 20,
    DngStage3IfPresent = 1 << 21,
    DngAddMasks = 1 << 22,
    CanonIgnoreMakernotesRotation = 1 << 23
}

/// <summary>
/// Represents the decoder flags in LibRaw.
/// </summary>
public enum DecoderFlag
{
    HasCurve = 1 << 4,
    SonyArw2 = 1 << 5,
    TryRawSpeed = 1 << 6,
    OwnAlloc = 1 << 7,
    FixedMaxC = 1 << 8,
    AdobeCopyPixel = 1 << 9,
    LegacyWithMargins = 1 << 10,
    ThreeChannel = 1 << 11,
    Sinar4Shot = 1 << 11,
    FlatData = 1 << 12,
    FlatBg2Swapped = 1 << 13,
    UnsupportedFormat = 1 << 14,
    NotSet = 1 << 15,
    TryRawSpeed3 = 1 << 16
}

/// <summary>
/// Represents the constructor flags in LibRaw.
/// </summary>
public enum ConstructorFlag
{
    None = 0,
    NoDataErrCallback = 1 << 1,
    OptionsNoDataErrCallback = NoDataErrCallback
}

/// <summary>
/// Represents the warnings in LibRaw.
/// </summary>
public enum LibRawWarning
{
    None = 0,
    BadCameraWb = 1 << 2,
    NoMetadata = 1 << 3,
    NoJpegLib = 1 << 4,
    NoEmbeddedProfile = 1 << 5,
    NoInputProfile = 1 << 6,
    BadOutputProfile = 1 << 7,
    NoBadPixelMap = 1 << 8,
    BadDarkFrameFile = 1 << 9,
    BadDarkFrameDim = 1 << 10,
    NoJasper = 1 << 11,
    RawSpeedProblem = 1 << 12,
    RawSpeedUnsupported = 1 << 13,
    RawSpeedProcessed = 1 << 14,
    FallbackToAhd = 1 << 15,
    ParseFujiProcessed = 1 << 16,
    DngSdkProcessed = 1 << 17,
    DngImagesReordered = 1 << 18,
    DngStage2Applied = 1 << 19,
    DngStage3Applied = 1 << 20,
    RawSpeed3Problem = 1 << 21,
    RawSpeed3Unsupported = 1 << 22,
    RawSpeed3Processed = 1 << 23,
    RawSpeed3NotListed = 1 << 24
}

/// <summary>
/// Represents the exceptions in LibRaw.
/// </summary>
public enum LibRawException
{
    None = 0,
    Alloc = 1,
    DecodeRaw = 2,
    DecodeJpeg = 3,
    IoEof = 4,
    IoCorrupt = 5,
    CancelledByCallback = 6,
    BadCrop = 7,
    IoBadFile = 8,
    DecodeJpeg2000 = 9,
    TooBig = 10,
    Mempool = 11,
    UnsupportedFormat = 12
}

/// <summary>
/// Represents the progress stages in LibRaw.
/// </summary>
public enum LibRawProgress
{
    Start = 0,
    Open = 1,
    Identify = 1 << 1,
    SizeAdjust = 1 << 2,
    LoadRaw = 1 << 3,
    Raw2Image = 1 << 4,
    RemoveZeroes = 1 << 5,
    BadPixels = 1 << 6,
    DarkFrame = 1 << 7,
    FoveonInterpolate = 1 << 8,
    ScaleColors = 1 << 9,
    PreInterpolate = 1 << 10,
    Interpolate = 1 << 11,
    MixGreen = 1 << 12,
    MedianFilter = 1 << 13,
    Highlights = 1 << 14,
    FujiRotate = 1 << 15,
    Flip = 1 << 16,
    ApplyProfile = 1 << 17,
    ConvertRgb = 1 << 18,
    Stretch = 1 << 19,
    Stage20 = 1 << 20,
    Stage21 = 1 << 21,
    Stage22 = 1 << 22,
    Stage23 = 1 << 23,
    Stage24 = 1 << 24,
    Stage25 = 1 << 25,
    Stage26 = 1 << 26,
    Stage27 = 1 << 27,
    ThumbLoad = 1 << 28,
    TReserved1 = 1 << 29,
    TReserved2 = 1 << 30
}

/// <summary>
/// Represents the error codes in LibRaw.
/// </summary>
public enum LibRawError
{
    Success = 0,
    UnspecifiedError = -1,
    FileUnsupported = -2,
    RequestForNonexistentImage = -3,
    OutOfOrderCall = -4,
    NoThumbnail = -5,
    UnsupportedThumbnail = -6,
    InputClosed = -7,
    NotImplemented = -8,
    RequestForNonexistentThumbnail = -9,
    InsufficientMemory = -100007,
    DataError = -100008,
    IOError = -100009,
    CancelledByCallback = -100010,
    BadCrop = -100011,
    TooBig = -100012,
    MempoolOverflow = -100013
}

/// <summary>
/// Represents the internal thumbnail formats in LibRaw.
/// </summary>
public enum InternalThumbnailFormat
{
    Unknown = 0,
    KodakThumb = 1,
    KodakYCbCr = 2,
    KodakRgb = 3,
    Jpeg = 4,
    Layer,
    Rollei,
    Ppm,
    Ppm16,
    X3f,
}

/// <summary>
/// Represents the thumbnail formats in LibRaw.
/// </summary>
public enum ThumbnailFormat
{
    Unknown = 0,
    Jpeg = 1,
    Bitmap = 2,
    Bitmap16 = 3,
    Layer = 4,
    Rollei = 5,
    H265 = 6
}

/// <summary>
/// Represents the image formats in LibRaw.
/// </summary>
public enum ImageFormat
{
    Jpeg = 1,
    Bitmap = 2
}