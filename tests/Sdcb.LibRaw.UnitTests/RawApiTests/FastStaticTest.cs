using Sdcb.LibRaw.Natives;
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

        
    }
}
