using AvesdoSystemDesign.Domain.Entities;
using AvesdoSystemDesign.Infrastructure.Db.Configs;
using Microsoft.EntityFrameworkCore;

namespace AvesdoSystemDesign.Infrastructure.Db;

public class LeadsDbContext(DbContextOptions<LeadsDbContext> options) : DbContext(options)
{
    public DbSet<Lead> Leads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeadsConfiguration).Assembly);
}