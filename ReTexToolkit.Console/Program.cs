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

        root.SetAction(async parseResult =>
        {
            var result = new Result();
            await result.TryAsync(async () => await CommandResolver.ResolveAsync(parseResult));
            if (!result.HasError())
                return;

            foreach (var error in result.Errors)
                await Console.Error.WriteLineAsync(error.Message);
        });

        var result = root.Parse(args);
        await result.InvokeAsync();
    }
}