using Sdcb.LibRaw.Natives;
using System;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;


/// <summary>
/// Provides a wrapper for OpenMP library APIs that provide support for parallel programming in C# applications.
/// </summary>
public class OpenMpLib
{
    static OpenMpLib()
    {
        LibRawNativeLoader.Init();
    }

    internal const string Dll = "vcomp140.dll";

    /// <summary>
    /// Sets the number of threads to be used for the next parallel region encountered.
    /// </summary>
    /// <param name="num_threads">The number of threads to be used for the next parallel region encountered. If num_threads is negative, then the reserved threads are disabled in the next parallel region.</param>
    [DllImport(Dll, EntryPoint = "omp_set_num_threads", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern void omp_set_num_threads(int num_threads);

    /// <summary>
    /// Returns the number of threads currently in the team executing the parallel region from which it is called.
    /// </summary>
    /// <returns>The number of threads currently in the team executing the parallel region from which it is called.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_num_threads", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_get_num_threads();

    /// <summary>
    /// Returns the maximum number of threads that could be used if a parallel region without the num_threads clause was encountered.
    /// </summary>
    /// <returns>The maximum number of threads that could be used if a parallel region without the num_threads clause was encountered.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_max_threads", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_get_max_threads();

    /// <summary>
    /// Returns the thread number, within the current team, of the calling thread.
    /// </summary>
    /// <returns>The thread number, within the current team, of the calling thread.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_thread_num", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_get_thread_num();

    /// <summary>
    /// Returns the number of processors available to the program.
    /// </summary>
    /// <returns>The number of processors available to the program.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_num_procs", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_get_num_procs();

    /// <summary>
    /// Returns true if the code is executing inside a parallel region and false otherwise.
    /// </summary>
    /// <returns>True if the code is executing inside a parallel region and false otherwise.</returns>
    [DllImport(Dll, EntryPoint = "omp_in_parallel", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_in_parallel();

    /// <summary>
    /// Enables or disables the dynamic adjustment of the number of threads in subsequent parallel regions.
    /// </summary>
    /// <param name="dynamic_threads">If dynamic_threads is nonzero, then the run-time system will make a decision whether to use fewer threads than the maximum number specified at the commencement of the parallel region.</param>
    [DllImport(Dll, EntryPoint = "omp_set_dynamic", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern void omp_set_dynamic(int dynamic_threads);

    /// <summary>
    /// Enables or disables the nested parallelism.
    /// </summary>
    /// <param name="nested_threads">If nested_threads is zero, nested parallelism is disabled. Otherwise, it is enabled.</param>
    [DllImport(Dll, EntryPoint = "omp_set_nested", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern void omp_set_nested(int nested_threads);

    /// <summary>
    /// Return a nonzero value if nested parallelism is enabled, or 0 otherwise.
    /// </summary>
    /// <returns>A nonzero value if nested parallelism is enabled, or 0 otherwise.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_nested", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern int omp_get_nested();

    /// <summary>
    /// Returns elapsed wall-clock time in seconds as a floating-point number.
    /// </summary>
    /// <returns>Elapsed wall-clock time in seconds as a floating-point number.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_wtime", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern double omp_get_wtime();

    /// <summary>
    /// Returns the timer precision, which is the number of seconds between successive clock ticks.
    /// </summary>
    /// <returns>The timer precision, which is the number of seconds between successive clock ticks.</returns>
    [DllImport(Dll, EntryPoint = "omp_get_wtick", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    public static extern double omp_get_wtick();

    // other apis to PInvoke
    // * omp_get_dynamic
    // * omp_get_cancellation
    // * omp_set_schedule
    // * omp_get_schedule
    // * omp_get_thread_limit
    // ...
}
