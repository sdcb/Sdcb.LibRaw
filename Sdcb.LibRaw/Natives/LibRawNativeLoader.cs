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
                    if (NativeLibrary.TryLoad("raw_r.dll", assembly, searchPath, out IntPtr handle))
                    {
                        return handle;
                    }
                    else
                    {
                        return NativeLibrary.Load("libraw.dll", assembly, searchPath);
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    string[] deps = new[] { "libjpeg.so.8", "liblcms2.so", "libgomp.so.1" };
                    foreach (string dep in deps)
                    {
                        if (!NativeLibrary.TryLoad(dep, assembly, searchPath, out IntPtr _))
                        {
                            Console.WriteLine($"[WARN]: Failed to load linux dependency: {dep}");
                        }
                    }

                    return NativeLibrary.Load("libraw_r.so.23", assembly, searchPath);
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

            return IntPtr.Zero;
        }
    }
}
