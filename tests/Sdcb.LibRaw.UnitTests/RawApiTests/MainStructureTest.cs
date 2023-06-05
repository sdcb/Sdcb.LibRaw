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
            LibRawOutputParams p = data.OutputParams;

            Assert.Equal(new uint[] { 0, 0, 4294967295, 4294967295 }, p.Greybox);
            Assert.Equal(new uint[] { 0, 0, 4294967295, 4294967295 }, p.Cropbox);
            Assert.Equal(new double[] { 1.00000000000000, 1.000000000000000, 1.00000000000000, 1.00000000000000 }, p.Aber);
            Assert.Equal(new double[] { 0.450000000000000, 4.5000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000 }, p.Gamm);
            Assert.Equal(new float[] { 0.00000000f, 0.00000000f, 0.00000000f, 0.00000000f }, p.UserMul);
            Assert.Equal(1.0f, p.Bright);
            Assert.Equal(0.0f, p.Threshold);
            Assert.Equal(0, p.HalfSize);
            Assert.Equal(0, p.FourColorRgb);
            Assert.Equal(0, p.Highlight);
            Assert.Equal(0, p.UseAutoWb);
            Assert.Equal(0, p.UseCameraWb);
            Assert.Equal(1, p.UseCameraMatrix);
            Assert.Equal(1, p.OutputColor);
            Assert.Null(p.OutputProfile);
            Assert.Null(p.CameraProfile);
            Assert.Null(p.BadPixels);
            Assert.Null(p.DarkFrame);
            Assert.Equal(8, p.OutputBps);
            Assert.Equal(0, p.OutputTiff);
            Assert.Equal(0, p.OutputFlags);
            Assert.Equal(-1, p.UserFlip);
            Assert.Equal(-1, p.UserQual);
            Assert.Equal(-1, p.UserBlack);
            Assert.Equal(new int[] { -1000001, -1000001, -1000001, -1000001 }, p.UserCblack);
            Assert.Equal(-1, p.UserSat);
            Assert.Equal(0, p.MedPasses);
            Assert.Equal((float)0.00999999978, p.AutoBrightThr);
            Assert.Equal((float)0.750000000, p.AdjustMaximumThr);
            Assert.Equal(0, p.NoAutoBright);
            Assert.Equal(1, p.UseFujiRotate);
            Assert.Equal(0, p.GreenMatching);
            Assert.Equal(0, p.DcbIterations);
            Assert.Equal(0, p.DcbEnhanceFl);
            Assert.Equal(0, p.FbddNoiserd);
            Assert.Equal(0, p.ExpCorrec);
            Assert.Equal(1.0f, p.ExpShift);
            Assert.Equal(0.0f, p.ExpPreser);
            Assert.Equal(0, p.NoAutoScale);
            Assert.Equal(0, p.NoInterpolation);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }

    [Fact]
    public void RawUnpackParamsTest()
    {
        IntPtr ptr = LibRawNative.Initialize();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            RawUnpackParams p = data.RawUnpackParams;

            Assert.Equal(1, p.UseRawSpeed);
            Assert.Equal(39, p.UseDngSdk);
            Assert.Equal((uint)2, p.Options);
            Assert.Equal((uint)0, p.ShotSelect);
            Assert.Equal((uint)0, p.Specials);
            Assert.Equal((uint)2048, p.MaxRawMemoryMb);
            Assert.Equal(0, p.SonyArw2PosterizationThr);
            Assert.Equal((float)1.00000000, p.CoolscanNefGamma);
            Assert.Equal("\0\0\0\0\0", p.P4ShotOrder);
            Assert.Equal(IntPtr.Zero, p.CustomCameraStrings);

            Assert.Equal(LibRawProgress.Start, data.ProgressFlags);
            Assert.Equal(LibRawWarning.None, data.ProcessWarnings);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }

    [Fact]
    public void ProgressWarningsTest()
    {
        IntPtr ptr = LibRawNative.Initialize();
        try
        {
            {
                LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
                Assert.Equal(LibRawProgress.Start, data.ProgressFlags);
                Assert.Equal(LibRawWarning.None, data.ProcessWarnings);
            }
            {
                LibRawNative.OpenFile(ptr, ExampleFileName);
                LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
                Assert.Equal(LibRawProgress.Open | LibRawProgress.Identify | LibRawProgress.SizeAdjust, data.ProgressFlags);
                Assert.Equal(LibRawWarning.None, data.ProcessWarnings);
            }
            {
                LibRawNative.Unpack(ptr);
                LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
                Assert.Equal(LibRawProgress.Open | LibRawProgress.Identify | LibRawProgress.SizeAdjust | LibRawProgress.LoadRaw, data.ProgressFlags);
                Assert.Equal(LibRawWarning.None, data.ProcessWarnings);
            }
        }
        finally
        {
            LibRawNative.Recycle(ptr);
        }
    }
}
