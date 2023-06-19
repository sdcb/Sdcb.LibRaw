using Sdcb.LibRaw.Natives;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;

/// <summary>
/// Represents a decoder information record.
/// </summary>
/// <param name="DecoderName">The name of the decoder.</param>
/// <param name="DecoderFlags"> The flag of the decoder.</param>
public record DecoderInfo(string DecoderName, DecoderFlag DecoderFlags)
{
    /// <summary>
    /// Creates a DecoderInfo record from the given LibRawDecoderInfo object.
    /// </summary>
    /// <param name="native">The native decoder information object.</param>
    /// <returns>The created DecoderInfo record.</returns>
    public static DecoderInfo FromNative(in LibRawDecoderInfo native) => new(Marshal.PtrToStringAnsi(native.DecoderName)!, native.DecoderFlags);
}
