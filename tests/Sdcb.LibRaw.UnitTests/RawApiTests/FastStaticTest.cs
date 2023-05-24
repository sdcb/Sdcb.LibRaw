using Sdcb.LibRaw.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void VersionTest()
        {
            string version = LibRawNative.GetVersion();
            _console.WriteLine(version);
        }
    }
}
