using System.Reflection;

namespace ReTexToolkit.Core.Extensions;

public static class EnumAttribute
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute =>
        enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
}