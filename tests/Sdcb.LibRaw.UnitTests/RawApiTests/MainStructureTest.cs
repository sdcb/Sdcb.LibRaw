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
            Assert.Equal("", info.BodySerial.TakeWhile(c => c != '\0').ToArray());
            Assert.Equal("4ff0000fc08", info.InternalBodySerial.TakeWhile(c => c != '\0').ToArray());
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }

    [Fact]
    public void LibRawOutputParamsTest()
    {
        IntPtr ptr = LibRawFromExampleFile();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawOutputParams @params = data.OutputParams;

            Assert.Equal(new uint[] { 0, 0, 4294967295, 4294967295 }, @params.Greybox);
            Assert.Equal(new uint[] { 0, 0, 4294967295, 4294967295 }, @params.Cropbox);
            Assert.Equal(new double[] { 1.00000000000000, 1.000000000000000, 1.00000000000000, 1.00000000000000 }, @params.Aber);
            Assert.Equal(new double[] { 0.450000000000000, 4.5000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000 }, @params.Gamm);
            Assert.Equal(new float[] { 0.00000000f, 0.00000000f, 0.00000000f, 0.00000000f }, @params.UserMul);
            Assert.Equal(1.0f, @params.Bright);
            Assert.Equal(0.0f, @params.Threshold);
            Assert.Equal(0, @params.HalfSize);
            Assert.Equal(0, @params.FourColorRgb);
            Assert.Equal(0, @params.Highlight);
            Assert.Equal(0, @params.UseAutoWb);
            Assert.Equal(0, @params.UseCameraWb);
            Assert.Equal(1, @params.UseCameraMatrix);
            Assert.Equal(1, @params.OutputColor);
            Assert.Null(@params.OutputProfile);
            Assert.Null(@params.CameraProfile);
            Assert.Null(@params.BadPixels);
            Assert.Null(@params.DarkFrame);
            Assert.Equal(8, @params.OutputBps);
            Assert.Equal(0, @params.OutputTiff);
            Assert.Equal(0, @params.OutputFlags);
            Assert.Equal(-1, @params.UserFlip);
            Assert.Equal(-1, @params.UserQual);
            Assert.Equal(-1, @params.UserBlack);
            Assert.Equal(new int[] { -1000001, -1000001, -1000001, -1000001 }, @params.UserCblack);
            Assert.Equal(-1, @params.UserSat);
            Assert.Equal(0, @params.MedPasses);
            Assert.Equal((float)0.00999999978, @params.AutoBrightThr);
            Assert.Equal((float)0.750000000, @params.AdjustMaximumThr);
            Assert.Equal(0, @params.NoAutoBright);
            Assert.Equal(1, @params.UseFujiRotate);
            Assert.Equal(0, @params.GreenMatching);
            Assert.Equal(0, @params.DcbIterations);
            Assert.Equal(0, @params.DcbEnhanceFl);
            Assert.Equal(0, @params.FbddNoiserd);
            Assert.Equal(0, @params.ExpCorrec);
            Assert.Equal(1.0f, @params.ExpShift);
            Assert.Equal(0.0f, @params.ExpPreser);
            Assert.Equal(0, @params.NoAutoScale);
            Assert.Equal(0, @params.NoInterpolation);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }
}
