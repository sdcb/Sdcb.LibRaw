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
        data.OutputParams.OutputTiff = 2;
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
}
