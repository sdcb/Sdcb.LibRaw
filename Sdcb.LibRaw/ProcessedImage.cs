using Sdcb.LibRaw.Natives;
using System;

namespace Sdcb.LibRaw;

/// <summary>
/// Represents a processed image.
/// </summary>
public unsafe class ProcessedImage : IDisposable
{
    private LibRawProcessedImage* _image;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessedImage"/> class.
    /// </summary>
    /// <param name="image">The LibRaw processed image.</param>
    public ProcessedImage(LibRawProcessedImage* image)
    {
        _image = image;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cleanup the managed and unmanaged resources used.
    /// </summary>
    /// <param name="disposing">Flag indicating if <see cref="_image"/> should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                // dispose managed resources here
            }

            FreeImage();
        }
    }

    /// <summary>
    /// Frees the memory used by this instance.
    /// </summary>
    private void FreeImage()
    {
        LibRawNative.FreeImage((IntPtr)_image);
        _image = null;
    }

    /// <summary>
    /// Gets a value indicating whether this instance has been disposed.
    /// </summary>
    public bool Disposed => _image == null;

    /// <summary>
    /// Gets or Sets the image format.
    /// </summary>
    public ProcessedImageType ImageType
    {
        get => _image->Type;
        set => _image->Type = value;
    }

    /// <summary>
    /// Gets or Sets the image height.
    /// </summary>
    public int Height
    {
        get => _image->Height;
        set => _image->Height = (ushort)value;
    }

    /// <summary>
    /// Gets or Sets the image weight.
    /// </summary>
    public int Width
    {
        get => _image->Width;
        set => _image->Width = (ushort)value;
    }

    /// <summary>
    /// Gets or Sets the number of colors in the image.
    /// </summary>
    public int Colors
    {
        get => _image->Colors;
        set => _image->Colors = (ushort)value;
    }

    /// <summary>
    /// Gets or Sets the bits value of the image.
    /// </summary>
    public int Bits
    {
        get => _image->Bits;
        set => _image->Bits = (ushort)value;
    }

    /// <summary>
    /// Gets the data as an array-like <see cref="Span{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>The data in a <see cref="Span{T}"/> object.</returns>
    public unsafe Span<T> GetData<T>()
    {
        return _image->GetData<T>();
    }

    /// <summary>
    /// Allows the finalizer to free memory used by this instance.
    /// </summary>
    ~ProcessedImage()
    {
        Dispose(false);
    }
}
