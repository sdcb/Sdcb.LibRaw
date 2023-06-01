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
            if (_image != null)
            {
                FreeImage();
            }
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
    /// Gets the image format.
    /// </summary>
    public ProcessedImageType ImageType => _image->Type;

    /// <summary>
    /// Gets the image height.
    /// </summary>
    public int Height => _image->Height;

    /// <summary>
    /// Gets the image weight.
    /// </summary>
    public int Width => _image->Width;

    /// <summary>
    /// Gets the number of colors in the image.
    /// </summary>
    public int Colors => _image->Colors;

    /// <summary>
    /// Gets the bits value of the image.
    /// </summary>
    public int Bits => _image->Bits;

    /// <summary>
    /// Gets the data as an array-like <see cref="Span{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>The data in a <see cref="Span{T}"/> object.</returns>
    public Span<T> GetData<T>() => _image->GetData<T>();

    /// <summary>
    /// Allows the finalizer to free memory used by this instance.
    /// </summary>
    ~ProcessedImage()
    {
        Dispose(false);
    }
}
