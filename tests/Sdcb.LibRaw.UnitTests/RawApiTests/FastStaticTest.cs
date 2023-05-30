using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class FastStaticTest
{
    private readonly ITestOutputHelper _console;
    private const string ExampleFileName = @"./examples/DSC02412.ARW";

    public FastStaticTest(ITestOutputHelper console)
    {
        _console = console;
    }

    private unsafe void V(LibRawError error)
    {
        if (error != LibRawError.Success)
        {
            _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
        }
        Assert.Equal(LibRawError.Success, error);
    }

    private IntPtr LibRawFromExampleFile()
    {
        IntPtr handle = LibRawNative.Initialize();
        Assert.NotEqual(IntPtr.Zero, handle);
        V(LibRawNative.OpenFile(handle, ExampleFileName));
        return handle;
    }

    private unsafe IntPtr LibRawFromExampleBayer()
    {
        IntPtr handle = LibRawNative.Initialize();
        Assert.NotEqual(IntPtr.Zero, handle);
        const ushort bayerWidth = 4, bayerHeight = 4;
        ushort[] bayerData = new ushort[bayerWidth * bayerHeight]
        {
                127, 0, 0, 127,
                0, 0, 0, 0,
                0, 0, 0, 0,
                255, 0, 0, 255,
        };
        fixed (void* dataPtr = &bayerData[0])
        {
            V(LibRawNative.OpenBayerData(handle, (IntPtr)dataPtr, (uint)bayerData.Length * sizeof(ushort),
                bayerWidth, bayerHeight,
                0, 0, 0, 0, 0, OpenBayerPattern.Bggr, 0, 0, 0));
        }
        return handle;
    }

    [Fact]
    public void GetErrorMessageTest()
    {
        IntPtr handle = LibRawNative.GetErrorMessage(LibRawError.IOError);
        Assert.True(handle != IntPtr.Zero);
        string? msg = Marshal.PtrToStringAnsi(handle);
        Assert.NotNull(msg);
        Assert.Equal("Input/output error", msg);
    }

    [Fact]
    public void GetProgressMessageTest()
    {
        IntPtr handle = LibRawNative.GetProgressMessage(LibRawProgress.Highlights);
        Assert.True(handle != IntPtr.Zero);
        string? msg = Marshal.PtrToStringAnsi(handle);
        Assert.NotNull(msg);
        Assert.Equal("Highlight recovery", msg);
    }

    [Fact]
    public void InitRecycleTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        Assert.NotEqual(IntPtr.Zero, handle);
        LibRawNative.Recycle(handle);
    }

    [Fact]
    public void OpenFileTest()
    {
        IntPtr handle = LibRawFromExampleFile();
        LibRawNative.Recycle(handle);
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public void OpenFileWTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            Assert.NotEqual(IntPtr.Zero, handle);
            LibRawError error = LibRawNative.OpenFileW(handle, ExampleFileName);
            if (error != LibRawError.Success)
            {
                _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
            }
            Assert.Equal(LibRawError.Success, error);
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void OpenBufferTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            Assert.NotEqual(IntPtr.Zero, handle);
            byte[] buffer = File.ReadAllBytes(ExampleFileName);
            fixed (byte* pbuffer = &buffer[0])
            {
                V(LibRawNative.OpenBuffer(handle, (IntPtr)pbuffer, buffer.Length));
            }
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void OpenBayerDataTest()
    {
        IntPtr handle = LibRawFromExampleBayer();
        try
        {
            const int bayerWidth = 4, bayerHeight = 4;
            V(LibRawNative.Unpack(handle));
            V(LibRawNative.ProcessDcraw(handle));
            LibRawProcessedImage* image = (LibRawProcessedImage*)LibRawNative.MakeDcrawMemoryImage(handle, out LibRawError errorCode);
            V(errorCode);
            Assert.NotEqual(IntPtr.Zero, (IntPtr)image);

            try
            {
                StringBuilder sb = new StringBuilder();
                Span<RGB24> d = image->GetData<RGB24>();
                Assert.Equal(bayerWidth * bayerHeight, d.Length);

                for (int y = 0; y < bayerHeight; ++y)
                {
                    for (int x = 0; x < bayerWidth; ++x)
                    {
                        RGB24 rgb = d[y * bayerWidth + x];
                        sb.Append($"{rgb} ");
                    }
                    sb.AppendLine();
                }
                _console.WriteLine(sb.ToString());
                Assert.Equal(179, d[0].R);
                Assert.Equal(83, d[5].R);
                Assert.Equal(255, d[15].B);
                Assert.Equal(255, d[12].G);
            }
            finally
            {
                LibRawNative.ClearDcrawMemory((IntPtr)image);
            }

        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void ThumbnailTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            Assert.NotEqual(IntPtr.Zero, handle);
            V(LibRawNative.OpenFile(handle, ExampleFileName));
            V(LibRawNative.UnpackThumbnail(handle));
            LibRawProcessedImage* image = (LibRawProcessedImage*)LibRawNative.MakeDcrawMemoryThumbnail(handle, out LibRawError errorCode);
            V(errorCode);
            Assert.NotEqual(IntPtr.Zero, (IntPtr)image);
            try
            {
                Assert.Equal(image->DataSize, image->GetData<byte>().ToArray().Length);
                Assert.Equal(ImageFormat.Jpeg, image->Type);
                Assert.Equal(386458, image->DataSize);
            }
            finally
            {
                LibRawNative.ClearDcrawMemory((IntPtr)image);
            }
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void GetIparamsTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            V(LibRawNative.OpenFile(handle, ExampleFileName));
            IntPtr ptr = LibRawNative.GetImageParameters(handle);
            LibRawImageParams iparams = Marshal.PtrToStructure<LibRawImageParams>(ptr);
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
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void GetLensInfoTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            V(LibRawNative.OpenFile(handle, ExampleFileName));
            IntPtr ptr = LibRawNative.GetLensInformation(handle);
            const float epsilon = 0.000001f;

            LibRawLensInfo iparams = Marshal.PtrToStructure<LibRawLensInfo>(ptr);
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
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void GetImageDataTest()
    {
        IntPtr handle = LibRawNative.Initialize();
        try
        {
            V(LibRawNative.OpenFile(handle, ExampleFileName));
            IntPtr ptr = LibRawNative.GetImageOtherParameters(handle);
            LibRawImageOtherParams oparams = Marshal.PtrToStructure<LibRawImageOtherParams>(ptr);
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
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void OutputBps16Test()
    {
        IntPtr handle = LibRawFromExampleBayer();
        try
        {
            const int bayerWidth = 4, bayerHeight = 4;
            V(LibRawNative.Unpack(handle));
            LibRawNative.SetOutputBitsPerSample(handle, 16);
            V(LibRawNative.ProcessDcraw(handle));
            LibRawProcessedImage* image = (LibRawProcessedImage*)LibRawNative.MakeDcrawMemoryImage(handle, out LibRawError errorCode);
            V(errorCode);
            Assert.NotEqual(IntPtr.Zero, (IntPtr)image);

            try
            {
                StringBuilder sb = new StringBuilder();
                Span<RGB48> d = image->GetData<RGB48>();
                Assert.Equal(bayerWidth * bayerHeight, d.Length);

                for (int y = 0; y < bayerHeight; ++y)
                {
                    for (int x = 0; x < bayerWidth; ++x)
                    {
                        RGB48 rgb = d[y * bayerWidth + x];
                        sb.Append($"{rgb} ");
                    }
                    sb.AppendLine();
                }
                _console.WriteLine(sb.ToString());
                Assert.Equal(46045, d[0].R);
                Assert.Equal(21353, d[5].R);
                Assert.Equal(65409, d[15].B);
                Assert.Equal(65409, d[12].G);
            }
            finally
            {
                LibRawNative.ClearDcrawMemory((IntPtr)image);
            }
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void SetOutputTiffTest()
    {
        IntPtr handle = LibRawFromExampleBayer();
        try
        {
            LibRawNative.SetOutputTiff(handle, 1);
            V(LibRawNative.Unpack(handle));
            V(LibRawNative.ProcessDcraw(handle));
            V(LibRawNative.WriteDcrawPpmTiff(handle, "test.tif"));
            Assert.True(File.Exists("test.tif"));
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }

    [Fact]
    public unsafe void SonyArwSetOutputTiffTest()
    {
        OpenMpLib.omp_set_num_threads(OpenMpLib.omp_get_max_threads());
        IntPtr handle = LibRawFromExampleFile();
        try
        {
            LibRawNative.SetOutputTiff(handle, 1);
            LibRawNative.SetGamma(handle, 0, 0.55f);
            V(LibRawNative.Unpack(handle));
            V(LibRawNative.ProcessDcraw(handle));
            V(LibRawNative.WriteDcrawPpmTiff(handle, "test.tif"));
            Assert.True(File.Exists("test.tif"));
        }
        finally
        {
            LibRawNative.Recycle(handle);
        }
    }
}
