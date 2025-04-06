using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.TemporalTables.API.Infra.Database.Repositories.v2;

public class ContractEntityConfiguration : IEntityTypeConfiguration<Domain.v2.Contract>
{
    public void Configure(EntityTypeBuilder<Domain.v2.Contract> builder)
    {
        builder
            .ToTable("Contratos_versionados", t => t.IsTemporal(true));

        builder
            .HasKey(x => x.Id);

        builder
           .Property(x => x.Id)
           .HasColumnName("ContratoId")
           .ValueGeneratedNever();

        builder
          .Property(x => x.Number)
          .HasColumnName("Numero")
          .HasColumnType("bigint")
          .IsRequired();

        builder
           .Property(x => x.StartDate)
          .HasColumnName("DataInicio")
          .HasColumnType("date")
          .IsRequired();

        builder
          .Property(x => x.EndDate)
          .HasColumnName("DataFim")
          .HasColumnType("date")
          .IsRequired();

        builder
          .Property(x => x.Active)
          .HasColumnName("Ativo")
          .HasColumnType("bit")
          .IsRequired();
    }
}