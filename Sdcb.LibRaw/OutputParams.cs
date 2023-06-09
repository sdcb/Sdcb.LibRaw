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
    public float[] Gamma { get; set; } = null!;

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
    /// <param name="r">The input LibRawOutputParams instance.</param>
    /// <returns>A new OutputParams instance.</returns>
    public static OutputParams FromLibRaw(in LibRawOutputParams r)
    {
        return new OutputParams
        {
            Greybox = Rectangle.FromLTRB((int)r.Greybox[0], (int)r.Greybox[1], (int)r.Greybox[2], (int)r.Greybox[3]),
            Cropbox = Rectangle.FromLTRB((int)r.Cropbox[0], (int)r.Cropbox[1], (int)r.Cropbox[2], (int)r.Cropbox[3]),
            Aber = r.Aber,
            Gamma = Array.ConvertAll(r.Gamma, x => (float)x),
            UserMultipliers = r.UserMultipliers,
            Brightness = r.Brightness,
            Threshold = r.Threshold,
            HalfSize = r.HalfSize,
            FourColorRgb = r.FourColorRgb,
            HighlightMode = r.Highlight,
            UseAutoWb = r.UseAutoWb,
            UseCameraWb = r.UseCameraWb,
            UseCameraMatrix = r.UseCameraMatrix,
            OutputColor = r.OutputColor,
            OutputProfile = Marshal.PtrToStringAnsi(r.OutputProfile),
            CameraProfile = Marshal.PtrToStringAnsi(r.CameraProfile),
            BadPixels = Marshal.PtrToStringAnsi(r.BadPixels),
            DarkFrame = Marshal.PtrToStringAnsi(r.DarkFrame),
            OutputBps = r.OutputBps,
            OutputTiff = r.OutputTiff,
            OutputFlags = r.OutputFlags,
            UserFlip = r.UserFlip,
            UserQual = (DemosaicAlgorithm)r.UserQual,
            UserBlack = r.UserBlack,
            UserCBlack = r.UserCBlack,
            UserSaturation = r.UserSaturation,
            MedianPasses = r.MedianPasses,
            AutoBrightThr = r.AutoBrightThr,
            AdjustMaximumThr = r.AdjustMaximumThr,
            NoAutoBright = r.NoAutoBright,
            UseFujiRotate = r.UseFujiRotate,
            GreenMatching = r.GreenMatching,
            DcbIterations = r.DcbIterations,
            DcbEnhanceFl = r.DcbEnhanceFl,
            FbddNoiserd = r.FbddNoiserd,
            ExpCorrec = r.ExpCorrec,
            ExpShift = r.ExpShift,
            ExpPreser = r.ExpPreser,
            AutoScale = !r.NoAutoScale,
            Interpolation = !r.NoInterpolation
        };
    }

    /// <summary>
    /// Converts the OutputParams object to a LibRawOutputParams object.
    /// </summary>
    /// <returns>The LibRawOutputParams object</returns>
    public LibRawOutputParams ToLibRaw()
    {
        return new LibRawOutputParams
        {
            Greybox = new uint[] { (uint)Greybox.X, (uint)Greybox.Y, (uint)Greybox.Right, (uint)Greybox.Bottom },
            Cropbox = new uint[] { (uint)Cropbox.X, (uint)Cropbox.Y, (uint)Cropbox.Right, (uint)Cropbox.Bottom },
            Aber = Aber,
            Gamma = Array.ConvertAll(Gamma, x => (double)x),
            UserMultipliers = UserMultipliers,
            Brightness = Brightness,
            Threshold = Threshold,
            HalfSize = HalfSize,
            FourColorRgb = FourColorRgb,
            Highlight = HighlightMode,
            UseAutoWb = UseAutoWb,
            UseCameraWb = UseCameraWb,
            UseCameraMatrix = UseCameraMatrix,
            OutputColor = OutputColor,
            OutputProfile = Marshal.StringToHGlobalAnsi(OutputProfile),
            CameraProfile = Marshal.StringToHGlobalAnsi(CameraProfile),
            BadPixels = Marshal.StringToHGlobalAnsi(BadPixels),
            DarkFrame = Marshal.StringToHGlobalAnsi(DarkFrame),
            OutputBps = OutputBps,
            OutputTiff = OutputTiff,
            OutputFlags = OutputFlags,
            UserFlip = UserFlip,
            UserQual = (int)UserQual,
            UserBlack = UserBlack,
            UserCBlack = UserCBlack,
            UserSaturation = UserSaturation,
            MedianPasses = MedianPasses,
            AutoBrightThr = AutoBrightThr,
            AdjustMaximumThr = AdjustMaximumThr,
            NoAutoBright = NoAutoBright,
            UseFujiRotate = UseFujiRotate,
            GreenMatching = GreenMatching,
            DcbIterations = DcbIterations,
            DcbEnhanceFl = DcbEnhanceFl,
            FbddNoiserd = FbddNoiserd,
            ExpCorrec = ExpCorrec,
            ExpShift = ExpShift,
            ExpPreser = ExpPreser,
            NoAutoScale = !AutoScale,
            NoInterpolation = !Interpolation
        };
    }

    /// <summary>
    /// Frees the memory allocated for strings in <paramref name="r"/>.
    /// </summary>
    /// <param name="r">The <see cref="LibRawOutputParams"/> containing strings to be freed.</param>
    public static void FreeLibRawStrings(in LibRawOutputParams r)
    {
        Marshal.FreeHGlobal(r.OutputProfile);
        Marshal.FreeHGlobal(r.CameraProfile);
        Marshal.FreeHGlobal(r.BadPixels);
        Marshal.FreeHGlobal(r.DarkFrame);
    }
}
