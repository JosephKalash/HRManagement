using System.Reflection;
using System.Runtime.InteropServices;
using HRManagement.Application.Interfaces;
using HRManagement.Application.Services;
using HRManagement.Core.Entities;
using HRManagement.Core.enums;
using HRManagement.Core.Enums;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Repositories;
using HRManagement.Infrastructure.Data;
using HRManagement.Infrastructure.Repositories;
using HRManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace HRManagement.API;

public static class ProgramConfigExtensions
{
    public static WebApplicationBuilder AddSwaggerAndApiVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HR Management API",
                Version = "v1",
                Description = "A comprehensive HR management system API",
            });
            MapEnum<RoleLevel>(c);
            MapEnum<AssignmentType>(c);
            MapEnum<SocialCondition>(c);
            MapEnum<BloodGroup>(c);
            MapEnum<Religions>(c);
            MapEnum<OrgUnitType>(c);
            MapEnum<MilitaryRank>(c);
            MapEnum<Gender>(c);
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
            options.MultipartBodyLengthLimit = 5 * 1024 * 1024; // 5MB
        });
        return builder;
    }

    public static WebApplicationBuilder AddDbContextAndRepositories(this WebApplicationBuilder builder)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            builder.Services.AddDbContext<HRDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("mac.DefaultConnection")));
        }
        else
        {
            builder.Services.AddDbContext<HRDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("win.DefaultConnection")));
        }
        // Register repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeProfileRepository, EmployeeProfileRepository>();
        builder.Services.AddScoped<IEmployeeServiceInfoRepository, EmployeeServiceInfoRepository>();
        builder.Services.AddScoped<IEmployeeAssignmentRepository, EmployeeAssignmentRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        builder.Services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
        builder.Services.AddScoped<IEmployeeContactRepository, EmployeeContactRepository>();
        builder.Services.AddScoped<IEmployeeSignatureRepository, EmployeeSignatureRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IEmployeeProfileService, EmployeeProfileService>();
        builder.Services.AddScoped<IEmployeeContactService, EmployeeContactService>();
        builder.Services.AddScoped<IEmployeeServiceInfoService, EmployeeServiceInfoService>();
        builder.Services.AddScoped<IEmployeeAssignmentService, EmployeeAssignmentService>();
        builder.Services.AddScoped<IOrgUnitService, OrgUnitService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IEmployeeSignatureService, EmployeeSignatureService>();

        // Add AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Register the web host environment adapter
        _ = builder.Services.AddScoped<Application.Interfaces.IWebHostEnvironment, WebHostEnvironmentAdapter>();

        // Current user service
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Output caching (ASP.NET Core 8)
        builder.Services.AddOutputCache(options =>
        {
            options.AddPolicy("EmployeesPaged", b => b
                .Expire(TimeSpan.FromSeconds(60))
                .SetVaryByQuery("pageNumber", "pageSize"));
            options.AddPolicy("EmployeeById", b => b
                .Expire(TimeSpan.FromMinutes(2))
                .SetVaryByRouteValue("id"));
            options.AddPolicy("ActiveEmployees", b => b
                .Expire(TimeSpan.FromSeconds(60)));
            options.AddPolicy("SearchEmployees", b => b
                .Expire(TimeSpan.FromSeconds(60))
                .SetVaryByQuery("searchTerm"));
            options.AddPolicy("EmployeeDetails", b => b
                .Expire(TimeSpan.FromMinutes(2))
                .SetVaryByRouteValue("id"));
            options.AddPolicy("EmployeeJobSummary", b => b
                .Expire(TimeSpan.FromSeconds(60))
                .SetVaryByRouteValue("employeeId"));
            // New policies
            options.AddPolicy("CurrentEmployee", b => b
                .Expire(TimeSpan.FromSeconds(30))
                .SetCacheKeyPrefix("me")
                .SetVaryByHeader("Authorization"));
            options.AddPolicy("OrgHierarchy", b => b
                .Expire(TimeSpan.FromMinutes(5)));
            options.AddPolicy("RolesPaged", b => b
                .Expire(TimeSpan.FromMinutes(2))
                .SetVaryByQuery("pageNumber", "pageSize"));
        });

        return builder;
    }

    static void MapEnum<T>(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
    {
        c.MapType<T>(() => new OpenApiSchema
        {
            Type = "string",
            Enum = [.. Enum.GetNames(typeof(T))
                    .Select(name => new OpenApiString(name))
                    .Cast<IOpenApiAny>()]
        });
    }
}