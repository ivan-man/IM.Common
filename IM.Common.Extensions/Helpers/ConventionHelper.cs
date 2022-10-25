using System.Reflection;
using System.Text.RegularExpressions;
using EFCore.NamingConventions.Internal;

namespace IM.Common.Extensions.Helpers;

public static class ConventionHelper
{
    public static string GetConventionName(this string propertyName, NamingConvention convention)
    {
        switch (convention)
        {
            case NamingConvention.None:
            case NamingConvention.CamelCase:
                return propertyName;
            
            case NamingConvention.SnakeCase:
                return propertyName.ToSnakeCase();
            
            case NamingConvention.UpperCase:
                return propertyName.ToUpperInvariant();
            
            case NamingConvention.LowerCase:
                return propertyName.ToLowerInvariant();
            
            case NamingConvention.UpperSnakeCase:
                return propertyName.ToSnakeCase().ToUpperInvariant();
                
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public static string GetConventionName(this PropertyInfo propertyInfo, NamingConvention convention)
    {
        switch (convention)
        {
            case NamingConvention.None:
            case NamingConvention.CamelCase:
                return propertyInfo.Name;
            
            case NamingConvention.SnakeCase:
                return propertyInfo.Name.ToSnakeCase();
            
            case NamingConvention.UpperCase:
                return propertyInfo.Name.ToUpperInvariant();
            
            case NamingConvention.LowerCase:
                return propertyInfo.Name.ToLowerInvariant();
            
            case NamingConvention.UpperSnakeCase:
                return propertyInfo.Name.ToSnakeCase().ToUpperInvariant();
                
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static string ToSnakeCase(this string text)
    {
        text = Regex.Replace(text, "(.)([A-Z][a-z]+)", "$1_$2");
        text = Regex.Replace(text, "([a-z0-9])([A-Z])", "$1_$2");
        return text.ToLowerInvariant();
    }
}
