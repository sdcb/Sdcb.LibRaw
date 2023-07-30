using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives;

[StructLayout(LayoutKind.Explicit, Size = 304)]
internal struct OutputParamsX64
{
    [FieldOffset(0)]
    public uint Greybox;

    [FieldOffset(16)]
    public uint Cropbox;

    [FieldOffset(32)]
    public double Aber;

    [FieldOffset(64)]
    public double Gamm;

    [FieldOffset(112)]
    public float UserMul;

    [FieldOffset(128)]
    public float Bright;

    [FieldOffset(132)]
    public float Threshold;

    [FieldOffset(136)]
    public int HalfSize;

    [FieldOffset(140)]
    public int FourColorRgb;

    [FieldOffset(144)]
    public int Highlight;

    [FieldOffset(148)]
    public int UseAutoWb;

    [FieldOffset(152)]
    public int UseCameraWb;

    [FieldOffset(156)]
    public int UseCameraMatrix;

    [FieldOffset(160)]
    public int OutputColor;

    [FieldOffset(168)]
    public IntPtr OutputProfile;

    [FieldOffset(176)]
    public IntPtr CameraProfile;

    [FieldOffset(184)]
    public IntPtr BadPixels;

    [FieldOffset(192)]
    public IntPtr DarkFrame;

    [FieldOffset(200)]
    public int OutputBps;

    [FieldOffset(204)]
    public int OutputTiff;

    [FieldOffset(208)]
    public int OutputFlags;

    [FieldOffset(212)]
    public int UserFlip;

    [FieldOffset(216)]
    public int UserQual;

    [FieldOffset(220)]
    public int UserBlack;

    [FieldOffset(224)]
    public int UserCblack;

    [FieldOffset(240)]
    public int UserSat;

    [FieldOffset(244)]
    public int MedPasses;

    [FieldOffset(248)]
    public float AutoBrightThr;

    [FieldOffset(252)]
    public float AdjustMaximumThr;

    [FieldOffset(256)]
    public int NoAutoBright;

    [FieldOffset(260)]
    public int UseFujiRotate;

    [FieldOffset(264)]
    public int GreenMatching;

    [FieldOffset(268)]
    public int DcbIterations;

    [FieldOffset(272)]
    public int DcbEnhanceFl;

    [FieldOffset(276)]
    public int FbddNoiserd;

    [FieldOffset(280)]
    public int ExpCorrec;

    [FieldOffset(284)]
    public float ExpShift;

    [FieldOffset(288)]
    public float ExpPreser;

    [FieldOffset(292)]
    public int NoAutoScale;

    [FieldOffset(296)]
    public int NoInterpolation;
}

[StructLayout(LayoutKind.Explicit, Size = 280)]
internal struct OutputParamsX86
{
    [FieldOffset(0)]
    public uint Greybox;

    [FieldOffset(16)]
    public uint Cropbox;

    [FieldOffset(32)]
    public double Aber;

    [FieldOffset(64)]
    public double Gamm;

    [FieldOffset(112)]
    public float UserMul;

    [FieldOffset(128)]
    public float Bright;

    [FieldOffset(132)]
    public float Threshold;

    [FieldOffset(136)]
    public int HalfSize;

    [FieldOffset(140)]
    public int FourColorRgb;

    [FieldOffset(144)]
    public int Highlight;

    [FieldOffset(148)]
    public int UseAutoWb;

    [FieldOffset(152)]
    public int UseCameraWb;

    [FieldOffset(156)]
    public int UseCameraMatrix;

    [FieldOffset(160)]
    public int OutputColor;

    [FieldOffset(164)]
    public IntPtr OutputProfile;

    [FieldOffset(168)]
    public IntPtr CameraProfile;

    [FieldOffset(172)]
    public IntPtr BadPixels;

    [FieldOffset(176)]
    public IntPtr DarkFrame;

    [FieldOffset(180)]
    public int OutputBps;

    [FieldOffset(184)]
    public int OutputTiff;

    [FieldOffset(188)]
    public int OutputFlags;

    [FieldOffset(192)]
    public int UserFlip;

    [FieldOffset(196)]
    public int UserQual;

    [FieldOffset(200)]
    public int UserBlack;

    [FieldOffset(204)]
    public int UserCblack;

    [FieldOffset(220)]
    public int UserSat;

    [FieldOffset(224)]
    public int MedPasses;

    [FieldOffset(228)]
    public float AutoBrightThr;

    [FieldOffset(232)]
    public float AdjustMaximumThr;

    [FieldOffset(236)]
    public int NoAutoBright;

    [FieldOffset(240)]
    public int UseFujiRotate;

    [FieldOffset(244)]
    public int GreenMatching;

    [FieldOffset(248)]
    public int DcbIterations;

    [FieldOffset(252)]
    public int DcbEnhanceFl;

    [FieldOffset(256)]
    public int FbddNoiserd;

    [FieldOffset(260)]
    public int ExpCorrec;

    [FieldOffset(264)]
    public float ExpShift;

    [FieldOffset(268)]
    public float ExpPreser;

    [FieldOffset(272)]
    public int NoAutoScale;

    [FieldOffset(276)]
    public int NoInterpolation;
}
