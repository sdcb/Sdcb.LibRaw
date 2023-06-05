﻿using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.UnitTests.RawApiTests;

public class StructureSizeTest
{
    [Fact]
    public void LibRawMakerNotesLensSizeTest()
    {
        Assert.Equal(736, Marshal.SizeOf<LibRawLensMakerNotes>());
    }

    [Fact]
    public void LibRawNikonLensSizeTest()
    {
        Assert.Equal(8, Marshal.SizeOf<LibRawNikonLens>());
    }

    [Fact]
    public void LibRawDngLensSizeTest()
    {
        Assert.Equal(16, Marshal.SizeOf<LibRawDngLens>());
    }

    [Fact]
    public void LibRawLensInfoSizeTest()
    {
        Assert.Equal(1296, Marshal.SizeOf<LibRawLensInfo>());
    }

    [Fact]
    public void LibRawGpsInfoSizeTest()
    {
        Assert.Equal(48, Marshal.SizeOf<LibRawGPS>());
    }

    [Fact]
    public void LibRawImageInfoSizeTest()
    {
        Assert.Equal(800, Marshal.SizeOf<LibRawImageOtherParams>());
    }

    [Fact]
    public void LibRawInsetCropSizeTest()
    {
        Assert.Equal(8, Marshal.SizeOf<LibRawInsetCrop>());
    }

    [Fact]
    public void LibRawImageSizesSizeTest()
    {
        Assert.Equal(184, Marshal.SizeOf<LibRawImageSizes>());
    }

    [Fact]
    public void LibRawShootingInfoSizeTest()
    {
        Assert.Equal(142, Marshal.SizeOf<LibRawShootingInfo>());
    }

    #region maker notes
    [Fact]
    public void LibRawMakerNotesSizeTest()
    {
        Assert.Equal(2920 + 4 * IntPtr.Size, Marshal.SizeOf<LibRawMakerNotes>());
    }

    [Fact]
    public void CanonMakerNotesSizeTest()
    {
        Assert.Equal(168, Marshal.SizeOf<LibRawCanonMakerNotes>());
    }

    [Fact]
    public void NikonMakerNotesSizeTest()
    {
        Assert.Equal(224, Marshal.SizeOf<LibRawNikonMakerNotes>());
    }

    [Fact]
    public void HasselbladMakerNotesSizeTest()
    {
        Assert.Equal(384, Marshal.SizeOf<LibRawHasselbladMakerNotes>());
    }

    [Fact]
    public void FujiInfoSizeTest()
    {
        Assert.Equal(280, Marshal.SizeOf<LibRawFujiInfo>());
    }

    [Fact]
    public void OlympusMakerNotesSizeTest()
    {
        Assert.Equal(408, Marshal.SizeOf<LibRawOlympusMakerNotes>());
    }

    [Fact]
    public void SonyInfoSizeTest()
    {
        Assert.Equal(180, Marshal.SizeOf<LibRawSonyInfo>());
    }

    [Fact]
    public void KodakMakerNotesSizeTest()
    {
        Assert.Equal(244, Marshal.SizeOf<LibRawKodakMakerNotes>());
    }

    [Fact]
    public void PanasonicMakerNotesSizeTest()
    {
        Assert.Equal(68, Marshal.SizeOf<LibRawPanasonicMakerNotes>());
    }

    [Fact]
    public void PentaxMakerNotesSizeTest()
    {
        Assert.Equal(32, Marshal.SizeOf<LibRawPentaxMakerNotes>());
    }

    [Fact]
    public void P1MakerNotesSizeTest()
    {
        Assert.Equal(448, Marshal.SizeOf<LibRawP1MakerNotes>());
    }

    [Fact]
    public void RicohMakerNotesSizeTest()
    {
        Assert.Equal(72, Marshal.SizeOf<LibRawRicohMakerNotes>());
    }

    [Fact]
    public void SamsungMakerNotesSizeTest()
    {
        Assert.Equal(136, Marshal.SizeOf<LibRawSamsungMakerNotes>());
    }

    [Fact]
    public void MetadataCommonSizeTest()
    {
        Assert.Equal(264 + 5 * IntPtr.Size, Marshal.SizeOf<LibRawMetadataCommon>());
    }
    #endregion

    [Fact]
    public void LibRawOutputParamsSizeTest()
    {
        Assert.Equal(256 + 6 * IntPtr.Size, Marshal.SizeOf<LibRawOutputParams>());
    }

    [Fact]
    public void RawUnpackParamsSizeTest()
    {
        Assert.Equal(40 + IntPtr.Size, Marshal.SizeOf<RawUnpackParams>());
    }
}
