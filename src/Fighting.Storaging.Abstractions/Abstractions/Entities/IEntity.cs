using System;

namespace Fighting.Storaging.Entities.Abstractions
{
    public interface IEntity : IEntity<Guid>
    {
    }

    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
