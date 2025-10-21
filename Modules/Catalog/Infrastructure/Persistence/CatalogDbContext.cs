using Microsoft.EntityFrameworkCore;
using Ecommerce.Shared.DataAccess.Contracts;

namespace Ecommerce.Catalog.Infrastructure.Persistence;

public class CatalogDbContext : DbContext
{
    private readonly IDbContextBuilder _contextBuilder;
    private const string ModuleSchema = "catalog";

    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options,
        IDbContextBuilder contextBuilder) : base(options)
    {
        _contextBuilder = contextBuilder;
    }

   // public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _contextBuilder.ConfigureContext(optionsBuilder, ModuleSchema);
        }
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default schema for this module
        modelBuilder.HasDefaultSchema(ModuleSchema);
        
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}