﻿using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Sdcb.LibRaw.UnitTests.RawApiTests
{
    public class FastStaticTest
    {
        private readonly ITestOutputHelper _console;

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
                LibRawError error = LibRawNative.OpenFile(handle, @"C:\Users\ZhouJie\Pictures\a7r3\DJI_0030.DNG");
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
    }
}
