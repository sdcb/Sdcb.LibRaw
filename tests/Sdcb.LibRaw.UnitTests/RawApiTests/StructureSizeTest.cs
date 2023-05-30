using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class StructureSizeTest
{
    [Fact]
    public void LibRawMakerNotesLensSizeTest()
    {
        Assert.Equal(736, Marshal.SizeOf<LibRawMakerNotesLens>());
    }

    [Fact]
    public void LibRawNikonLensSizeTest()
    {
        Assert.Equal(8, Marshal.SizeOf<LibRawNikonLens>());
    }

    [Fact]
    public void LibRawDngLensSizeTest()
    {
        Assert.Equal(16, Marshal.SizeOf<LibRawDngLens>());
    }

    [Fact]
    public void LibRawLensInfoSizeTest()
    {
        Assert.Equal(1296, Marshal.SizeOf<LibRawLensInfo>());
    }
}
