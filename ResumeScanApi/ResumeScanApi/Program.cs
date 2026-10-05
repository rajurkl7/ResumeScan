using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Text;
using ResumeScanApi;
using ResumeScanApi.Application.Mapping;
using ResumeScanApi.Application.Manufactures;
using ResumeScanApi.Application.EquipmentModalities;
using ResumeScanApi.Application.CoverageStatuses;
using ResumeScanApi.Application.Cities;
using ResumeScanApi.Application.CustomerEquipmentRegistrations;
using ResumeScanApi.Application.FieldServiceEngineers;
using ResumeScanApi.Application.Products;
using ResumeScanApi.Application.States;
using ResumeScanApi.Application.UserTypes;
using ResumeScanApi.Authentication;
using ResumeScanApi.Infrastructure;
using ResumeScanApi.Middleware;
using ResumeScanApi.Swagger;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
try
{
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterWeb", policy => policy
        .WithOrigins(
            "http://localhost:8080",
            "http://127.0.0.1:8080",
            "http://localhost:3000",
            "http://127.0.0.1:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services).Enrich.FromLogContext());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Resume Scan API", Version = "v1" });
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "basic",
        In = ParameterLocation.Header, Description = "Enter the configured username and password."
    });
    options.OperationFilter<BasicAuthOperationFilter>();
});
builder.Services.AddAuthentication("Basic")
    .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("Basic", options =>
    {
        builder.Configuration.GetSection(BasicAuthenticationOptions.SectionName).Bind(options);
    });

builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile<ProductProfile>();
    configuration.AddProfile<ManufactureProfile>();
    configuration.AddProfile<EquipmentModalityProfile>();
    configuration.AddProfile<CoverageStatusProfile>();
    configuration.AddProfile<UserTypeProfile>();
    configuration.AddProfile<CustomerEquipmentRegistrationProfile>();
    configuration.AddProfile<FieldServiceEngineerProfile>();
    configuration.AddProfile<StateProfile>();
    configuration.AddProfile<CityProfile>();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateManufactureRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEquipmentModalityRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCoverageStatusRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserTypeRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerEquipmentRegistrationRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateFieldServiceEngineerRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStateRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCityRequestValidator>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IManufactureService, ManufactureService>();
builder.Services.AddScoped<IEquipmentModalityService, EquipmentModalityService>();
builder.Services.AddScoped<ICoverageStatusService, CoverageStatusService>();
builder.Services.AddScoped<IUserTypeService, UserTypeService>();
builder.Services.AddScoped<ICustomerEquipmentRegistrationService, CustomerEquipmentRegistrationService>();
builder.Services.AddScoped<IFieldServiceEngineerService, FieldServiceEngineerService>();
builder.Services.AddScoped<IEngineerDocumentStorage, EngineerDocumentStorage>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors("FlutterWeb");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var credentials = builder.Configuration.GetSection(BasicAuthenticationOptions.SectionName)
        .Get<BasicAuthenticationOptions>();

    if (credentials is not null && !string.IsNullOrWhiteSpace(credentials.Username))
    {
        var encodedCredentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{credentials.Username}:{credentials.Password}"));
        options.UseRequestInterceptor(
            $"(request) => {{ request.headers.Authorization = 'Basic {encodedCredentials}'; return request; }}");
    }
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
