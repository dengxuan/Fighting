using System;

namespace Fighting.Storage.Abstractions
{
    public abstract class Entity : Entity<Guid>
    {
    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
}
