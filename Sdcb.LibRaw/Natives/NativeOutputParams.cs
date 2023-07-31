using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives;

[StructLayout(LayoutKind.Sequential)]
internal struct NativeOutputParams
{
    public unsafe fixed uint Greybox[4];

    public unsafe fixed uint Cropbox[4];

    public unsafe fixed double Aber[4];

    public unsafe fixed double Gamm[6];

    public unsafe fixed float UserMul[4];

    public float Bright;

    public float Threshold;

    public int HalfSize;

    public int FourColorRgb;

    public int Highlight;

    public int UseAutoWb;

    public int UseCameraWb;

    public int UseCameraMatrix;

    public int OutputColor;

    public IntPtr OutputProfile;

    public IntPtr CameraProfile;

    public IntPtr BadPixels;

    public IntPtr DarkFrame;

    public int OutputBps;

    public int OutputTiff;

    public int OutputFlags;

    public int UserFlip;

    public int UserQual;

    public int UserBlack;

    public unsafe fixed int UserCblack[4];

    public int UserSat;

    public int MedPasses;

    public float AutoBrightThr;

    public float AdjustMaximumThr;

    public int NoAutoBright;

    public int UseFujiRotate;

    public int GreenMatching;

    public int DcbIterations;

    public int DcbEnhanceFl;

    public int FbddNoiserd;

    public int ExpCorrec;

    public float ExpShift;

    public float ExpPreser;

    public int NoAutoScale;

    public int NoInterpolation;
}
