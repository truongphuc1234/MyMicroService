using AutoMapper;
using User.API.DTOs;
using User.API.Entities;

namespace Api.Mapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<UserSignUpDto, ApplicationUser>();
        CreateMap<UserProfileDto, UserProfile>().ReverseMap();
    }
}