using Microsoft.EntityFrameworkCore;

namespace DEPLOY.TemporalTables.API.Infra.Database.Persistence;

public class DeployDbContext : DbContext
{
    public DeployDbContext(DbContextOptions<DeployDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.v1.Person> Person { get; set; }

    public DbSet<Domain.v1.Contract> Contact { get; set; }

    public DbSet<Domain.v2.Person> Person_Versioned { get; set; }

    public DbSet<Domain.v2.Contract> Contract_Versioned { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Repositories.v1.PersonEntityConfiguration());
        modelBuilder.ApplyConfiguration(new Repositories.v1.ContractEntityConfiguration());

        modelBuilder.ApplyConfiguration(new Repositories.v2.PersonEntityConfiguration());
        modelBuilder.ApplyConfiguration(new Repositories.v2.ContractEntityConfiguration());

        MapearPropriedadesEsquecidas(modelBuilder);

        modelBuilder.HasServiceTier("Basic");
        modelBuilder.HasPerformanceLevel("Basic");
    }

    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
    {
        foreach (var item in modelBuilder.Model.GetEntityTypes())
        {
            var prop = item.GetProperties().Where(c => c.ClrType == typeof(string));

            foreach (var itemProp in prop)
            {
                if (string.IsNullOrEmpty(itemProp.GetColumnType())
                    && !itemProp.GetMaxLength().HasValue)
                {
                    //itemProp.SetMaxLength(100);
                    itemProp.SetColumnType("VARCHAR(100)");
                }
            }
        }
    }
}

//public class EntityConfigurationv1<T> : IEntityTypeConfiguration<T> where T 
//    : DEPLOY.TemporalTables.API.Domain.v1.BaseEntity<T>, new()
//{
//    public virtual void Configure(EntityTypeBuilder<T> builder)
//    {
//        //Table-per-hierarchy Tph
//        //Table-per-type Tpt
//        //Table-per-concrete-type Tpc
//        //https://learn.microsoft.com/en-us/ef/core/modeling/inheritance

//        builder.UseTpcMappingStrategy();
//    }
//}

//public class EntityConfigurationv2<T> : IEntityTypeConfiguration<T> where T
//    : DEPLOY.TemporalTables.API.Domain.v2.BaseEntity<T>, new()
//{
//    public virtual void Configure(EntityTypeBuilder<T> builder)
//    {
//        //Table-per-hierarchy Tph
//        //Table-per-type Tpt
//        //Table-per-concrete-type Tpc
//        //https://learn.microsoft.com/en-us/ef/core/modeling/inheritance

//        builder.UseTpcMappingStrategy();
//    }
//}