namespace DEPLOY.TemporalTables.API.Domain.v1;

public class Person : BaseEntity<Guid>
{
    public Person(Guid id,
        DateTime createdAt,
        DateTime updatedAt,
string name,
        string email,
        string phone,
        string document,
        string address,
        DateOnly bornDate)
        : base(id, createdAt, updatedAt)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Document = document;
        Address = address;
        BornDate = bornDate;
    }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Phone { get; init; }

    public string Document { get; init; }

    public string Address { get; init; }

    public DateOnly BornDate { get; init; }

    public List<Contract> Contracts { get; set; }
}
