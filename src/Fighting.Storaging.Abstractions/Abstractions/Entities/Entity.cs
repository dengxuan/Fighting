using System;

namespace Fighting.Storaging.Entities.Abstractions
{
    public abstract class Entity : Entity<int>
    {
    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
}
