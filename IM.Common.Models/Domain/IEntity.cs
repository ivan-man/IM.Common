using System.ComponentModel.DataAnnotations;

namespace IM.Common.Models.Domain;

public interface IEntity<TId> : IEntity
{
    [Key] TId Id { get; set; }
}

public interface IEntity
{
    DateTime Created { get; set; }

    DateTime? Updated { get; set; }
}
