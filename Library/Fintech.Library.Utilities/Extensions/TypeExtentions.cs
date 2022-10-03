using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Fintech.Library.Utilities.Extensions;

public static class TypeExtentions
{
    public static string GetTableName(this Type type)
    {
        return type.GetCustomAttribute<TableAttribute>()?.Name;
    }
    public static string GetTableSchema(this Type type)
    {
        return type.GetCustomAttribute<TableAttribute>()?.Schema;
    }
}
