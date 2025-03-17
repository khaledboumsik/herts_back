using Invoicer.Repositories;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>()
            .HasMany(p => p.Invoices)
            .WithOne(i => i.Provider)
            .HasForeignKey(i => i.ProviderId);
        modelBuilder.Entity<Invoice>()
       .Property(i => i.DateDeposite)
       .HasDefaultValue(null); // Explicitly set default value to null
    }
}