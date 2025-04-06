namespace DEPLOY.TemporalTables.API.Domain.v2;

public class Contract : BaseEntity<Guid>
{
    public Contract(Guid id,
        long number,
        DateOnly startDate,
        DateOnly endDate,
        bool active)
        : base(id)
    {
        Number = number;
        StartDate = startDate;
        EndDate = endDate;
        Active = active;
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