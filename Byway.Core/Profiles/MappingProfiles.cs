using AutoMapper;
using Byway.Core.Auth;
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
        CreateMap<Course, CourseListToReturnDto>();
        CreateMap<Instructor, CourseInstructorDto>();

        CreateMap<Course, CourseToReturnDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CourseDto, Course>()
            .ForMember(des => des.Lectures , o=> o.MapFrom(s => s.Lectures))
            .ForMember(des => des.ImageUrl, o => o.Ignore());
        CreateMap<CourseLectureDto, CourseLecture>();
        CreateMap<CourseLecture,CourseLectureToReturnDto>();

        CreateMap<ApplicationUser, UserDto>();
    }
}
