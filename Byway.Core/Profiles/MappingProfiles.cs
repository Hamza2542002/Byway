using AutoMapper;
using Byway.Core.Dtos;
using Byway.Core.Dtos.Instructor;
using Byway.Core.Entities;

namespace Byway.Core.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Instructor, InstructorToReturnDto>();
        CreateMap<Course, InstructorCourseDto>()
            .ForMember(des => des.CategoryName,o=> o.MapFrom(s => s.Category.Name));
    }
}
