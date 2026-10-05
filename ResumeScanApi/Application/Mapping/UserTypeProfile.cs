using AutoMapper;
using ResumeScanApi.Application.UserTypes;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class UserTypeProfile : Profile
{
    public UserTypeProfile()
    {
        CreateMap<UserType, UserTypeDto>()
            .ForMember(destination => destination.UserType, options => options.MapFrom(source => source.UserTypeName));
        CreateMap<CreateUserTypeRequest, UserType>()
            .ForMember(destination => destination.UserTypeName, options => options.MapFrom(source => source.UserType));
        CreateMap<UpdateUserTypeRequest, UserType>()
            .ForMember(destination => destination.UserTypeName, options => options.MapFrom(source => source.UserType));
    }
}
