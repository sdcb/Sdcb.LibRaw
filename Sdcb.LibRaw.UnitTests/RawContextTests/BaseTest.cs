namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class BaseTest
{
    protected static RawContext ExampleFile() => RawContext.OpenFile("./examples/DSC02412.ARW");
    protected static RawContext ExampleFileBuffer() => RawContext.FromBuffer(File.ReadAllBytes("./examples/DSC02412.ARW"));

    protected static RawContext ExampleBayer()
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
