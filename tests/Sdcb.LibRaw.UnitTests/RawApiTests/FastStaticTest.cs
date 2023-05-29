using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests
{
    public class FastStaticTest
    {
        private readonly ITestOutputHelper _console;
        private const string ExampleFileName = @"./examples/DSC02412.ARW";

        public FastStaticTest(ITestOutputHelper console)
        {
            _console = console;
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
        public void InitRecycleTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            Assert.NotEqual(IntPtr.Zero, handle);
            LibRawNative.Recycle(handle);
        }

        [Fact]
        public void OpenFileTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                Assert.NotEqual(IntPtr.Zero, handle);
                LibRawError error = LibRawNative.OpenFile(handle, @"./examples/DSC02412.ARW");
                if (error != LibRawError.Success)
                {
                    _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
                }
                Assert.Equal(LibRawError.Success, error);
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void OpenFileWTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                Assert.NotEqual(IntPtr.Zero, handle);
                LibRawError error = LibRawNative.OpenFileW(handle, ExampleFileName);
                if (error != LibRawError.Success)
                {
                    _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
                }
                Assert.Equal(LibRawError.Success, error);
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        [Fact]
        public unsafe void OpenBufferTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                Assert.NotEqual(IntPtr.Zero, handle);
                byte[] buffer = File.ReadAllBytes(ExampleFileName);
                fixed (byte* pbuffer = &buffer[0])
                {
                    EnsureSuccess(LibRawNative.OpenBuffer(handle, (IntPtr)pbuffer, buffer.Length));
                }
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        [Fact]
        public unsafe void OpenBayerDataTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                Assert.NotEqual(IntPtr.Zero, handle);
                ushort bayerWidth = 4, bayerHeight = 4;
                ushort[] bayerData = Enumerable
                    .Range(0, bayerHeight)
                    .SelectMany(x => Enumerable.Range(0, bayerWidth).Select((x, i) => (ushort)(x * bayerWidth + i)))
                    .ToArray();
                fixed (void* dataPtr = &bayerData[0])
                {
                    EnsureSuccess(LibRawNative.OpenBayerData(handle, (IntPtr)dataPtr, (uint)bayerData.Length * sizeof(ushort), 
                        bayerWidth, bayerHeight,
                        0, 0, 0, 0, 0, OpenBayerPattern.Bggr, 0, 0, 0));
                    EnsureSuccess(LibRawNative.Unpack(handle));
                    EnsureSuccess(LibRawNative.ProcessDcraw(handle));
                    IntPtr image = LibRawNative.MakeDcrawMemoryImage(handle, out LibRawError errorCode);
                    EnsureSuccess(errorCode);
                    LibRawNative.ClearDcrawMemory(image);
                }
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        private unsafe void EnsureSuccess(LibRawError error)
        {
            if (error != LibRawError.Success)
            {
                _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
            }
            Assert.Equal(LibRawError.Success, error);
        }
    }
}
