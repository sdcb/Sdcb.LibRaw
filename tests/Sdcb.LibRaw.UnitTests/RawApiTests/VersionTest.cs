using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class VersionTest
{
    private readonly ITestOutputHelper _console;

    public VersionTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void GetVersionTest()
    {
        IntPtr handle = LibRawNative.GetVersion();
        Assert.True(handle != IntPtr.Zero);
        string? version = Marshal.PtrToStringAnsi(handle);
        Assert.NotNull(version);
        Assert.True(string.Compare(version, "0.21.1-Release") >= 0);
    }

    [Fact]
    public void GetVersionNumberTest()
    {
        int num = LibRawNative.GetVersionNumber();
        int major = num >> 16;
        int minor = num >> 8;
        int patch = num & 0xFF;
        Assert.True(major >= 0);
        Assert.True(minor >= 21);
        Assert.True(patch >= 1);
    }
}