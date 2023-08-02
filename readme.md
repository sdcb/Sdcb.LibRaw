# Sdcb.LibRaw ![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.svg?style=flat-square&label=nuget) ![NuGet](https://img.shields.io/nuget/dt/Sdcb.LibRaw.svg?style=flat-square) ![GitHub](https://img.shields.io/github/license/sdcb/Sdcb.LibRaw.svg?style=flat-square&label=license) [![QQ](https://img.shields.io/badge/QQ_Group-495782587-52B6EF?style=social&logo=tencent-qq&logoColor=000&logoWidth=20)](http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=mma4msRKd372Z6dWpmBp4JZ9RL4Jrf8X&authKey=gccTx0h0RaH5b8B8jtuPJocU7MgFRUznqbV%2FLgsKdsK8RqZE%2BOhnETQ7nYVTp1W0&noverify=0&group_code=495782587)

Sdcb.LibRaw is an advanced raw image processing library in C#, based on [LibRaw](https://www.libraw.org/).

## NuGet Packages

| Package                     | NuGet                                                                                                                                                                 | License                   | Comments                   |
| --------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------- | -------------------------- |
| Sdcb.LibRaw                 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw/)                                 | MIT                       | Primary package            |
| Sdcb.LibRaw.runtime.win64   | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.win64.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.win64/)     | LGPL-2.1-only OR CDDL-1.0 | Windows x64 runtime        |
| Sdcb.LibRaw.runtime.win32   | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.win32.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.win32/)     | LGPL-2.1-only OR CDDL-1.0 | Windows x86 runtime        |
| Sdcb.LibRaw.runtime.linux64 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.linux64.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.linux64/) | LGPL-2.1-only OR CDDL-1.0 | Ubuntu 22.04 x64 runtime   |

## Install
Please note all examples below need to install at least two of the following NuGet packages:
* Sdcb.LibRaw
* Sdcb.LibRaw.runtime.win64 (or other system-specific runtime packages)

All native packages are pre-compiled using `vcpkg`.

Please note the `Sdcb.LibRaw.runtime.linux-64` package is only supported on Ubuntu 22.04. If you are using .NET in Docker, append `jammy` to your docker file. For example, write:

```
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy
```

Instead of:

```
FROM mcr.microsoft.com/dotnet/sdk:6.0
```

## High-Level API Usage
The `RawContext` class wraps all `LibRaw` native functions for high-level API usage. See the following examples for more details.

### Convert a RAW file into a Bitmap

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\a7r3\DSC02653.ARW");
r.Unpack();
r.DcrawProcess();
using ProcessedImage image = r.MakeDcrawMemoryImage();
using Bitmap bmp = ProcessedImageToBitmap(image);

Bitmap ProcessedImageToBitmap(ProcessedImage rgbImage)
{
    rgbImage.SwapRGB();
    using Bitmap bmp = new Bitmap(rgbImage.Width, rgbImage.Height, rgbImage.Width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, rgbImage.DataPointer);
    return new Bitmap(bmp);
}
```

This code shows how to convert a RAW file into a Bitmap image. It's worth noting that the pixel format output by `LibRaw` is slightly different from Bitmap's. In the code above, `rgbImage.SwapRGB();` is used to swap the red and blue color channels, i.e., converting RGB24 to BGR24.

Although the example is based on the `.ARW` photo, it actually supports almost all RAW format photos, including `.CR2` and `.DNG`. You can retrieve a list of supported cameras using `RawContext.SupportedCameras`. As of the current version, it supports 1182 camera models:

```csharp
Console.WriteLine("Sdcb.LibRaw supported cameras:");
foreach (string model in RawContext.SupportedCameras)
{
	Console.WriteLine(model);
}
```

### Convert a RAW file into a Bitmap with custom settings

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\a7r3\DSC02653.ARW");
r.DcrawProcess(c =>
{
    c.HalfSize = false; // Reduce the image size to 1/4
    c.UseCameraWb = true; // Use camera white balance; if false, the white balance will be controlled by UserMultipliers
    c.Gamma[0] = 0.35; // Adjust the exponent of the gamma curve
    c.Gamma[1] = 3.5;  // Adjust the slope of the gamma curve
    c.Brightness = 2.2f; // Brightness
    c.Interpolation = true; // Perform demosaic operation
    c.OutputBps = 8; // Output bit depth is 8 bits
    c.OutputTiff = false; // Output a TIFF file? If false, a Bitmap will be output
    // c.Cropbox = new Rectangle(4000, 2000, 1500, 700); // Crop the image
    // Many other settings can be explored at will
});
using ProcessedImage rgbImage = r.MakeDcrawMemoryImage();
using Bitmap bmp = ProcessedImageToBitmap(rgbImage);
```

