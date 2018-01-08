namespace Fighting.Storage.Abstractions
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}
