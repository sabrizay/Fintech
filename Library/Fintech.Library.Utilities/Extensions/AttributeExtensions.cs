using Fintech.Library.Entities.Attributes;
using System.Reflection;

namespace Fintech.Library.Utilities.Extensions;

public static class AttributeExtensions
{
    //public static string GetCosmosDbConfigAttributePartitionKey(this Type type)
    //{
    //    try
    //    {
    //        return (type.GetCustomAttributes()
    //                 .FirstOrDefault(a => a.GetType() == typeof(CosmosDbConfigAttribute))
    //                 as CosmosDbConfigAttribute).PartitionKey;
    //    }
    //    catch { }
    //    return "";
    //}
    public static string GetCosmosDbConfigAttributeContainerName(this Type type)
    {
        try
        {
            return (type.GetCustomAttributes()
                     .FirstOrDefault(a => a.GetType() == typeof(CosmosDbConfigAttribute))
                     as CosmosDbConfigAttribute).ContainerName;
        }
        catch { }
        return "";
    }
}
