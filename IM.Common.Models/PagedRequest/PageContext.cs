using System.Runtime.Serialization;

namespace IM.Common.Models.PagedRequest;

[DataContract]
public class PageContext : IPageContext
{
    public PageContext(
        int pageIndex,
        int pageSize,
        IEnumerable<SortDescriptor>? listSort = null)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        ListSort = listSort?.ToList() ?? Enumerable.Empty<SortDescriptor>().ToList();
    }
    
    public PageContext()
    {
        ListSort = Enumerable.Empty<SortDescriptor>().ToList();
    }

    [DataMember(Order = 1)] public int PageIndex { get; set; }
    [DataMember(Order = 2)] public int PageSize { get; set; }
    [DataMember(Order = 3)] public List<SortDescriptor> ListSort { get; set; }
}

[DataContract]
public class PageContext<T> : IPageContext<T>
    where T : class, new()
{
    public PageContext()
    {
        Filter = new T();
        ListSort = Enumerable.Empty<SortDescriptor>().ToList();
    }
    
    public PageContext(
        int pageIndex,
        int pageSize,
        IEnumerable<SortDescriptor>? listSort = null,
        T? filter = null)
    {
        Filter = filter ?? new T();
        ListSort = listSort?.ToList() ?? Enumerable.Empty<SortDescriptor>().ToList();
    }

    [DataMember(Order = 1)] public int PageIndex { get; set; }
    [DataMember(Order = 2)] public int PageSize { get; set; }
    [DataMember(Order = 3)] public List<SortDescriptor>? ListSort { get; set; }
    [DataMember(Order = 4)] public T? Filter { get; set; }

    public bool IsValid()
    {
        return PageIndex > 0 && PageSize > 0
                             && Filter != null && ListSort != null;
    }
}
