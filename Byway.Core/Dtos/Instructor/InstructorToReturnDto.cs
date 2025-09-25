namespace Byway.Core.Dtos.Instructor;

public class InstructorToReturnDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double Rate { get; set; }
    public string? JobTitle { get; set; }
    public int StudentsNumber { get; set; }
}
