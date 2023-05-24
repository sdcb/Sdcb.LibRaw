using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdcb.LibRaw.Natives
{
    /// <summary>
    /// Represents a LibRaw decoder information structure.
    /// </summary>
    /// <remarks>
    /// Provides details about specific LibRaw decoder, including the decoder's name and associated flags.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct LibRawDecoderInfo
    {
        /// <summary>
        /// The name of the LibRaw decoder.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string DecoderName;

        /// <summary>
        /// The flags associated with the LibRaw decoder.
        /// </summary>
        public uint DecoderFlags;
    }
}
