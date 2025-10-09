namespace Byway.Core.Dtos.Category;

public class CategoryToReturnDto
{
    public Guid Id { get; set; }
    public int CourseCount { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
}
