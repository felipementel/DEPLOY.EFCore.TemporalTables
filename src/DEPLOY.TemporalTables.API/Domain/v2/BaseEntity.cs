namespace DEPLOY.TemporalTables.API.Domain.v2;

public abstract class BaseEntity<Tid>
{
    protected BaseEntity(Tid id)
    {
        Id = id;
    }

    public Tid Id { get; init; }
}