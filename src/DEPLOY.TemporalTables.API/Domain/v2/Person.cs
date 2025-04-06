namespace DEPLOY.TemporalTables.API.Domain.v2;

public class Person : BaseEntity<Guid>
{
    public Person(Guid id,
        string name,
        string email,
        string phone,
        string document,
        string address,
        DateOnly bornDate)
        : base(id)
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

    public void AddContract(Contract contract)
    {
        Contracts.Add(contract);
    }
}
