using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;

public record DecoderInfo(string DecoderName, DecoderFlag DecoderFlags)
{
    public static DecoderInfo FromNative(in LibRawDecoderInfo native) => new(Marshal.PtrToStringAnsi(native.DecoderName)!, native.DecoderFlags);
}
