using Sdcb.LibRaw.Natives;
using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw
{
    /// <summary>
    /// Represents an error that occurs while using the LibRaw library.
    /// </summary>
    public class LibRawException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibRawException"/> class with a specified error code and message.
        /// </summary>
        /// <param name="error">The error code that describes the error.</param>
        /// <param name="message">The message that describes the error.</param>
        public LibRawException(LibRawError error, string? message)
            : base(message ?? $"ErrorCode: {(int)error}({error}), {Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(error))!}")
        {
            ErrorCode = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibRawException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LibRawException(string message)
                       : base(message)
        {
            ErrorCode = LibRawError.UnspecifiedError;
        }

        /// <summary>
        /// Gets a string that describes the error set by the <see cref="ErrorCode"/> property.
        /// </summary>
        public string ErrorExplain => Marshal.PtrToStringAnsi(LibRawNative.GetErrorMessage(ErrorCode))!;

        /// <summary>
        /// Gets the error code that describes the error that caused the exception.
        /// </summary>
        public LibRawError ErrorCode { get; }

        public static void ThrowIfFailed(LibRawError error, string? errorMessage = null)
        {
            if (error != LibRawError.Success)
            {
                throw new LibRawException(error, errorMessage);
            }
        }
    }
}
