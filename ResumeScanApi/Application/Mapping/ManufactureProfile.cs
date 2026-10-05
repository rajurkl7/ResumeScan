using AutoMapper;
using ResumeScanApi.Application.Manufactures;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class ManufactureProfile : Profile
{
    public ManufactureProfile()
    {
        CreateMap<Manufacture, ManufactureDto>();
        CreateMap<CreateManufactureRequest, Manufacture>();
        CreateMap<UpdateManufactureRequest, Manufacture>();
    }
}
