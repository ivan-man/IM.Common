namespace IM.Common.Models.Domain;

public class BaseEntity<TId> : BaseEntity, IEntity<TId>
{
    public TId Id { get; set; }
}

public class BaseEntity : IEntity
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}
