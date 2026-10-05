using AutoMapper;
using ResumeScanApi.Application.CustomerEquipmentRegistrations;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Mapping;

public sealed class CustomerEquipmentRegistrationProfile : Profile
{
    public CustomerEquipmentRegistrationProfile()
    {
        CreateMap<CustomerEquipmentRegistration, CustomerEquipmentRegistrationDto>();
        CreateMap<CreateCustomerEquipmentRegistrationRequest, CustomerEquipmentRegistration>();
        CreateMap<UpdateCustomerEquipmentRegistrationRequest, CustomerEquipmentRegistration>();
    }
}
