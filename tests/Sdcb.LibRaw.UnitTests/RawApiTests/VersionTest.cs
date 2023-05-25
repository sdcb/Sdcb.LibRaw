using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

internal class VersionTest
{
    private readonly ITestOutputHelper _console;

    public VersionTest()
    {
        _console = console;
    }
    
}

[Fact]
public void VersionTest()
{
    IntPtr handle = LibRawNative.GetVersion();
    Assert.True(handle != IntPtr.Zero);
    string? version = Marshal.PtrToStringAnsi(handle);
    Assert.NotNull(version);
    Assert.Equal("0.21.1-Release", version);
}
