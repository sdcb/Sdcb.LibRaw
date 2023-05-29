namespace Sdcb.LibRaw.UnitTests.RawApiTests
{
    record struct RGB24(byte B, byte G, byte R)
    {
        public override string ToString() => $"({R},{G},{B})";
    }
}
