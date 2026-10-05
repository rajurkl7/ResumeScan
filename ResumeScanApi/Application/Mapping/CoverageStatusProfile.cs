using AutoMapper;
using ResumeScanApi.Application.CoverageStatuses;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class CoverageStatusProfile : Profile
{
    public CoverageStatusProfile()
    {
        CreateMap<CoverageStatus, CoverageStatusDto>();
        CreateMap<CreateCoverageStatusRequest, CoverageStatus>();
        CreateMap<UpdateCoverageStatusRequest, CoverageStatus>();
    }
}
