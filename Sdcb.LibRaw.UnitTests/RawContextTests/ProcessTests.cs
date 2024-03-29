﻿using Sdcb.LibRaw.UnitTests.RawApiTests;
using System.Drawing;
using System.Text;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class ProcessTests : BaseTest
{
    private readonly ITestOutputHelper _console;

    public ProcessTests(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void OpenBayerAndProcessImage()
    {
        using RawContext ctx = ExampleBayer();
        Assert.Equal(4, ctx.RawWidth);
        Assert.Equal(4, ctx.RawHeight);
        Assert.Equal(4, ctx.Width);
        Assert.Equal(4, ctx.Height);
        Assert.Equal(DecoderFlag.FlatData, ctx.DecoderInfo.DecoderFlags);
        Assert.Equal("unpacked_load_raw()", ctx.DecoderInfo.DecoderName);

        ctx.Unpack();
        ctx.DcrawProcess();
        using ProcessedImage image = ctx.MakeDcrawMemoryImage();
        Span<RGB24> d = image.AsSpan<RGB24>();
        StringBuilder sb = new();
        for (int y = 0; y < ctx.RawHeight; ++y)
        {
            for (int x = 0; x < ctx.RawWidth; ++x)
            {
                RGB24 rgb = d[y * ctx.RawWidth + x];
                sb.Append($"{rgb} ");
            }
            sb.AppendLine();
        }
        _console.WriteLine(sb.ToString());
        Assert.Equal(179, d[0].R);
        Assert.Equal(83, d[5].R);
        Assert.Equal(255, d[15].B);
        Assert.Equal(255, d[12].G);

        ctx.OutputTiff = true;
        ctx.DcrawProcess();
        ctx.WriteDcrawPpmTiff("test2.tif");
        Assert.True(File.Exists("test2.tif"));
        File.Delete("test2.tif");
    }

    [Fact]
    public void OpenFileTest()
    {
        using RawContext ctx = ExampleFile();
        Assert.Equal(8000, ctx.RawWidth);
        Assert.Equal(5320, ctx.RawHeight);
        Assert.Equal(7968, ctx.Width);
        Assert.Equal(5320, ctx.Height);
        Assert.Equal(DecoderFlag.HasCurve | DecoderFlag.SonyArw2 | DecoderFlag.TryRawSpeed | DecoderFlag.TryRawSpeed3, ctx.DecoderInfo.DecoderFlags);
        Assert.Equal("sony_arw2_load_raw()", ctx.DecoderInfo.DecoderName);

        ctx.Unpack();
    }

    [Fact]
    public void OpenBufferTest()
    {
        using RawContext ctx = ExampleFileBuffer();
        Assert.Equal(8000, ctx.RawWidth);
        Assert.Equal(5320, ctx.RawHeight);
        Assert.Equal(7968, ctx.Width);
        Assert.Equal(5320, ctx.Height);
        Assert.Equal(DecoderFlag.HasCurve | DecoderFlag.SonyArw2 | DecoderFlag.TryRawSpeed | DecoderFlag.TryRawSpeed3, ctx.DecoderInfo.DecoderFlags);
        Assert.Equal("sony_arw2_load_raw()", ctx.DecoderInfo.DecoderName);
    }

    [Fact]
    public void FileThumbnailTest()
    {
        using RawContext ctx = ExampleFile();
        {
            ctx.UnpackThumbnail(0);
            using ProcessedImage image0 = ctx.MakeDcrawMemoryThumbnail();
            Assert.Equal(ProcessedImageType.Jpeg, image0.ImageType);
            Assert.Equal(386458, image0.AsSpan<byte>().Length);
            ctx.WriteDcrawThumbnail("test.jpg");
            Assert.True(File.Exists("test.jpg"));
            File.Delete("test.jpg");
        }
        {
            ctx.UnpackThumbnail(1);
            using ProcessedImage image1 = ctx.MakeDcrawMemoryThumbnail();
            Assert.Equal(ProcessedImageType.Jpeg, image1.ImageType);
            Assert.Equal(8817, image1.AsSpan<byte>().Length);
            ctx.WriteDcrawThumbnail("test.jpg");
            Assert.True(File.Exists("test.jpg"));
            File.Delete("test.jpg");
        }
    }

    [Fact]
    public void GammaTest()
    {
        using RawContext ctx = ExampleBayer();

        ctx.Unpack();
        ctx.Gamma[0] = 1;
        ctx.DcrawProcess();
        using ProcessedImage image = ctx.MakeDcrawMemoryImage();
        Span<RGB24> d = image.AsSpan<RGB24>();
        StringBuilder sb = new();
        for (int y = 0; y < ctx.RawHeight; ++y)
        {
            for (int x = 0; x < ctx.RawWidth; ++x)
            {
                RGB24 rgb = d[y * ctx.RawWidth + x];
                sb.Append($"{rgb} ");
            }
            sb.AppendLine();
        }
        _console.WriteLine(sb.ToString());
        Assert.Equal(127, d[0].R);
        Assert.Equal(63, d[1].R);
        Assert.Equal(31, d[5].R);
        Assert.Equal(127, d[14].B);
        Assert.Equal(255, d[12].G);
    }

    [Fact]
    public void NoInterpolationTest()
    {
        using RawContext ctx = ExampleBayer();

        ctx.Unpack();
        ctx.DcrawProcess(c =>
        {
            c.Interpolation = false;
            c.Gamma[0] = 1;
        });
        using ProcessedImage image = ctx.MakeDcrawMemoryImage();
        Span<RGB24> d = image.AsSpan<RGB24>();
        StringBuilder sb = new();
        for (int y = 0; y < ctx.RawHeight; ++y)
        {
            for (int x = 0; x < ctx.RawWidth; ++x)
            {
                RGB24 rgb = d[y * ctx.RawWidth + x];
                sb.Append($"{rgb} ");
            }
            sb.AppendLine();
        }
        _console.WriteLine(sb.ToString());
        Assert.Equal(127, d[0].R);
        Assert.Equal(0, d[1].R);
        Assert.Equal(0, d[5].R);
        Assert.Equal(0, d[14].B);
        Assert.Equal(255, d[12].G);
    }

    [Fact]
    public void ThumbnailShouldHaveWidth()
    {
        using RawContext ctx = ExampleFile();
        ctx.UnpackThumbnail(1);
        using ProcessedImage image0 = ctx.MakeDcrawMemoryThumbnail();
        Assert.Equal(ProcessedImageType.Jpeg, image0.ImageType);
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Assert.Equal(160, image0.Width);
            Assert.Equal(120, image0.Height);
        }
        else
        {
            _console.WriteLine("SKIP, linux don't support this.");
        }
    }

    [Fact]
    public void CropTest()
    {
        using RawContext ctx = ExampleBayer();
        ctx.Unpack();
        ctx.DcrawProcess(c => c.Cropbox = Rectangle.FromLTRB(1, 1, 2, 3));
        using ProcessedImage image = ctx.MakeDcrawMemoryImage();
        Assert.Equal(1, image.Width);
        Assert.Equal(2, image.Height);
    }
}
