using Byway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Byway.Persestance.Configuration;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable(nameof(Instructor));
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(i => i.ImageUrl)
            .IsRequired();
        builder.Property(i => i.Rate)
            .IsRequired();
        builder.Property(i => i.JobTitle)
            .IsRequired();

        builder.HasMany(i => i.Courses)
            .WithOne(c => c.Instructor)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
