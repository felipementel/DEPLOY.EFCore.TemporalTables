using DEPLOY.TemporalTables.API.Domain.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEPLOY.TemporalTables.API.Infra.Database.Repositories.v1;

public class ContractEntityConfiguration : IEntityTypeConfiguration<Domain.v1.Contract>
{
    public void Configure(EntityTypeBuilder<Domain.v1.Contract> builder)
    {
        builder
            .ToTable("Contratos_simples", TableBuilder =>
            {
                TableBuilder.HasCheckConstraint("CK_Contratos_simples_DataInicio",
            sql: $"{nameof(Contract.Number)} > 0");
            });

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