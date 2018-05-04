using Fighting.Timing;
using System;

namespace Fighting.Storaging.Entities.Abstractions
{
    public abstract class Entity : Entity<int>, IEntity<int>, IEntity
    {
    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public Entity()
        {
            CreationTime = Clock.Now;
        }
        public virtual TPrimaryKey Id { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}
