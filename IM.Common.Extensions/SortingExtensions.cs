using System.Reflection;

namespace IM.Common.Extensions;

public static class SortingExtensions
{
    public static string GetSorting<T>(this string sortBy) where T : new()
    {
        var sortItems = sortBy.GetSortItems();

        if (sortItems is null)
            return string.Empty;

        var orderString = string.Empty;
        foreach (var sortItem in sortItems)
        {
            var emptyInstance = new T();

            foreach (var propertyInfo in emptyInstance.GetType().GetProperties())
            {
                try
                {
                    propertyInfo.SetValue(emptyInstance, Activator.CreateInstance(propertyInfo.PropertyType));
                }
                catch
                {
                    //ignore
                }
            }

            GetValue(emptyInstance, sortItem.PropertyName.ToLower(), out var name);
            if (string.IsNullOrEmpty(name)) return string.Empty;

            if (sortItem.IsDescending)
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + name + " DESC"
                    : orderString + ", " + name + " DESC";
            }
            else
            {
                orderString = string.IsNullOrEmpty(orderString)
                    ? orderString + name + " ASC"
                    : orderString + ", " + name + " ASC";
            }
        }

        return orderString;
    }

    private static void GetValue(object currentObject, string propName, out string? value)
    {
        // call helper function that keeps track of which objects we've seen before
        GetValue(currentObject, propName, out value, new HashSet<object>());
    }

    private static string GetValue(object currentObject, string propName, out string? value,
        ISet<object> searchedObjects, string parentName = "")
    {
        var type = currentObject.GetType();
        var propInfo = type.GetProperty(propName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propInfo != null)
        {
            value = !string.IsNullOrEmpty(parentName) ? parentName + "." + propInfo.Name : propInfo.Name;
            return !string.IsNullOrEmpty(parentName) ? parentName + "." + propInfo.Name : propInfo.Name;
        }

        // search child properties
        foreach (var propInfo2 in currentObject.GetType()
                     .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance))
        {
            // ignore indexed properties
            if (propInfo2.GetIndexParameters().Length != 0) continue;
            var newObject = propInfo2.GetValue(currentObject, null);
            if (newObject != null && searchedObjects.Add(newObject) &&
                GetValue(newObject, propName, out value, searchedObjects, propInfo2.Name) != string.Empty)
                return !string.IsNullOrEmpty(parentName) ? parentName + "." + propInfo2.Name : propInfo2.Name;
        }

        // property not found here
        value = null;
        return string.Empty;
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
