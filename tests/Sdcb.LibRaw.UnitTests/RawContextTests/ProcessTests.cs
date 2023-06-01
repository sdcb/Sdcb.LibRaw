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
        return RawContext.OpenBayerData(bayerData.AsSpan(), width, height);
    }
}
