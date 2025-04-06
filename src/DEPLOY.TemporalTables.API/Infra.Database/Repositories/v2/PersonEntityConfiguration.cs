using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.TemporalTables.API.Infra.Database.Repositories.v2;

public class PersonEntityConfiguration : IEntityTypeConfiguration<Domain.v2.Person>
{
    public void Configure(EntityTypeBuilder<Domain.v2.Person> builder)
    {
        builder
            .ToTable("Pessoas_versionados", t => t
                .IsTemporal(p => p
                    .UseHistoryTable("HistoricoTabelaPessoa")
                    .HasPeriodEnd("fim")
                    .HasColumnName("column-fim")));

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
    }
}