using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class BaseCApiTest
{
    protected const string ExampleFileName = @"./examples/DSC02412.ARW";
    protected readonly ITestOutputHelper _console;

    public BaseCApiTest(ITestOutputHelper console)
    {
        _console = console;
    }

    protected unsafe void V(LibRawError error)
    {
        if (error != LibRawError.Success)
        {
            _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
        }
        Assert.Equal(LibRawError.Success, error);
    }

    protected IntPtr LibRawFromExampleFile()
    {
        IntPtr handle = LibRawNative.Initialize();
        Assert.NotEqual(IntPtr.Zero, handle);
        V(LibRawNative.OpenFile(handle, ExampleFileName));
        return handle;
    }

    protected unsafe IntPtr LibRawFromExampleBayer()
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
                0, 0, 0, 0, 0, OpenBayerPattern.BGGR, 0, 0, 0));
        }
        return handle;
    }
}
