using AutoMapper;
using CourseLibrary.API.Entities;
using Demo.Web.API.Helpers;
using Demo.Web.API.Models;

namespace Demo.Web.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>().ForMember(dest => dest.Name
            , opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

            CreateMap<AuthorForCreationDto, Author>();
        }
    }
}
