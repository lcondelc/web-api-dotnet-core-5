using AutoMapper;
using CourseLibrary.API.Entities;
using Demo.Web.API.Models;

namespace Demo.Web.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseDto>();
        }
    }
}
