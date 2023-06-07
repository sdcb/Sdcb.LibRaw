﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.LibRaw.UnitTests.RawContextTests;

public class UnexpectedProcessTest : BaseTest
{
    [Fact]
    public void GetThumbInNonThumbImage_Should_Error()
    {
        using RawContext r = ExampleBayer();
        LibRawException ex = Assert.Throws<LibRawException>(() => r.UnpackThunbnail());
        Assert.Equal(LibRawError.RequestForNonexistentThumbnail, ex.ErrorCode);
    }

    [Fact]
    public void OpenUnexpectedFile_Should_Error()
    {
        LibRawException ex = Assert.Throws<LibRawException>(() => RawContext.OpenFile("a-file-that-not-exists.cr2"));
        Assert.Equal(LibRawError.IOError, ex.ErrorCode);
    }

    [Fact]
    public void DcprocessWithoutUnpack_Should_Error()
    {
        using RawContext r = ExampleBayer();
        LibRawException ex = Assert.Throws<LibRawException>(() => r.ProcessDcraw());
        Assert.Equal(LibRawError.OutOfOrderCall, ex.ErrorCode);
    }

    [Fact]
    public void ExportTifWithoutDcraw_Should_Error()
    {
        using RawContext r = ExampleBayer();
        LibRawException ex = Assert.Throws<LibRawException>(() =>
        {
            r.Unpack();
            r.WriteDcrawPpmTiff("test.tif");
        });
        Assert.Equal(LibRawError.OutOfOrderCall, ex.ErrorCode);
    }

    [Fact]
    public void AccessDisposedObject_Should_Error()
    {
        using RawContext r = ExampleBayer();
        r.Dispose();

        Assert.Throws<ObjectDisposedException>(() =>
        {
            int width = r.Width;
        });
    }
}