using AvesdoSystemDesign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvesdoSystemDesign.Infrastructure.Db.Configs;

public class LeadsConfiguration: IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(l => l.Email).IsUnique();

        builder.Property(l => l.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(l => l.LastName)
            .IsRequired()
            .HasMaxLength(50);
    }
}