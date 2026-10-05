using AutoMapper;
using ResumeScanApi.Application.EquipmentModalities;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class EquipmentModalityProfile : Profile
{
    public EquipmentModalityProfile()
    {
        CreateMap<EquipmentModality, EquipmentModalityDto>();
        CreateMap<CreateEquipmentModalityRequest, EquipmentModality>();
        CreateMap<UpdateEquipmentModalityRequest, EquipmentModality>();
    }
}
