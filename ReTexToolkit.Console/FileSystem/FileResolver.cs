using System.ComponentModel.DataAnnotations;
using ReTexToolkit.Core.Extensions;

namespace ReTexToolkit.Console.FileSystem;

public static class FileResolver
{
    public static string[] ResolveFilesName(string dir, FileType type)
    {
        if (!Directory.Exists(dir))
            throw new DirectoryNotFoundException(dir);

        var ext = type.GetAttribute<DisplayAttribute>()?.Name;
        if (ext is null)
            throw new Exception("Invalid file type.");

        var files = Directory
            .GetFiles(dir)
            .Select(Path.GetFileName)
            .Where(x => x is not null && x.EndsWith($".{ext}"))
            .ToArray() ?? [];

        if (files is null || files.Length is 0)
            throw new FileNotFoundException($"No files of type '{ext}' were found in directory '{dir}'.");

        return files!;
    }

    public static string ResolveOutputPath(string inputPath, string dirName)
    {
        var outputPath = Path.Combine(inputPath, dirName);
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        return outputPath;
    }
}