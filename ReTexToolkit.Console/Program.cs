using System.CommandLine;
using ReTexToolkit.Console;
using ReTexToolkit.Core.Validation;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var root = new RootCommand("ReTexToolkit - Texture Conversion Toolkit")
        {
            Arguments.InputArg,
            Arguments.FuncOpt
        };

        root.SetAction(parseResult =>
        {
            var result = new Result();
            result.Try(() => CommandResolver.Resolve(parseResult));
            if (!result.HasError())
                return;

            foreach (var error in result.Errors)
                Console.Error.WriteLine(error.Message);
        });

        var result = root.Parse(args);
        await result.InvokeAsync();
    }
}