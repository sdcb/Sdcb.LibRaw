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

        private unsafe void V(LibRawError error)
        {
            if (error != LibRawError.Success)
            {
                _console.WriteLine(Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error)));
            }
            Assert.Equal(LibRawError.Success, error);
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
                LibRawError error = LibRawNative.OpenFile(handle, ExampleFileName);
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
                    V(LibRawNative.OpenBuffer(handle, (IntPtr)pbuffer, buffer.Length));
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
                        0, 0, 0, 0, 0, OpenBayerPattern.Bggr, 0, 0, 0));
                }

                V(LibRawNative.Unpack(handle));
                V(LibRawNative.ProcessDcraw(handle));
                LibRawProcessedImage* image = (LibRawProcessedImage*)LibRawNative.MakeDcrawMemoryImage(handle, out LibRawError errorCode);
                V(errorCode);
                Assert.NotEqual(IntPtr.Zero, (IntPtr)image);

                try
                {
                    StringBuilder sb = new StringBuilder();
                    Span<RGB24> d = image->GetData<RGB24>();
                    Assert.Equal(bayerWidth * bayerHeight, d.Length);

                    for (int y = 0; y < bayerHeight; ++y)
                    {
                        for (int x = 0; x < bayerWidth; ++x)
                        {
                            RGB24 rgb = d[y * bayerWidth + x];
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
                finally
                {
                    LibRawNative.ClearDcrawMemory((IntPtr)image);
                }

            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        [Fact]
        public unsafe void ThumbnailTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                Assert.NotEqual(IntPtr.Zero, handle);
                V(LibRawNative.OpenFile(handle, ExampleFileName));
                V(LibRawNative.UnpackThumbnail(handle));
                LibRawProcessedImage* image = (LibRawProcessedImage*)LibRawNative.MakeDcrawMemoryThumbnail(handle, out LibRawError errorCode);
                V(errorCode);
                Assert.NotEqual(IntPtr.Zero, (IntPtr)image);
                try
                {
                    Assert.Equal(image->DataSize, image->GetData<byte>().ToArray().Length);
                    Assert.Equal(ImageFormat.Jpeg, image->Type);
                    Assert.Equal(386458, image->DataSize);
                }
                finally
                {
                    LibRawNative.ClearDcrawMemory((IntPtr)image);
                }
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }

        [Fact]
        public unsafe void GetIparamsTest()
        {
            IntPtr handle = LibRawNative.Initialize();
            try
            {
                LibRawNative.GetImageParameters(handle);
            }
            finally
            {
                LibRawNative.Recycle(handle);
            }
        }
    }
}
