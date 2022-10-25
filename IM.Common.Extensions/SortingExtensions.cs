using System.Reflection;
using EFCore.NamingConventions.Internal;
using IM.Common.Extensions.Helpers;
using IM.Common.Models.PagedRequest;

namespace IM.Common.Extensions;

public static class SortingExtensions
{
    private static readonly BindingFlags BindingFlags =
        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

    public static NamingConvention NamingConvention { get; private set; } = NamingConvention.None;

    public static void SetSnakeNamingConvention()
    {
        NamingConvention = NamingConvention.SnakeCase;
    }
    
    public static void SetCamelCaseNamingConvention()
    {
        NamingConvention = NamingConvention.CamelCase;
    }

    public static string GetSorting<T>(this IEnumerable<SortDescriptor> descriptors,
        NamingConvention convention = NamingConvention.None)
    {
        convention = convention is NamingConvention.None ? NamingConvention : convention;

        var sortDescriptors = descriptors as SortDescriptor[] ?? descriptors.ToArray();

        if (sortDescriptors?.Any() != true)
            return string.Empty;

        var orderString = string.Empty;

        var properties = typeof(T).GetProperties(BindingFlags);

        foreach (var sortItem in sortDescriptors)
        {
            var sortItemPropertyName = sortItem.Field.ToLowerInvariant();
            var targetProperty = properties.FirstOrDefault(q => q.Name.ToLowerInvariant().Equals(sortItemPropertyName));

            if (targetProperty == null)
                continue;

            if (sortItem.Order == EnumSortDirection.Desc)
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + targetProperty.GetConventionName(convention) + " DESC"
                    : orderString + ", " + targetProperty.GetConventionName(convention) + " DESC";
            }
            else
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + targetProperty.GetConventionName(convention) + " ASC"
                    : orderString + ", " + targetProperty.GetConventionName(convention) + " ASC";
            }
        }

        return orderString;
    }

    public static string GetSorting<T>(this string sortBy, NamingConvention convention = NamingConvention.CamelCase)
    {
        convention = convention is NamingConvention.None ? NamingConvention : convention;

        var sortItems = sortBy.GetSortItems();

        if (sortItems is null)
            return string.Empty;

        var orderString = string.Empty;

        var properties = typeof(T).GetProperties(BindingFlags);

        foreach (var sortItem in sortItems)
        {
            var sortItemPropertyName = sortItem.PropertyName.ToLowerInvariant();
            var targetProperty = properties.FirstOrDefault(q => q.Name.ToLowerInvariant().Equals(sortItemPropertyName));

            if (targetProperty == null)
                continue;

            if (sortItem.IsDescending)
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + targetProperty.Name.GetConventionName(convention) + " DESC"
                    : orderString + ", " + targetProperty.Name.GetConventionName(convention) + " DESC";
            }
            else
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + targetProperty.GetConventionName(convention) + " ASC"
                    : orderString + ", " + targetProperty.GetConventionName(convention) + " ASC";
            }
        }

        return orderString;
    }

    private static List<SortingElement>? GetSortItems(this string sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return null;

        var sortItems = sortBy.Split(',');

        return (from item in sortItems
                let isDescending = item.Contains('-')
                select new SortingElement { PropertyName = item.Replace("-", ""), IsDescending = isDescending })
            .ToList();
    }

    private class SortingElement
    {
        public string PropertyName { get; init; }
        public bool IsDescending { get; init; }
    }
}
