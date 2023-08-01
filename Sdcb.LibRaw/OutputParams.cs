using Sdcb.LibRaw.Natives;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Sdcb.LibRaw;

/// <summary>Represents a set of output parameters for LibRaw processing.</summary>
public record OutputParams
{
    /// <summary>Gets or sets the grey box for color balance adjustment.</summary>
    public Rectangle Greybox { get; set; }

    /// <summary>Gets or sets the crop box for cropping the image.</summary>
    public Rectangle Cropbox { get; set; }

    /// <summary>Gets or sets the color aberration correction values (red, green).</summary>
    public double[] Aber { get; set; } = null!;

    /// <summary>Gets or sets the gamma curve parameters (ts, t, r, g, b, a).</summary>
    public double[] Gamma { get; set; } = null!;

    /// <summary>Gets or sets the custom white balance multipliers (red, green, blue, green2).</summary>
    public float[] UserMultipliers { get; set; } = null!;

    /// <summary>Gets or sets the brightness adjustment.</summary>
    public float Brightness { get; set; }

    /// <summary>Gets or sets the threshold for zeroing high noise values.</summary>
    public float Threshold { get; set; }

    /// <summary>Gets or sets a value indicating whether to reduce image size to half during processing.</summary>
    public bool HalfSize { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable 4-color RGB interpolation.</summary>
    public bool FourColorRgb { get; set; }

    /// <summary>Gets or sets the highlight recovery mode (0 - clip, 1-9 - unclip using blend).</summary>
    public int HighlightMode { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable automatic white balance based on color temperature.</summary>
    public bool UseAutoWb { get; set; }

    /// <summary>Gets or sets a value indicating whether to use the camera's white balance data.</summary>
    public bool UseCameraWb { get; set; }

    /// <summary>Gets or sets a value indicating whether to use the camera color matrix (1 - use, 0 - do not use).</summary>
    public bool UseCameraMatrix { get; set; }

    /// <summary>Gets or sets the output color space.</summary>
    public LibRawColorSpace OutputColor { get; set; }

    /// <summary>Gets or sets the output profile filename.</summary>
    public string? OutputProfile { get; set; }

    /// <summary>Gets or sets the camera profile filename.</summary>
    public string? CameraProfile { get; set; }

    /// <summary>Gets or sets the bad pixel filename (dead or hot pixels).</summary>
    public string? BadPixels { get; set; }

    /// <summary>Gets or sets the dark frame filename for noise reduction.</summary>
    public string? DarkFrame { get; set; }

    /// <summary>Gets or sets the output bits per sample. 8 or 16.</summary>
    public int OutputBps { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable output in TIFF format.</summary>
    public bool OutputTiff { get; set; }

    /// <summary>Gets or sets output flags.</summary>
    public int OutputFlags { get; set; }

    /// <summary>Gets or sets the user-defined image rotation (0, 90, 180, 270).</summary>
    public int UserFlip { get; set; }

    /// <summary>Gets or sets the demosaic algorithm.</summary>
    public DemosaicAlgorithm UserQual { get; set; }

    /// <summary>Gets or sets the custom black level.</summary>
    public int UserBlack { get; set; }

    /// <summary>Gets or sets the custom black level for each channel.</summary>
    public int[] UserCBlack { get; set; } = null!;

    /// <summary>Gets or sets the custom saturation level.</summary>
    public int UserSaturation { get; set; }

    /// <summary>Gets or sets the number of median passes for noise reduction.</summary>
    public int MedianPasses { get; set; }

    /// <summary>Gets or sets the auto-brightness threshold.</summary>
    public float AutoBrightThr { get; set; }

    /// <summary>Gets or sets the maximum threshold adjustment.</summary>
    public float AdjustMaximumThr { get; set; }

    /// <summary>Gets or sets a value indicating whether to disable automatic brightness adjustment.</summary>
    public bool NoAutoBright { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable Fuji-style image rotation.</summary>
    public bool UseFujiRotate { get; set; }

    /// <summary>Gets or sets the green matching mode.</summary>
    public int GreenMatching { get; set; }

    /// <summary>Gets or sets the number of DCB interpolation iterations.</summary>
    public int DcbIterations { get; set; }

    /// <summary>Gets or sets the DCB enhance flag (1 - use, 0 - do not use).</summary>
    public int DcbEnhanceFl { get; set; }

    /// <summary>Gets or sets the FBDD noise reduction mode.</summary>
    public int FbddNoiserd { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable exposure correction.</summary>
    public bool ExpCorrec { get; set; }

    /// <summary>Gets or sets the exposure shift value.</summary>
    public float ExpShift { get; set; }

    /// <summary>Gets or sets the exposure preservation value.</summary>
    public float ExpPreser { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable auto-scaling of the image.</summary>
    public bool AutoScale { get; set; }

    /// <summary>Gets or sets a value indicating whether to enable image interpolation.</summary>
    public bool Interpolation { get; set; }

    /// <summary>
    /// Creates and returns a new OutputParams instance based on the provided LibRawOutputParams instance.
    /// </summary>
    /// <param name="rawData">The pointer of libraw_data_t.</param>
    /// <returns>A new OutputParams instance.</returns>
    public static OutputParams FromLibRaw(IntPtr rawData)
    {
        if (IntPtr.Size == 8)
        {
            return FromLibRawX64(rawData);
        }
        else
        {

            return FromLibRawX86(rawData);
        }

        unsafe static OutputParams FromLibRawX64(IntPtr rawData)
        {
            LibRawDataX64* data = (LibRawDataX64*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            return ReadFromNative(r);
        }

        unsafe static OutputParams FromLibRawX86(IntPtr rawData)
        {
            LibRawDataX86* data = (LibRawDataX86*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            return ReadFromNative(r);
        }

        unsafe static OutputParams ReadFromNative(NativeOutputParams* r)
        {
            return new OutputParams
            {
                Greybox = new Rectangle((int)r->Greybox[0], (int)r->Greybox[1], (int)r->Greybox[2], (int)r->Greybox[3]),
                Cropbox = new Rectangle((int)r->Cropbox[0], (int)r->Cropbox[1], (int)r->Cropbox[2], (int)r->Cropbox[3]),
                Aber = new ReadOnlySpan<double>(r->Aber, 4).ToArray(),
                Gamma = new ReadOnlySpan<double>(r->Gamm, 6).ToArray(),
                UserMultipliers = new ReadOnlySpan<float>(r->UserMul, 4).ToArray(),
                Brightness = r->Bright,
                Threshold = r->Threshold,
                HalfSize = r->HalfSize != 0,
                FourColorRgb = r->FourColorRgb != 0,
                HighlightMode = r->Highlight,
                UseAutoWb = r->UseAutoWb != 0,
                UseCameraWb = r->UseCameraWb != 0,
                UseCameraMatrix = r->UseCameraMatrix != 0,
                OutputColor = (LibRawColorSpace)r->OutputColor,
                OutputProfile = Marshal.PtrToStringAnsi(r->OutputProfile),
                CameraProfile = Marshal.PtrToStringAnsi(r->CameraProfile),
                BadPixels = Marshal.PtrToStringAnsi(r->BadPixels),
                DarkFrame = Marshal.PtrToStringAnsi(r->DarkFrame),
                OutputBps = r->OutputBps,
                OutputTiff = r->OutputTiff != 0,
                OutputFlags = r->OutputFlags,
                UserFlip = r->UserFlip,
                UserQual = (DemosaicAlgorithm)r->UserQual,
                UserBlack = r->UserBlack,
                UserCBlack = new ReadOnlySpan<int>(r->UserCblack, 4).ToArray(),
                UserSaturation = r->UserSat,
                MedianPasses = r->MedPasses,
                AutoBrightThr = r->AutoBrightThr,
                AdjustMaximumThr = r->AdjustMaximumThr,
                NoAutoBright = r->NoAutoBright != 0,
                UseFujiRotate = r->UseFujiRotate != 0,
                GreenMatching = r->GreenMatching,
                DcbIterations = r->DcbIterations,
                DcbEnhanceFl = r->DcbEnhanceFl,
                FbddNoiserd = r->FbddNoiserd,
                ExpCorrec = r->ExpCorrec != 0,
                ExpShift = r->ExpShift,
                ExpPreser = r->ExpPreser,
                AutoScale = r->NoAutoScale == 0,
                Interpolation = r->NoInterpolation == 0,
            };
        }
    }

    /// <summary>
    /// Updates the LibRaw raw data from the specified pointer. 
    /// </summary>
    /// <param name="rawData">The pointer of libraw_data_t</param>
    /// <exception cref="InvalidOperationException">Array element size not match.</exception>
    public void Commit(IntPtr rawData)
    {
        // Check array lengths
        if (Aber.Length != 4)
            throw new InvalidOperationException($"{nameof(Aber)} array must contain exactly 4 elements");
        if (Gamma.Length != 6)
            throw new InvalidOperationException($"{nameof(Gamma)} array must contain exactly 6 elements");
        if (UserMultipliers.Length != 4)
            throw new InvalidOperationException($"{nameof(UserMultipliers)} array must contain exactly 4 elements");
        if (UserCBlack.Length != 4)
            throw new InvalidOperationException($"{nameof(UserCBlack)} array must contain exactly 4 elements");

        if (IntPtr.Size == 8)
        {
            UpdateX64(rawData);
        }
        else
        {
            UpdateX86(rawData);
        }

        unsafe void UpdateX64(IntPtr rawData)
        {
            LibRawDataX64* data = (LibRawDataX64*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            UpdateRaw(r);
        }

        unsafe void UpdateX86(IntPtr rawData)
        {
            LibRawDataX86* data = (LibRawDataX86*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            UpdateRaw(r);
        }

        unsafe void UpdateRaw(NativeOutputParams* r)
        {
            new uint[] { (uint)Greybox.Left, (uint)Greybox.Top, (uint)Greybox.Width, (uint)Greybox.Height }.AsSpan().CopyTo(new Span<uint>(r->Greybox, 4));
            new uint[] { (uint)Cropbox.Left, (uint)Cropbox.Top, (uint)Cropbox.Width, (uint)Cropbox.Height }.AsSpan().CopyTo(new Span<uint>(r->Cropbox, 4));

            Aber.AsSpan().CopyTo(new Span<double>(r->Aber, 4));
            Gamma.AsSpan().CopyTo(new Span<double>(r->Gamm, 6));
            UserMultipliers.AsSpan().CopyTo(new Span<float>(r->UserMul, 4));

            r->Bright = Brightness;
            r->Threshold = Threshold;
            r->HalfSize = HalfSize ? 1 : 0;
            r->FourColorRgb = FourColorRgb ? 1 : 0;
            r->Highlight = HighlightMode;
            r->UseAutoWb = UseAutoWb ? 1 : 0;
            r->UseCameraWb = UseCameraWb ? 1 : 0;
            r->UseCameraMatrix = UseCameraMatrix ? 1 : 0;
            r->OutputColor = (int)OutputColor;
            r->OutputProfile = Marshal.StringToHGlobalAnsi(OutputProfile);
            r->CameraProfile = Marshal.StringToHGlobalAnsi(CameraProfile);
            r->BadPixels = Marshal.StringToHGlobalAnsi(BadPixels);
            r->DarkFrame = Marshal.StringToHGlobalAnsi(DarkFrame);
            r->OutputBps = OutputBps;
            r->OutputTiff = OutputTiff ? 1 : 0;
            r->OutputFlags = OutputFlags;
            r->UserFlip = UserFlip;
            r->UserQual = (int)UserQual;
            r->UserBlack = UserBlack;

            UserCBlack.AsSpan().CopyTo(new Span<int>(r->UserCblack, 4));

            r->UserSat = UserSaturation;
            r->MedPasses = MedianPasses;
            r->AutoBrightThr = AutoBrightThr;
            r->AdjustMaximumThr = AdjustMaximumThr;
            r->NoAutoBright = NoAutoBright ? 1 : 0;
            r->UseFujiRotate = UseFujiRotate ? 1 : 0;
            r->GreenMatching = GreenMatching;
            r->DcbIterations = DcbIterations;
            r->DcbEnhanceFl = DcbEnhanceFl;
            r->FbddNoiserd = FbddNoiserd;
            r->ExpCorrec = ExpCorrec ? 1 : 0;
            r->ExpShift = ExpShift;
            r->ExpPreser = ExpPreser;
            r->NoAutoScale = AutoScale ? 0 : 1;
            r->NoInterpolation = Interpolation ? 0 : 1;
        }
    }


    /// <summary>
    /// Frees the memory allocated for strings in <see cref="NativeOutputParams"/>.
    /// </summary>
    /// <param name="rawData">libraw_data_t that containing libraw_output_params_t's strings to free.</param>
    public static void FreeLibRawStrings(IntPtr rawData)
    {
        if (IntPtr.Size == 8)
        {
            FreeX64(rawData);
        }
        else
        {
            FreeX86(rawData);
        }

        unsafe static void FreeX64(IntPtr rawData)
        {
            LibRawDataX64* data = (LibRawDataX64*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            FreeRaw(r);
        }

        unsafe static void FreeX86(IntPtr rawData)
        {
            LibRawDataX86* data = (LibRawDataX86*)rawData;
            NativeOutputParams* r = &data->OutputParams;
            FreeRaw(r);
        }

        unsafe static void FreeRaw(NativeOutputParams* r)
        {
            Marshal.FreeHGlobal(r->OutputProfile);
            r->OutputProfile = IntPtr.Zero;
            Marshal.FreeHGlobal(r->CameraProfile);
            r->CameraProfile = IntPtr.Zero;
            Marshal.FreeHGlobal(r->BadPixels);
            r->BadPixels = IntPtr.Zero;
            Marshal.FreeHGlobal(r->DarkFrame);
            r->DarkFrame = IntPtr.Zero;
        }
    }
}
