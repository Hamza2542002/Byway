using Byway.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace Byway.Persestance.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
{
    public ApplicationDbContext()
    {
        
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {

    }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseLecture> CourseLectures { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CourseLecture>().ToTable(nameof(CourseLecture));
        builder.Entity<Category>().ToTable(nameof(Category));

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
