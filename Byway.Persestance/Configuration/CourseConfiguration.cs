using Byway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Byway.Persestance.Configuration;

internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(c => c.Description)
            .IsRequired();
        builder.Property(c => c.Cost)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(c => c.TotalHours)
            .IsRequired();
        builder.Property(c => c.Rate)
            .IsRequired();
        builder.Property(c => c.Certification)
            .IsRequired();
        builder.Property(c => c.Level)
            .IsRequired();
        builder.Property(c => c.ImageUrl)
            .IsRequired(false);
        
        builder.HasMany(c => c.Lectures)
            .WithOne()
            .HasForeignKey(cl => cl.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
            
    }
}
