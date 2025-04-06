using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.TemporalTables.API.Infra.Database.Repositories.v1;

public class PersonEntityConfiguration : IEntityTypeConfiguration<Domain.v1.Person>
{
    public void Configure(EntityTypeBuilder<Domain.v1.Person> builder)
    {
        builder
            .ToTable("Pessoas_simples");

        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("PessoaId")
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Email)
            .IsRequired();

        builder.Property(p => p.Phone);

        builder.Property(p => p.Document);

        builder.Property(p => p.Address);

        builder.Property(p => p.BornDate)
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("datetime")
            .IsRequired();
    }
}