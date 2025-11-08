using Pfim;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace ReTexToolkit.Core.Conversion;

public static class Converter
{
    public static void ToPng(string fileName, string inputDir, string outDir)
    {
        var inputPath = Path.Combine(inputDir, fileName);
        var outputPath = Path.Combine(outDir, $"{Path.GetFileNameWithoutExtension(fileName)}.png");

        using var pfImg = Pfimage.FromFile(inputPath);
        ToImage(pfImg).Save(outputPath, new PngEncoder());
    }

    public static void ToDds(string inputPath, string outputPath) => throw new NotImplementedException();

    public static Image ToImage(IImage input) =>
        input.Format switch
        {
            ImageFormat.Rgb8 => FromRgb8(input),
            ImageFormat.R5g5b5 => FromR5g5b5(input),
            ImageFormat.R5g6b5 => FromR5g6b5(input),
            ImageFormat.R5g5b5a1 => FromR5g5b5a1(input),
            ImageFormat.Rgba16 => FromRgba16(input),
            ImageFormat.Rgb24 => FromRgb24(input),
            ImageFormat.Rgba32 => FromRgba32(input),
            _ => throw new NotSupportedException($"Unsupported format: {input.Format}")
        };

    private static Image<Rgba32> FromRgb8(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + x;
            var v = data[idx];
            img[x, y] = new Rgba32(v, v, v, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g5b5(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 10) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x1F) * 255 / 31);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g6b5(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 11) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x3F) * 255 / 63);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromR5g5b5a1(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 10) & 0x1F) * 255 / 31);
            var g = (byte)(((pixel >> 5) & 0x1F) * 255 / 31);
            var b = (byte)((pixel & 0x1F) * 255 / 31);
            var a = (byte)(((pixel >> 15) & 0x01) * 255);
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }

    private static Image<Rgba32> FromRgba16(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 2);
            var pixel = BitConverter.ToUInt16(data, idx);
            var r = (byte)(((pixel >> 12) & 0x0F) * 17);
            var g = (byte)(((pixel >> 8) & 0x0F) * 17);
            var b = (byte)(((pixel >> 4) & 0x0F) * 17);
            var a = (byte)((pixel & 0x0F) * 17);
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }

    private static Image<Rgba32> FromRgb24(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 3);
            var r = data[idx + 0];
            var g = data[idx + 1];
            var b = data[idx + 2];
            img[x, y] = new Rgba32(r, g, b, 255);
        }

        return img;
    }

    private static Image<Rgba32> FromRgba32(IImage input)
    {
        var img = new Image<Rgba32>(input.Width, input.Height);
        var data = input.Data;
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
        {
            var idx = (y * input.Stride) + (x * 4);
            var r = data[idx + 0];
            var g = data[idx + 1];
            var b = data[idx + 2];
            var a = data[idx + 3];
            img[x, y] = new Rgba32(r, g, b, a);
        }

        return img;
    }
}