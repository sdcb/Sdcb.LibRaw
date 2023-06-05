using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class MainStructureTest : BaseCApiTest
{
    public MainStructureTest(ITestOutputHelper console) : base(console)
    {
    }

    [Fact]
    public void ImagesTest()
    {
        IntPtr ptr = LibRawFromExampleBayer();
        LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
        Assert.All(data.Image, v => Assert.Equal(0, v));
    }

    [Fact]
    public void LibRawImageSizesTest()
    {
        IntPtr ptr = LibRawFromExampleBayer();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawImageSizes sizes = data.ImageSizes;

            Assert.Equal((ushort)4, sizes.RawHeight);
            Assert.Equal((ushort)4, sizes.RawWidth);
            Assert.Equal((ushort)4, sizes.Height);
            Assert.Equal((ushort)4, sizes.Width);
            Assert.Equal((ushort)0, sizes.TopMargin);
            Assert.Equal((ushort)0, sizes.LeftMargin);
            Assert.Equal((ushort)4, sizes.IHeight);
            Assert.Equal((ushort)4, sizes.IWidth);
            Assert.Equal((uint)0, sizes.RawPitch);
            Assert.Equal(1.0, sizes.PixelAspect);
            Assert.Equal(0, sizes.Flip);
            Assert.Equal((ushort)0, sizes.RawAspect);
            Assert.All(sizes.Mask, mask => Assert.Equal(0, mask));
            Assert.All(sizes.RawInsetCrops, crop =>
            {
                Assert.Equal((ushort)0, crop.CLeft);
                Assert.Equal((ushort)0, crop.CTop);
                Assert.Equal((ushort)0, crop.CWidth);
                Assert.Equal((ushort)0, crop.CHeight);
            });
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }

    [Fact]
    public void LibRawShootingInfoTest()
    {
        IntPtr ptr = LibRawFromExampleFile();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawShootingInfo info = data.ShootingInfo;

            Assert.Equal(0, info.DriveMode);
            Assert.Equal(3, info.FocusMode);
            Assert.Equal(0, info.MeteringMode);
            Assert.Equal(-1, info.AFPoint);
            Assert.Equal(7, info.ExposureMode);
            Assert.Equal(1, info.ExposureProgram);
            Assert.Equal(1, info.ImageStabilization);
            Assert.Equal("",info.BodySerial.TakeWhile(c => c != '\0').ToArray());
            Assert.Equal("4ff0000fc08",info.InternalBodySerial.TakeWhile(c => c != '\0').ToArray());
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }
}
