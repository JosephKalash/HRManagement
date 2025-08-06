using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using HRManagement.Infrastructure.Data;
using HRManagement.Application.Interfaces;
using HRManagement.Application.Services;
using HRManagement.Core.Interfaces;
using HRManagement.Infrastructure.Repositories;

namespace HRManagement.API;

public static class ProgramConfigExtensions
{
    public static WebApplicationBuilder AddSwaggerAndApiVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HR Management API",
                Version = "v1",
                Description = "A comprehensive HR management system API",
                Contact = new OpenApiContact
                {
                    Name = "HR Management Team",
                    Email = "hr@company.com"
                }
            });
            // Include XML comments for better documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return builder;
    }

    public static WebApplicationBuilder AddJsonOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
        return builder;
    }

    public static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        return builder;
    }

    public static WebApplicationBuilder AddFormOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
        });
        return builder;
    }

    public static WebApplicationBuilder AddDbContextAndRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<HRDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Register repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeProfileRepository, EmployeeProfileRepository>();
        builder.Services.AddScoped<IEmployeeServiceInfoRepository, EmployeeServiceInfoRepository>();
        builder.Services.AddScoped<IEmployeeAssignmentRepository, EmployeeAssignmentRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        builder.Services.AddScoped<IPerformanceReviewRepository, PerformanceReviewRepository>();
        builder.Services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IOrgUnitService, OrgUnitService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        return builder;
    }
}