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

    private static readonly Dictionary<string, (string outDir, FileType type)> FunctionMappings =
        new()
        {
            { "dds-to-png", ("out_png", FileType.Dds) },
            { "ps2-alpha-scale", ("out_scaled", FileType.Png) }
            // { "ps2-alpha-unscale", ("out_unscaled", FileType.Png) }
        };

    private static readonly SemaphoreSlim Throttler = new(Environment.ProcessorCount);

    public static async Task ResolveAsync(ParseResult parseResult)
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

        var count = 0;
        var files = FileResolver.ResolveFilesName(inputValue, FunctionMappings[funcValue].type);
        var total = files.Length;

        var tasks = FileResolver.ResolveFilesName(inputValue, FunctionMappings[funcValue].type)
            .Select(async fileName =>
            {
                await Throttler.WaitAsync();
                try
                {
                    var output = string.Empty;

                    if (funcValue is "dds-to-png")
                        output = await Converter.ToPngAsync(
                            fileName,
                            inputValue,
                            FileResolver.ResolveOutputPath(inputValue, "out_png")
                        );
                    if (funcValue is "ps2-alpha-scale")
                        output = await Ps2Alpha.ScaleAsync(
                            fileName,
                            inputValue,
                            FileResolver.ResolveOutputPath(inputValue, "out_scaled")
                        );

                    var done = Interlocked.Increment(ref count);
                    System.Console.WriteLine($"[{done}/{total}] SAVED: {Path.GetFileName(output)}");
                }
                finally
                {
                    Throttler.Release();
                }
            });

        await Task.WhenAll(tasks);
    }
}