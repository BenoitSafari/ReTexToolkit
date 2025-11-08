using System.ComponentModel.DataAnnotations;

namespace ReTexToolkit.Console.FileSystem;

public enum FileType
{
    [Display(Name = "dds")]
    Dds,

    [Display(Name = "png")]
    Png
}