using AutoMapper;
using ResumeScanApi.Application.States;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class StateProfile : Profile
{
    public StateProfile()
    {
        CreateMap<State, StateDto>();
        CreateMap<CreateStateRequest, State>();
        CreateMap<UpdateStateRequest, State>();
    }
}
