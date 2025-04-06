namespace DEPLOY.TemporalTables.API.Domain.v1;

public class Contract : BaseEntity<Guid>
{
    public Contract(Guid id,
        DateTime createdAt,
        DateTime updatedAt,
        long number,
        DateOnly startDate,
        DateOnly endDate,
        bool ative)
        : base(id, createdAt, updatedAt)
    {
        Number = number;
        StartDate = startDate;
        EndDate = endDate;
        Active = ative;
    }

    public Contract(Guid id)
        : base(id)
    {

    }

    public long Number { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public bool Active { get; init; }
}