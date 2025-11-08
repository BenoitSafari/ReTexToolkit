using System.CommandLine;

namespace ReTexToolkit.Console;

public static class Arguments
{
    public static readonly Argument<string> InputArg = new("input");
    public static readonly Option<string> FuncOpt = new("Function", "--func");
}