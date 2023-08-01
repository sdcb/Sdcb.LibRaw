using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class DetailedPropTests : BaseTest
{
    [Fact]
    public void DefaultOutputTiffShouldBeFalse()
    {
        using RawContext r = ExampleBayer();
        Assert.False(r.OutputTiff);
    }

    [Fact]
    public void OutputTiffCanSetToTrue()
    {
        using RawContext r = ExampleBayer();
        r.OutputTiff = true;
        Assert.True(r.OutputTiff);
    }

    [Fact]
    public void OutputTiffCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = ExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputTiff = true;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.True(r.OutputTiff);
    }

    [Fact]
    public void DefaultOutputBitsPerSampleShouldBeFalse()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(8, r.OutputBitsPerSample);
    }

    [Fact]
    public void OutputBitsPerSampleCanSetToTrue()
    {
        using RawContext r = ExampleBayer();
        r.OutputBitsPerSample = 16;
        Assert.Equal(16, r.OutputBitsPerSample);
    }

    [Fact]
    public void OutputBitsPerSampleCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = ExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputBps = 32;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.Equal(32, r.OutputBitsPerSample);
    }

    [Fact]
    public void DefaultOutputColorSpaceShouldBeFalse()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(LibRawColorSpace.SRGB, r.OutputColorSpace);
    }

    [Fact]
    public void OutputColorSpaceCanSetToTrue()
    {
        using RawContext r = ExampleBayer();
        r.OutputColorSpace = LibRawColorSpace.AdobeRgb;
        Assert.Equal(LibRawColorSpace.AdobeRgb, r.OutputColorSpace);
    }

    [Fact]
    public void OutputColorSpaceCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = ExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputColor = LibRawColorSpace.CameraLinear;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.Equal(LibRawColorSpace.CameraLinear, r.OutputColorSpace);
    }

    [Fact]
    public void WhiteBalanceTest()
    {
        using RawContext r = ExampleFile();
        float[] data = r.CameraMultipler.ToArray();
        Assert.Equal(new float[] { 2024, 1024, 2164, 1024 }, data);
    }

    [Fact]
    public void PreMultiplerTest()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(new float[] { 1, 1, 1, 1 }, r.PreMultipler);
    }

    [Fact]
    public void RgbCameraTest()
    {
        using RawContext r = ExampleFile();
        Assert.Equal(new float[]
        {
            1.73653758f, -0.561196744f, -0.175340846f, 0.00000000f,
            -0.153087497f, 1.55836308f, -0.405275583f, 0.00000000f,
            0.0198776610f, -0.404100716f, 1.38422310f, 0.00000000f
        }, r.RgbCamera);
    }

    [Fact]
    public void ColorMax()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(65535, r.ColorMaximum);
    }

    [Fact]
    public void UserMultiplierTest()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(new float[] { 0, 0, 0, 0 }, r.UserMultiplier);
    }

    [Fact]
    public void UserMultiplier_CanBeSet()
    {
        using RawContext r = ExampleBayer();
        r.UserMultiplier[3] = 5;
        Assert.Equal(new float[] { 0, 0, 0, 5 }, r.UserMultiplier);
    }

    [Fact]
    public void DefaultDemosaicAlgorithm()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.Equal(DemosaicAlgorithm.Default, r.DemosaicAlgorithm);
    }

    [Fact]
    public void DemosaicAlgorithm_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.DemosaicAlgorithm = DemosaicAlgorithm.AdaptiveAHD;
        Assert.Equal(DemosaicAlgorithm.AdaptiveAHD, r.DemosaicAlgorithm);
    }

    [Fact]
    public void DefaultAdjustMaxThreshold()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.Equal(0.75f, r.AdjustMaximumThreshold);
    }

    [Fact]
    public void AdjustMaxThreshold_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.AdjustMaximumThreshold = 0;
        Assert.Equal(0, r.AdjustMaximumThreshold);
    }

    [Fact]
    public void DefaultGamma()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.Equal(new float[2] { 0.45f, 4.5f }, r.Gamma);
    }

    [Fact]
    public void Gamma_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.Gamma[0] = 0.25f;
        Assert.Equal(new float[] { 0.25f, 4.5f }, r.Gamma);
    }

    [Fact]
    public void DefaultAutoBright()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.True(r.AutoBright);
    }

    [Fact]
    public void AutoBright_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.AutoBright = false;
        Assert.False(r.AutoBright);
    }

    [Fact]
    public void DefaultBrightness()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.Equal(1, r.Brightness);
    }

    [Fact]
    public void Brightness_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.Brightness = 1.2f;
        Assert.Equal(1.2f, r.Brightness);
    }

    [Fact]
    public void DefaultHightlightMode()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.Equal(0, r.HighlightMode);
    }

    [Fact]
    public void HightlightMode_CanBeSet()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.HighlightMode = 2; // blend_highlights
        Assert.Equal(2, r.HighlightMode);
    }

    [Fact]
    public void ImageParamsTest()
    {
        using RawContext r = ExampleFile();
        LibRawImageParams iparams = r.ImageParams;
        Assert.Equal("", iparams.Guard);
        Assert.Equal("Sony", iparams.Make);
        Assert.Equal("ILCE-7RM3", iparams.Model);
        Assert.Equal("ILCE-7RM3 v3.10", iparams.Software);
        Assert.Equal("Sony", iparams.NormalizedMake);
        Assert.Equal("ILCE-7RM3", iparams.NormalizedModel);
        Assert.Equal(63u, iparams.MakerIndex);
        Assert.Equal(1u, iparams.RawCount);
        Assert.Equal(0u, iparams.DngVersion);
        Assert.Equal(0u, iparams.IsFoveon);
        Assert.Equal(3, iparams.Colors);
        Assert.Equal(3031741620u, iparams.Filters);
        byte[] expectedXtrans = new byte[6 * 6]
        {
                0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0
        };
        Assert.Equal(expectedXtrans, iparams.XTrans);
        Assert.Equal(expectedXtrans, iparams.XTransAbs);
        Assert.Equal("RGBG", iparams.Cdesc);
        Assert.Equal(4097, iparams.XmpLength);
        Assert.StartsWith("<?xpacket", Encoding.UTF8.GetString(iparams.XmpData));
    }

    [Fact]
    public void ImageOtherParamsTest()
    {
        using RawContext r = ExampleFile();
        LibRawImageOtherParams oparams = r.ImageOtherParams;
        const float epsilon = 0.000001f;

        Assert.Equal(100.0, oparams.IsoSpeed, epsilon);
        Assert.Equal(100.0, oparams.IsoSpeed, epsilon);
        Assert.Equal(0.005000, oparams.Shutter, epsilon);
        Assert.Equal(1.200000, oparams.Aperture, epsilon);
        Assert.Equal(50.000000, oparams.FocalLength, epsilon);
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Assert.Equal(1674456985, oparams.Timestamp); // 2023/1/23 6:56:25
        }
        else
        {
            Assert.Equal(1674485785, oparams.Timestamp); // 2023/1/23 14:56:25
        }

        Assert.Equal(0u, oparams.ShotOrder);
        Assert.Equal(new string(' ', 31), oparams.Description);
        Assert.Equal("Zhou Jie/sdcb", oparams.Artist);
        Assert.Equal(0.000000, oparams.AnalogBalance[0]);
        Assert.Equal(0.000000, oparams.AnalogBalance[1]);
        Assert.Equal(0.000000, oparams.AnalogBalance[2]);
        Assert.Equal(0.000000, oparams.AnalogBalance[3]);

        LibRawGPS gps = oparams.ParsedGPS;
        Assert.Equal(0.0f, gps.LatitudeDegrees);
        Assert.Equal(0.0f, gps.LatitudeMinutes);
        Assert.Equal(0.0f, gps.LatitudeSeconds);
        Assert.Equal(0.0f, gps.LongitudeDegrees);
        Assert.Equal(0.0f, gps.LongitudeMinutes);
        Assert.Equal(0.0f, gps.LongitudeSeconds);
        Assert.Equal(0.0f, gps.GPSTimeStampDegrees);
        Assert.Equal(0.0f, gps.GPSTimeStampMinutes);
        Assert.Equal(0.0f, gps.GPSTimeStampSeconds);
        Assert.Equal(0.0f, gps.Altitude);
        Assert.Equal(0, gps.AltitudeReference);
        Assert.Equal(0, gps.LatitudeReference);
        Assert.Equal(0, gps.LongitudeReference);
        Assert.Equal('V', gps.GPSStatus);
        Assert.Equal(1, gps.GPSParsed);
    }

    [Fact]
    public void LensInfoTest()
    {
        using RawContext r = ExampleFile();
        LibRawLensInfo iparams = r.LensInfo;
        const float epsilon = 0.000001f;

        // LibRawLensInfo
        Assert.Equal(50, iparams.MinFocal, epsilon);
        Assert.Equal(50, iparams.MaxFocal, epsilon);
        Assert.Equal(1.2, iparams.MaxAperture4MinFocal, epsilon);
        Assert.Equal(1.2, iparams.MaxAperture4MaxFocal, epsilon);
        Assert.Equal(1.198906, iparams.ExifMaxAperture, epsilon);
        Assert.Equal("", iparams.LensMake);
        Assert.Equal("FE 50mm F1.2 GM", iparams.Lens);
        Assert.Equal("", iparams.LensSerial);
        Assert.Equal("", iparams.InternalLensSerial);
        Assert.Equal(50, iparams.FocalLengthIn35mmFormat);
        // LibRawLensMakerNotes
        LibRawLensMakerNotes makerNotes = iparams.MakerNotes;
        Assert.Equal(32862u, makerNotes.LensID);
        Assert.Equal("", makerNotes.Lens);
        Assert.Equal(2, makerNotes.LensFormat, epsilon);
        Assert.Equal(40, makerNotes.LensMount, epsilon);
        Assert.Equal(362u, makerNotes.CameraId);
        Assert.Equal(2, makerNotes.CameraFormat, epsilon);
        Assert.Equal(40, makerNotes.CameraMount, epsilon);
        Assert.Equal("", makerNotes.Body);
        Assert.Equal(0.0, makerNotes.FocalType, epsilon);
        Assert.Equal("", makerNotes.LensFeaturesPre);
        Assert.Equal("", makerNotes.LensFeaturesSuffix);
        Assert.Equal(0.0, makerNotes.MaxFocal, epsilon);
        Assert.Equal(0.0, makerNotes.MaxAperture4MaxFocal, epsilon);
        Assert.Equal(1.2, makerNotes.MaxAperture4MinFocal, epsilon);
        Assert.Equal(0.0, makerNotes.MinAperture4MinFocal, epsilon);
        Assert.Equal(0.0, makerNotes.MaxAperture, epsilon);
        Assert.Equal(0.0, makerNotes.MinAperture, epsilon);
        Assert.Equal(0.0, makerNotes.CurrentFocal, epsilon);
        Assert.Equal(0.0, makerNotes.CurrentAperture, epsilon);
        Assert.Equal(0.0, makerNotes.MaxAperture4CurrentFocal, epsilon);
        Assert.Equal(0.0, makerNotes.MinAperture4CurrentFocal, epsilon);
        Assert.Equal(0.0, makerNotes.MinFocusDistance, epsilon);
        Assert.Equal(0.0, makerNotes.FocusRangeIndex, epsilon);
        Assert.Equal(0.0, makerNotes.LensFStops, epsilon);
        Assert.Equal(0u, makerNotes.TeleconverterID);
        Assert.Equal("", makerNotes.Teleconverter);
        Assert.Equal(0u, makerNotes.AdapterID);
        Assert.Equal("", makerNotes.Adapter);
        Assert.Equal(0u, makerNotes.AttachmentID);
        Assert.Equal("", makerNotes.Attachment);
        Assert.Equal(1.0, makerNotes.FocalUnits, epsilon);
        Assert.Equal(0.0, makerNotes.FocalLengthIn35mmFormat, epsilon);
        // Dng
        Assert.Equal(0, iparams.Dng.MinFocal);
        Assert.Equal(0, iparams.Dng.MaxFocal);
        Assert.Equal(0, iparams.Dng.MaxAp4MinFocal);
        Assert.Equal(0, iparams.Dng.MaxAp4MaxFocal);
        // Nikon
        Assert.Equal(0, iparams.Nikon.EffectiveMaxAp);
        Assert.Equal(0, iparams.Nikon.LensIDNumber);
        Assert.Equal(0, iparams.Nikon.LensFStops);
        Assert.Equal(0, iparams.Nikon.MCUVersion);
        Assert.Equal(0, iparams.Nikon.LensType);
    }
}
