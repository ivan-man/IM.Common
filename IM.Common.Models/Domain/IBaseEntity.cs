using System.ComponentModel.DataAnnotations;

namespace IM.Common.Models.Domain;

public interface IBaseEntity<TId> : IBaseEntity
{
    [Key] TId Id { get; set; }
}

public interface IBaseEntity
{
    DateTime Created { get; set; }

    DateTime? Updated { get; set; }
}