### Get a thumbnail from a RAW file

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\a7r3\DSC02653.ARW");
using ProcessedImage image = r.ExportThumbnail(thumbnailIndex: 0);
using Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(image.AsSpan<byte>().ToArray()));
```

Modern RAW format photos often contain one or more JPEG format thumbnails. You can use `Sdcb.LibRaw` to extract these thumbnails. The above example shows how to extract the first thumbnail from an `ARW` photo and convert it into a `Bitmap`.

### Save a RAW file as a local TIFF file

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\a7r3\DSC02653.ARW");
r.SaveRawImage(@"C:\test\test.tiff");
```

### Get metadata from a photo

```csharp
using RawContext r = RawContext.OpenFile(@"C:\a7r3\DSC02653.ARW");
LibRawImageParams imageParams = r.ImageParams;
LibRawImageOtherParams otherParams = r.ImageOtherParams;
LibRawLensInfo lensInfo = r.LensInfo;

Console.WriteLine($"Camera: {imageParams.Model}");
Console.WriteLine($"Version: {imageParams.Software}");
Console.WriteLine($"ISO: {otherParams.IsoSpeed}");
Console.WriteLine($"Shutter Speed: 1/{1 / otherParams.Shutter:F0}s");
Console.WriteLine($"Focal Length: {otherParams.FocalLength}mm");
Console.WriteLine($"Artist Tag: {otherParams.Artist}");
Console.WriteLine($"Shot Date: {new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(otherParams.Timestamp)}");
Console.WriteLine($"Lens Name: {lensInfo.Lens}");
```

## Performance and Comparison

| Method                      | Time (ms) | Notes                |
| --------------------------- | --------- | -------------------- |
| Sdcb.LibRaw                 | 1627      |                      |
| Windows Imaging Component   | 2177      | Limited post-process |
| Magick.NET                  | 5496      | Limited post-process |
| Native C code               | 1619      |                      |

As you can see, `Sdcb.LibRaw` has top-tier performance and its powerful image post-processing capability is based on Bayer data.


## Low level API

### API mapping reference

You can refer to following 2 pages to find out which C API is mapped to which C# API:
* [Table styled mapping reference](./docs/c-api-mapping-table.md)
* [List styled mapping reference](./docs/c-api-mapping.md)

### Low level API usage
You can refer to existing [unit tests](https://github.com/sdcb/Sdcb.LibRaw/tree/master/tests/Sdcb.LibRaw.UnitTests/RawApiTests) for low level api usage.

## License
The primary project(Sdcb.LibRaw) is licensed under the [MIT license](./LICENSE.txt), but native packages are licensed under **LGPL-2.1-only OR CDDL-1.0**.

Please refer to [LibRaw license](https://www.libraw.org/) for more details.

## Conclusion
I understand that not many people need to process RAW format photos with code. However, with the development of smartphones, many mobile phones can now shoot RAW format photos. I firmly believe that this tool will bring great help to those who need it. I will continue to put effort into this field, add more features to `Sdcb.LibRaw`, and solve possible problems.

The above content is just a part of the features of the `Sdcb.LibRaw` library I created. Its powerful functions and high efficiency will bring unprecedented convenience to your processing of RAW format photos. I sincerely hope that more `.NET` enthusiasts can join me to explore the world of RAW photo processing. `Sdcb.LibRaw` will always remain useful and free. Let's look forward to more wonderful features!

If you are interested in trying `Sdcb.LibRaw`, welcome to visit my [Github](https://github.com/sdcb/Sdcb.LibRaw), and please give a Star 🌟
