﻿namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class StaticMethodTests
{
    [Fact]
    public void SupportedCamerasTest()
    {
        string[] cameras = RawContext.SupportedCameras;
        Assert.NotEmpty(cameras);
        Assert.Contains("Canon EOS 5D Mark IV", cameras);
    }

    [Fact]
    public void VersionTest()
    {
        Assert.NotNull(RawContext.Version);
        Assert.True(string.Compare(RawContext.Version, "0.21.1-Release") >= 0);
    }

    [Fact]
    public void VersionNumberTest()
    {
        Assert.NotNull(RawContext.VersionNumber);
        Assert.True(RawContext.VersionNumber >= new Version("0.21.1"));
    }
}
