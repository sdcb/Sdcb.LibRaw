# Sdcb.LibRaw [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw/) [![NuGet](https://img.shields.io/nuget/dt/Sdcb.LibRaw.svg?style=flat-square)](https://www.nuget.org/packages/Sdcb.LibRaw/) [![GitHub](https://img.shields.io/github/license/sdcb/Sdcb.LibRaw.svg?style=flat-square&label=license)](https://github.com/sdcb/Sdcb.LibRaw/blob/master/LICENSE.txt)

Advanced raw image processing library in C# based on [LibRaw](https://www.libraw.org/).

## NuGet Packages

| Package                     | NuGet                                                                                                                                                                 | License                   | Comments            |
| --------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------- | ------------------- |
| Sdcb.LibRaw                 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw/)                                 | MIT                       | Primary package     |
| Sdcb.LibRaw.runtime.win64   | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.win64.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.win64/)     | LGPL-2.1-only OR CDDL-1.0 | Windows x64 runtime |
| Sdcb.LibRaw.runtime.win32   | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.win32.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.win32/)     | LGPL-2.1-only OR CDDL-1.0 | Windows x86 runtime |
| Sdcb.LibRaw.runtime.linux64 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.LibRaw.runtime.linux64.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Sdcb.LibRaw.runtime.linux64/) | LGPL-2.1-only OR CDDL-1.0 | Linux x64 runtime   |

## Install
Please note all examples below need install following NuGet packages:
* Sdcb.LibRaw
* Sdcb.LibRaw.runtime.win64 
* Sdcb.LibRaw.runtime.win32
* Sdcb.LibRaw.runtime.linux-64 (for Ubuntu22.04 support)

All native packages are pre-compiled using `vcpkg`.

Please note the `Sdcb.LibRaw.runtime.linux-64` package is only support Ubuntu22.04, if you using .NET in Docker, you should append `jammy` when write docker file, for example, you should write:

```
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy
```

Instead of:

```
FROM mcr.microsoft.com/dotnet/sdk:6.0
```

if you want to use in other Linux distros without docker, please install `libraw` using OS provided package manager, for example, in `Debian 11`, you can install by running following bash command to install:

```bash
echo "deb http://deb.debian.org/debian experimental main" | tee -a /etc/apt/sources.list
echo "deb-src http://deb.debian.org/debian experimental main" | tee -a /etc/apt/sources.list
apt-get update
apt-get -t experimental install libraw-dev
```

Likewise in MacOS, you also need to install `libraw` using MacOS provided package manager.

## High level API Usage
For high level API usage, I created `RawContext` class to wrap all `LibRaw` native functions, you can refer to following examples for more details.

### Convert raw file into bitmap

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\Users\ZhouJie\Pictures\a7r3\11030126\DSC02653.ARW");
r.Unpack();
r.DcrawProcess();
using ProcessedImage image = r.MakeDcrawMemoryImage();
using Bitmap bmp = ProcessedImageToBitmap(image);

Bitmap ProcessedImageToBitmap(ProcessedImage rgbImage)
{
	fixed (void* data = rgbImage.GetData<byte>())
	{
		SwapRedAndBlue(rgbImage.GetData<byte>(), rgbImage.Width, rgbImage.Height);
		using Bitmap bmp = new Bitmap(rgbImage.Width, rgbImage.Height, rgbImage.Width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)data);
		return new Bitmap(bmp);
	}
}

void SwapRedAndBlue(Span<byte> rgbData, int width, int height)
{
	int totalPixels = width * height;
	for (int i = 0; i < totalPixels; i++)
	{
		int pixelIndex = i * 3;
		byte red = rgbData[pixelIndex];
		byte blue = rgbData[pixelIndex + 2];

		rgbData[pixelIndex] = blue;
		rgbData[pixelIndex + 2] = red;
	}
}
```

### Convert raw file into bitmap with custom settings

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\Users\ZhouJie\Pictures\a7r3\11030126\DSC02653.ARW");
// r.ExportRawImage is a shortcut for r.Unpack() + r.DcrawProcess() + r.MakeDcrawMemoryImage()
r.Gamma[0] = 0.55f; // gamma for inverse power
r.Gamma[1] = 3.5f;  // gamma for slope
using ProcessedImage rgbImage = r.ExportRawImage();
using Bitmap bmp = ProcessedImageToBitmap(rgbImage);

// ProcessedImageToBitmap is the same as above
// SwapRedAndBlue is the same as above
```

### Get raw file thumbnail

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\Users\ZhouJie\Pictures\a7r3\11030126\DSC02653.ARW");
// r.ExportThumbnail() is a shortcut for r.UnpackThumbnail() + r.MakeDcrawMemoryThumbnail()
using ProcessedImage image = r.ExportThumbnail(thumbnailIndex: 0);
using Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(image.GetData<byte>().ToArray()));
```

### Save raw file as tiff into local file

```csharp
using Sdcb.LibRaw;

using RawContext r = RawContext.OpenFile(@"C:\Users\ZhouJie\Pictures\a7r3\11030126\DSC02653.ARW");
// r.SaveRawImage() is a shortcut for r.Unpack() + r.DcrawProcess() + r.WriteDcrawPpmTiff(fileName)
r.SaveRawImage(@"C:\test\test.tiff");
```

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
