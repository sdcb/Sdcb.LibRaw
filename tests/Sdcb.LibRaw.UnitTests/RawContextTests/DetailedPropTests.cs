using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class DetailedPropTests
{
    private static RawContext CreateExampleBayer()
    {
        return RawContext.OpenBayerData<ushort>(new ushort[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 4, 4);
    }

    [Fact]
    public void DefaultOutputTiffShouldBeFalse()
    {
        using RawContext r = CreateExampleBayer();
        Assert.False(r.OutputTiff);
    }

    [Fact]
    public void OutputTiffCanSetToTrue()
    {
        using RawContext r = CreateExampleBayer();
        r.OutputTiff = true;
        Assert.True(r.OutputTiff);
    }

    [Fact]
    public void OutputTiffCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = CreateExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputTiff = 2;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.True(r.OutputTiff);
    }

    [Fact]
    public void DefaultOutputBitsPerSampleShouldBeFalse()
    {
        using RawContext r = CreateExampleBayer();
        Assert.Equal(8, r.OutputBitsPerSample);
    }

    [Fact]
    public void OutputBitsPerSampleCanSetToTrue()
    {
        using RawContext r = CreateExampleBayer();
        r.OutputBitsPerSample = 16;
        Assert.Equal(16, r.OutputBitsPerSample);
    }

    [Fact]
    public void OutputBitsPerSampleCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = CreateExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputBps = 32;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.Equal(32, r.OutputBitsPerSample);
    }

    [Fact]
    public void DefaultOutputColorSpaceShouldBeFalse()
    {
        using RawContext r = CreateExampleBayer();
        Assert.Equal(LibRawColorSpace.Srgb ,r.OutputColorSpace);
    }

    [Fact]
    public void OutputColorSpaceCanSetToTrue()
    {
        using RawContext r = CreateExampleBayer();
        r.OutputColorSpace = LibRawColorSpace.AdobeRgb;
        Assert.Equal(LibRawColorSpace.AdobeRgb, r.OutputColorSpace);
    }

    [Fact]
    public void OutputColorSpaceCanSetToTrueUsingTechnicalWay()
    {
        using RawContext r = CreateExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(r.UnsafeGetHandle());
        data.OutputParams.OutputColor = (int)LibRawColorSpace.CameraLinear;
        Marshal.StructureToPtr(data, r.UnsafeGetHandle(), fDeleteOld: false);
        Assert.Equal(LibRawColorSpace.CameraLinear, r.OutputColorSpace);
    }
}
