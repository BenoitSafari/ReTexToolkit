using System.CommandLine;
using ReTexToolkit.Core.Conversion;
using ReTexToolkit.Core.Conversion.Types;
using ReTexToolkit.Core.Validation;

public sealed class Program
{
    private static readonly Argument<string> InputArg = new("input");
    private static readonly Argument<string> OutputArg = new("output");

    private static readonly Option<string> FuncOpt = new(
        "Function",
        "--func"
    );

    private static readonly string[] ValidFunctions =
    [
        "ddsToPng",
        "ddsToPngPs2"
    ];

    public static async Task Main(string[] args)
    {
        var root = new RootCommand("ReTexToolkit - Texture Conversion Toolkit")
        {
            InputArg,
            OutputArg,
            FuncOpt
        };

        root.SetAction(parseResult =>
        {
            var inputValue = parseResult.GetValue(InputArg);
            if (string.IsNullOrEmpty(inputValue))
                throw new Exception("Input path is required.");

            var outputValue = parseResult.GetValue(OutputArg);
            if (string.IsNullOrEmpty(outputValue))
                throw new Exception("Output path is required.");

            var funcValue = parseResult.GetValue(FuncOpt);
            if (string.IsNullOrEmpty(funcValue))
                throw new Exception("Function is required.");
            if (!ValidFunctions.Contains(funcValue))
                throw new Exception(
                    $"Invalid function '{funcValue}'. Valid functions are: {string.Join(", ", ValidFunctions)}");

            var result = new Result();
            if (funcValue is "ddsToPng")
                result.Try(() => DdsConverter.SaveToPng(inputValue, outputValue));
            if (funcValue is "ddsToPngPs2")
                result.Try(() => DdsConverter.SaveToPng(inputValue, outputValue, ConversionFlag.Ps2));
        });

        var result = root.Parse(args);
        await result.InvokeAsync();
    }
}