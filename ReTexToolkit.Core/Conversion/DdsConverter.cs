using Pfim;
using ReTexToolkit.Core.Conversion.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace ReTexToolkit.Core.Conversion;

public static class DdsConverter
{
    public static void SaveToPng(string inputPath, string outputPath, ConversionFlag? flag = null)
    {
        using var dds = Pfimage.FromFile(inputPath);
        var image = ToImage(dds);

        if (flag is ConversionFlag.Ps2)
            image.TryScaleAlpha();

        image.Save(outputPath, new PngEncoder());
    }

    public static Image ToImage(IImage dds) =>
        dds.Format switch
        {
            ImageFormat.Rgb8 => FromRgb8(dds),
            ImageFormat.R5g5b5 => FromR5g5b5(dds),
            ImageFormat.R5g6b5 => FromR5g6b5(dds),
            ImageFormat.R5g5b5a1 => FromR5g5b5a1(dds),
            ImageFormat.Rgba16 => FromRgba16(dds),
            ImageFormat.Rgb24 => FromRgb24(dds),
            ImageFormat.Rgba32 => FromRgba32(dds),
            _ => throw new NotSupportedException($"Unsupported format: {dds.Format}")
        };

    private static Image<Rgba32> FromRgb8(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + x;
            var v = data[idx];
            img[x, y] = new Rgba32(v, v, v, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g5b5(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 10) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x1F) * 255 / 31);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g6b5(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 11) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x3F) * 255 / 63);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g5b5a1(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 10) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x1F) * 255 / 31);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            var a = (byte)(((pixel >> 15) & 0x01) * 255);
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }

    private static Image<Rgba32> FromRgba16(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 12) & 0x0F) * 17);
            var g = (byte)(((pixel >> 8) & 0x0F) * 17);
            var b = (byte)(((pixel >> 4) & 0x0F) * 17);
            var a = (byte)((pixel & 0x0F) * 17);
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }

    private static Image<Rgba32> FromRgb24(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 3);
            var r = data[idx + 0];
            var g = data[idx + 1];
            var b = data[idx + 2];
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromRgba32(IImage dds)
    {
        var img = new Image<Rgba32>(dds.Width, dds.Height);
        var data = dds.Data;
        for (var y = 0; y < dds.Height; y++)
        for (var x = 0; x < dds.Width; x++)
        {
            var idx = (y * dds.Stride) + (x * 4);
            var r = data[idx + 0];
            var g = data[idx + 1];
            var b = data[idx + 2];
            var a = data[idx + 3];
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }
}