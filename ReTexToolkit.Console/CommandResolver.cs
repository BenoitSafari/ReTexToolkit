using System.CommandLine;
using ReTexToolkit.Console.FileSystem;
using ReTexToolkit.Core.Conversion;

namespace ReTexToolkit.Console;

public static class CommandResolver
{
    private static readonly string[] ValidFunctions =
    [
        "dds-to-png",
        "ps2-alpha-scale"
        // "ps2-alpha-unscale"
    ];

    // TODO: Add logging
    public static void Resolve(ParseResult parseResult)
    {
        var inputValue = parseResult.GetValue(Arguments.InputArg);
        if (string.IsNullOrEmpty(inputValue))
            throw new Exception("Input path is required.");

        var funcValue = parseResult.GetValue(Arguments.FuncOpt);
        if (string.IsNullOrEmpty(funcValue))
            throw new Exception("Function is required.");
        if (!ValidFunctions.Contains(funcValue))
            throw new Exception(
                $"Invalid function '{funcValue}'. Valid functions are: {string.Join(", ", ValidFunctions)}"
            );

        if (funcValue is "dds-to-png")
            foreach (var fileName in FileResolver.ResolveFilesName(inputValue, FileType.Dds))
                Converter.ToPng(fileName, inputValue, FileResolver.ResolveOutputPath(inputValue, "out_png"));
        if (funcValue is "ps2-alpha-scale")
            foreach (var fileName in FileResolver.ResolveFilesName(inputValue, FileType.Png))
                Ps2Alpha.ScaleAlpha(
                    fileName,
                    inputValue,
                    FileResolver.ResolveOutputPath(inputValue, "out_scaled")
                );
    }
}