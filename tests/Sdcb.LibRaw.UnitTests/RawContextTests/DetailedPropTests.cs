using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

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
        Assert.Equal(LibRawColorSpace.Srgb, r.OutputColorSpace);
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
        data.OutputParams.OutputColor = (int)LibRawColorSpace.CameraLinear;
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
    public void WhiteBalance_Should_CanBeSet()
    {
        using RawContext r = ExampleBayer();
        r.CameraMultipler[0] = 5;
        r.CameraMultipler[3] = 15;
        Assert.Equal(new float[] { 5, 0, 0, 15 }, r.CameraMultipler);
    }

    [Fact]
    public void PreMultiplerTest()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(new float[] { 1, 1, 1, 1 }, r.PreMultipler);
    }

    [Fact]
    public void PreMultipler_CanBeSet()
    {
        using RawContext r = ExampleBayer();
        r.PreMultipler[0] = 5;
        r.PreMultipler[3] = 15;
        Assert.Equal(new float[] { 5, 1, 1, 15 }, r.PreMultipler);
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
    public void RgbCamera_CanBeSet()
    {
        using RawContext r = ExampleBayer();
        r.RgbCamera[1, 2] = 3.14f;
        float[] expected = new float[12];
        expected[1 * 4 + 2] = 3.14f; ;
        Assert.Equal(expected, r.RgbCamera);
    }

    [Fact]
    public void ColorMax()
    {
        using RawContext r = ExampleBayer();
        Assert.Equal(65535, r.ColorMaximum);
    }

    [Fact]
    public void ColorMax_CanBeSet()
    {
        using RawContext r = ExampleBayer();
        r.ColorMaximum = 123;
        Assert.Equal(123, r.ColorMaximum);
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
    public void DefaultInterpolation()
    {
        using RawContext r = new(LibRawNative.Initialize());
        Assert.True(r.Interpolation);
    }

    [Fact]
    public void Interpolation_CanBeClose()
    {
        using RawContext r = new(LibRawNative.Initialize());
        r.Interpolation = false;
        Assert.False(r.Interpolation);
    }
}
