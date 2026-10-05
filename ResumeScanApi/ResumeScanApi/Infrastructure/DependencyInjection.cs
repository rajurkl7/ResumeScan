using Microsoft.Extensions.DependencyInjection;
using ResumeScanApi.Application.CoverageStatuses;
using ResumeScanApi.Application.Cities;
using ResumeScanApi.Application.CustomerEquipmentRegistrations;
using ResumeScanApi.Application.EquipmentModalities;
using ResumeScanApi.Application.FieldServiceEngineers;
using ResumeScanApi.Application.Manufactures;
using ResumeScanApi.Application.Products;
using ResumeScanApi.Application.States;
using ResumeScanApi.Application.UserTypes;
using ResumeScanApi.Infrastructure.Data;
using ResumeScanApi.Infrastructure.Repositories;

namespace ResumeScanApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IManufactureRepository, ManufactureRepository>();
        services.AddScoped<IEquipmentModalityRepository, EquipmentModalityRepository>();
        services.AddScoped<ICoverageStatusRepository, CoverageStatusRepository>();
        services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        services.AddScoped<ICustomerEquipmentRegistrationRepository, CustomerEquipmentRegistrationRepository>();
        services.AddScoped<IFieldServiceEngineerRepository, FieldServiceEngineerRepository>();
        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        return services;
    }
}
