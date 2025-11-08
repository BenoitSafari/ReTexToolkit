using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace ReTexToolkit.Core.Conversion;

public static class Ps2Alpha
{
    public static void ScaleAlpha(string fileName, string inputDir, string outDir)
    {
        var inputPath = Path.Combine(inputDir, fileName);
        var outputPath = Path.Combine(outDir, fileName);

        using var img = Image.Load(inputPath);
        if (img.TryScaleAlpha())
            img.Save(outputPath);
        else
            img.Save(Path.GetFileNameWithoutExtension(fileName) + "_alpha-unchanged" + Path.GetExtension(fileName));
    }

    public static void UnScaleAlpha(string fileName, string inputDir, string outDir)
    {
        var inputPath = Path.Combine(inputDir, fileName);
        var outputPath = Path.Combine(outDir, fileName);

        using var img = Image.Load(inputPath);
        if (img.TryUnScaleAlpha())
            img.Save(outputPath);
        else
            img.Save(Path.GetFileNameWithoutExtension(fileName) + "_alpha-unchanged" + Path.GetExtension(fileName));
    }

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

    public static bool TryUnScaleAlpha(this Image img) => throw new NotImplementedException();

    public static bool IsPs2UnscaledDump(Image<Rgba32> img)
    {
        foreach (var memory in img.GetPixelMemoryGroup())
            for (var i = 0; i < memory.Span.Length; i++)
                if (memory.Span[i].A <= 128)
                    return true;

        return false;
    }
}