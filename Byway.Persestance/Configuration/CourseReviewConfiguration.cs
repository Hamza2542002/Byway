using Byway.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Byway.Persestance.Configuration;

internal class CourseReviewConfiguration : IEntityTypeConfiguration<CourseReview>
{
    public void Configure(EntityTypeBuilder<CourseReview> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Message).IsRequired();

        builder.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId);
        builder.HasOne(e => e.Course)
            .WithMany()
            .HasForeignKey(e => e.CourseId);

    }
}
