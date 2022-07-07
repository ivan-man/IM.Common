namespace IM.Common.Models.Domain;

public class BaseEntity<TId> : BaseEntity, IBaseEntity<TId>
{
    public TId Id { get; set; }
}

public class BaseEntity : IBaseEntity
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}
