using Sdcb.LibRaw.UnitTests.RawApiTests;
using System.Text;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class ProcessTests
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
        Assert.Equal(4, ctx.Width);
        Assert.Equal(4, ctx.Height);

        ctx.Unpack();
        ctx.ProcessDcraw();
        using ProcessedImage image = ctx.MakeDcrawMemoryImage();
        Span<RGB24> d = image.GetData<RGB24>();
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < ctx.Height; ++y)
        {
            for (int x = 0; x < ctx.Width; ++x)
            {
                RGB24 rgb = d[y * ctx.Width + x];
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

    [Fact]
    public void OpenFileTest()
    {
        using RawContext ctx = ExampleFile();
        Assert.Equal(8000, ctx.Width);
        Assert.Equal(5320, ctx.Height);
    }

    [Fact]
    public void OpenBufferTest()
    {
        using RawContext ctx = ExampleFileBuffer();
        Assert.Equal(8000, ctx.Width);
        Assert.Equal(5320, ctx.Height);
    }

    [Fact]
    public void FileThumbnailTest()
    {
        using RawContext ctx = ExampleFile();
        {
            ctx.UnpackThunbnail(0);
            using ProcessedImage image0 = ctx.MakeDcrawMemoryThumbnail();
            Assert.Equal(ProcessedImageType.Jpeg, image0.ImageType);
            Assert.Equal(386458, image0.GetData<byte>().Length);
        }
        {
            ctx.UnpackThunbnail(1);
            using ProcessedImage image1 = ctx.MakeDcrawMemoryThumbnail();
            Assert.Equal(ProcessedImageType.Jpeg, image1.ImageType);
            Assert.Equal(8817, image1.GetData<byte>().Length);
        }
    }

    private RawContext ExampleFile() => RawContext.OpenFile("./examples/DSC02412.ARW");
    private RawContext ExampleFileBuffer() => RawContext.FromBuffer(File.ReadAllBytes("./examples/DSC02412.ARW"));

    private RawContext ExampleBayer()
    {
        const int width = 4;
        const int height = 4;
        ushort[] bayerData = new ushort[width * height]
        {
            127, 0, 0, 127,
            0, 0, 0, 0,
            0, 0, 0, 0,
            255, 0, 0, 255,
        };
        return RawContext.OpenBayerData<ushort>(bayerData, width, height);
    }
}
