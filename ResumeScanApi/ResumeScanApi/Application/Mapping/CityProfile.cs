using AutoMapper;
using ResumeScanApi.Application.Cities;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDto>();
        CreateMap<CreateCityRequest, City>();
        CreateMap<UpdateCityRequest, City>();
    }
}
