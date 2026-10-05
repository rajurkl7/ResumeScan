using AutoMapper;
using ResumeScanApi.Application.FieldServiceEngineers;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class FieldServiceEngineerProfile : Profile
{
    public FieldServiceEngineerProfile()
    {
        CreateMap<FieldServiceEngineer, FieldServiceEngineerDto>();
        CreateMap<FieldServiceEngineerCapability, EngineerCapabilityDto>();
        CreateMap<FieldServiceEngineerState, EngineerStateDto>();
        CreateMap<FieldServiceEngineerCity, EngineerCityDto>();
        CreateMap<FieldServiceEngineerDocument, EngineerDocumentDto>();
        CreateMap<EngineerCapabilityRequest, FieldServiceEngineerCapability>();
        CreateMap<EngineerStateRequest, FieldServiceEngineerState>();
        CreateMap<EngineerCityRequest, FieldServiceEngineerCity>();
        CreateMap<EngineerDocumentRequest, FieldServiceEngineerDocument>();
        CreateMap<CreateFieldServiceEngineerRequest, FieldServiceEngineer>()
            .ForMember(x => x.PasswordHash, options => options.Ignore())
            .ForMember(x => x.Capabilities, options => options.MapFrom(x => x.Capabilities))
            .ForMember(x => x.OperatingStates, options => options.MapFrom(x => x.OperatingStates))
            .ForMember(x => x.OperatingCities, options => options.MapFrom(x => x.OperatingCities))
            .ForMember(x => x.Documents, options => options.MapFrom(x => x.Documents));
        CreateMap<UpdateFieldServiceEngineerRequest, FieldServiceEngineer>()
            .ForMember(x => x.PasswordHash, options => options.Ignore())
            .ForMember(x => x.Capabilities, options => options.MapFrom(x => x.Capabilities))
            .ForMember(x => x.OperatingStates, options => options.MapFrom(x => x.OperatingStates))
            .ForMember(x => x.OperatingCities, options => options.MapFrom(x => x.OperatingCities))
            .ForMember(x => x.Documents, options => options.MapFrom(x => x.Documents));

        //test
    }
}
