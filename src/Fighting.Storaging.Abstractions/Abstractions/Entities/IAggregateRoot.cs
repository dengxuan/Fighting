namespace Fighting.Storaging.Entities.Abstractions
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}
