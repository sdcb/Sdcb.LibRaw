using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives;

[StructLayout(LayoutKind.Explicit)]
internal struct LibRawDataX86
{
    [FieldOffset(5008)]
    public NativeOutputParams OutputParams;

    [FieldOffset(5332)]
    public LibRawProgress Progress;
}

[StructLayout(LayoutKind.Explicit)]
internal struct LibRawDataX64
{
    [FieldOffset(5024)]
    public NativeOutputParams OutputParams;

    [FieldOffset(5376)]
    public LibRawProgress Progress;
}
