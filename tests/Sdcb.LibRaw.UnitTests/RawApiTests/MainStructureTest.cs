using Newtonsoft.Json.Schema;
using Sdcb.LibRaw.Natives;
using System.Reflection.Metadata;
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
            LibRawNative.Close(ptr);
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
            LibRawNative.Close(ptr);
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
            Assert.Equal(new double[] { 0.450000000000000, 4.5000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000 }, p.Gamma);
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
            LibRawNative.Close(ptr);
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
            LibRawNative.Close(ptr);
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
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void ColorTest()
    {
        IntPtr ptr = LibRawFromExampleBayer();
        try
        {
            V(LibRawNative.Unpack(ptr));
            V(LibRawNative.ProcessDcraw(ptr));

            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawColorData color = data.Color;
            Assert.Equal(Enumerable.Range(0, 0x10000).Select(x => (ushort)x), color.Curve);
            Assert.Equal(new uint[LibRawNative.CBlackSize], color.CBlack);
            Assert.Equal(0u, color.Black);
            Assert.Equal(255u, color.DataMaximum);
            Assert.Equal(65535u, color.Maximum);
            Assert.Equal(new int[] { 0, 0, 0, 0 }, color.LinearMax);
            Assert.Equal(0.0f, color.FMaximum);
            Assert.Equal(0.0f, color.FNorm);
            Assert.Equal(new ushort[64], color.White);
            Assert.Equal(new float[] { 0.0f, 0.0f, 0.0f, 0.0f }, color.CamMul);
            Assert.Equal(new float[] { 1.0f, 1.0f, 1.0f, 1.0f }, color.PreMul);
            Assert.Equal(new float[12], color.CMatrix);
            Assert.Equal(new float[12], color.Ccm);
            Assert.Equal(new float[12], color.RgbCam);
            Assert.Equal(new float[12], color.CamXyz);
            Assert.Equal(0, color.PhaseOneData.Format);
            Assert.Equal(0, color.PhaseOneData.KeyOff);
            Assert.Equal(0, color.PhaseOneData.Tag21a);
            Assert.Equal(0.0f, color.FlashUsed);
            Assert.Equal(0.0f, color.CanonEv);
            Assert.Equal("", color.Model2);
            Assert.Equal("", color.UniqueCameraModel);
            Assert.Equal("", color.LocalizedCameraModel);
            Assert.Equal("", color.ImageUniqueID);
            Assert.Equal("", color.RawDataUniqueID);
            Assert.Equal("", color.OriginalRawFileName);
            Assert.Equal(IntPtr.Zero, color.Profile);
            Assert.Equal(0u, color.ProfileLength);
            Assert.Equal(new uint[] { 0, 0, 0, 0, 0, 0, 0, 0 }, color.BlackStat);
            Assert.Equal(2, color.DngColor.Length);
            Assert.Equal(0u, color.DngLevels.ParsedFields);
            Assert.Equal(new int[256 * 4], color.WbCoeffs);
            Assert.Equal(new float[64 * 5], color.WbctCoeffs);
            Assert.Equal(0, color.AsShotWbApplied);
            Assert.Equal(2, color.P1Color.Length);
            Assert.Equal(0u, color.RawBps);
            Assert.Equal(0, color.ExifColorSpace);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void OtherParamsTest()
    {
        IntPtr ptr = LibRawFromExampleFile();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawImageOtherParams oparams = data.OtherParams;
            const float epsilon = 0.000001f;

            Assert.Equal(100.0, oparams.IsoSpeed, epsilon);
            Assert.Equal(100.0, oparams.IsoSpeed, epsilon);
            Assert.Equal(0.005000, oparams.Shutter, epsilon);
            Assert.Equal(1.200000, oparams.Aperture, epsilon);
            Assert.Equal(50.000000, oparams.FocalLength, epsilon);
            Assert.Equal(1674456985, oparams.Timestamp);
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
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void ThumbnailTest()
    {
        IntPtr ptr = LibRawFromExampleFile();
        try
        {
            V(LibRawNative.UnpackThumbnail(ptr));
            LibRawData data2 = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawThumbnail thumbnail2 = data2.Thumbnail;
            Assert.Equal(ThumbnailFormat.Jpeg, thumbnail2.Format);
            Assert.Equal(1616, thumbnail2.Width);
            Assert.Equal(1080, thumbnail2.Height);
            Assert.Equal(385072u, thumbnail2.Length);
            Assert.Equal(3, thumbnail2.Colors);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void ThumbnailListTest()
    {
        IntPtr ptr = LibRawFromExampleFile();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawThumbnailList list = data.ThumbnailList;

            var thumbnail1 = list.ThumbList[0];
            Assert.Equal(InternalThumbnailFormat.Jpeg, thumbnail1.Format);
            Assert.Equal(1616, thumbnail1.Width);
            Assert.Equal(1080, thumbnail1.Height);
            Assert.Equal(0, thumbnail1.Flip);
            Assert.Equal(385072u, thumbnail1.Length);
            Assert.Equal(104u, thumbnail1.Misc);
            Assert.Equal(135330, thumbnail1.Offset);

            var thumbnail2 = list.ThumbList[1];
            Assert.Equal(InternalThumbnailFormat.Jpeg, thumbnail2.Format);
            Assert.Equal(160, thumbnail2.Width);
            Assert.Equal(120, thumbnail2.Height);
            Assert.Equal(0, thumbnail2.Flip);
            Assert.Equal(7431u, thumbnail2.Length);
            Assert.Equal(104u, thumbnail2.Misc);
            Assert.Equal(43424, thumbnail2.Offset);

            Assert.Equal(2, list.ThumbCount);
            Assert.Equal(LibRawNative.ThumbnailMaxCount, list.ThumbList.Length);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void RawTest()
    {
        IntPtr ptr = LibRawFromExampleBayer();
        try
        {
            V(LibRawNative.Unpack(ptr));
            V(LibRawNative.ProcessDcraw(ptr));

            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            LibRawColorData color = data.RawData.Color;

            Assert.Equal(Enumerable.Range(0, 0x10000).Select(x => (ushort)x), color.Curve);
            Assert.Equal(new uint[LibRawNative.CBlackSize], color.CBlack);
            Assert.Equal(0u, color.Black);
            Assert.Equal(0u, color.DataMaximum);
            Assert.Equal(65535u, color.Maximum);
            Assert.Equal(new int[] { 0, 0, 0, 0 }, color.LinearMax);
            Assert.Equal(0.0f, color.FMaximum);
            Assert.Equal(0.0f, color.FNorm);
            Assert.Equal(new ushort[64], color.White);
            Assert.Equal(new float[] { 0.0f, 0.0f, 0.0f, 0.0f }, color.CamMul);
            Assert.Equal(new float[] { 1.0f, 1.0f, 1.0f, 1.0f }, color.PreMul);
            Assert.Equal(new float[12], color.CMatrix);
            Assert.Equal(new float[12], color.Ccm);
            Assert.Equal(new float[12], color.RgbCam);
            Assert.Equal(new float[12], color.CamXyz);
            Assert.Equal(0, color.PhaseOneData.Format);
            Assert.Equal(0, color.PhaseOneData.KeyOff);
            Assert.Equal(0, color.PhaseOneData.Tag21a);
            Assert.Equal(0.0f, color.FlashUsed);
            Assert.Equal(0.0f, color.CanonEv);
            Assert.Equal("", color.Model2);
            Assert.Equal("", color.UniqueCameraModel);
            Assert.Equal("", color.LocalizedCameraModel);
            Assert.Equal("", color.ImageUniqueID);
            Assert.Equal("", color.RawDataUniqueID);
            Assert.Equal("", color.OriginalRawFileName);
            Assert.Equal(IntPtr.Zero, color.Profile);
            Assert.Equal(0u, color.ProfileLength);
            Assert.Equal(new uint[] { 0, 0, 0, 0, 0, 0, 0, 0 }, color.BlackStat);
            Assert.Equal(2, color.DngColor.Length);
            Assert.Equal(0u, color.DngLevels.ParsedFields);
            Assert.Equal(new int[256 * 4], color.WbCoeffs);
            Assert.Equal(new float[64 * 5], color.WbctCoeffs);
            Assert.Equal(0, color.AsShotWbApplied);
            Assert.Equal(2, color.P1Color.Length);
            Assert.Equal(0u, color.RawBps);
            Assert.Equal(0, color.ExifColorSpace);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }

    [Fact]
    public void ParentClassTest()
    {
        IntPtr ptr = LibRawFromExampleBayer();
        try
        {
            LibRawData data = Marshal.PtrToStructure<LibRawData>(ptr);
            Assert.NotEqual(IntPtr.Zero, data.ParentClass);
        }
        finally
        {
            LibRawNative.Recycle(ptr);
            LibRawNative.Close(ptr);
        }
    }
}
