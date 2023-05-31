using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class StaticFunctionTest
{
    private readonly ITestOutputHelper _console;

    public StaticFunctionTest(ITestOutputHelper console)
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
    public void CapabilitiesTest()
    {
        RuntimeCapability caps = LibRawNative.GetCapabilities();
        _console.WriteLine(caps.ToString());
    }

    [Fact]
    public unsafe void CameraListTest()
    {
        int count = LibRawNative.GetCameraCount();
        IntPtr* list = (IntPtr*)LibRawNative.GetCameraList(); // char**
        List<string> cameras = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            cameras.Add(Marshal.PtrToStringAnsi(list[i])!);
        }
        Assert.Contains("Sony ILCE-7RM3 (A7R III)", cameras);
        Assert.NotNull(cameras[^1]);
    }
}