using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw.Natives
{
    internal class LibRawNativeLoader
    {
        static LibRawNativeLoader()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), LibRawImportResolver);
        }

        public static void Init()
        {
            // stub to ensure static constructor executed at least once.
        }

        private static IntPtr LibRawImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == LibRawNative.Dll)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load("libraw.dll", assembly, searchPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return NativeLibrary.Load("libraw.so.23", assembly, searchPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return NativeLibrary.Load("libraw.23.dylib", assembly, searchPath);
                }
                else
                {
                    throw new NotSupportedException("Not support current OS's LibRaw.");
                }
            }
            else if (libraryName == OpenMpLib.Dll)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load("vcomp140.dll", assembly, searchPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return NativeLibrary.Load("libgomp.so.1", assembly, searchPath);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return NativeLibrary.Load("libomp.dylib", assembly, searchPath);
                }
                else
                {
                    throw new NotSupportedException("Not support current OS's OpenMP.");
                }
            }

            return IntPtr.Zero;
        }
    }
}
