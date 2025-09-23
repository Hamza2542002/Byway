using AutoMapper;
using Byway.Core.Dtos;
using Byway.Core.Dtos.Course;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;

namespace Byway.Core.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Instructor, InstructorToReturnDto>();
        CreateMap<InstructorDto, Instructor>()
            .ForMember(des => des.ImageUrl, o => o.Ignore());
        CreateMap<Course, InstructorCourseDto>()
            .ForMember(des => des.CategoryName, o => o.MapFrom(s => s.Category.Name));
        CreateMap<Course, CourseToReturnDto>()
            .ForMember(des => des.LecturesCount, o => o.MapFrom(s => s.Lectures.Count));
    }
}
