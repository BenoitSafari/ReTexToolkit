using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace ReTexToolkit.Core.Conversion;

public static class Rgba32Converter
{
    public static bool TryScaleAlpha(this Image img)
    {
        if (img is not Image<Rgba32> image || !IsPs2UnscaledDump(image))
            return false;

        foreach (var memory in image.GetPixelMemoryGroup())
        {
            var span = memory.Span;
            for (var i = 0; i < span.Length; i++)
            {
                var p = span[i];
                p.A = (byte)Math.Min((p.A * 2) - 1, 255);
                span[i] = p;
            }
        }

        return true;
    }

    /// <summary>
    ///     PS2 unscaled dumps have their alpha channel set to 128 for opaque pixels.
    /// </summary>
    /// <param name="img">
    ///     <see cref="Image" />
    /// </param>
    public static bool IsPs2UnscaledDump(Image<Rgba32> img)
    {
        foreach (var memory in img.GetPixelMemoryGroup())
            for (var i = 0; i < memory.Span.Length; i++)
                if (memory.Span[i].A <= 128)
                    return true;

        return false;
    }
}