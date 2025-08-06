using System.Text.Json.Serialization;
using HRManagement.API;
using HRManagement.API.Middleware;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;

// Use extension methods for configuration
builder.AddSwaggerAndApiVersioning();
// builder.Services.Configure<JsonOptions>(options =>
//         {
//             options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
//         });

builder.AddCorsPolicy();
builder.AddFormOptions();
builder.AddDbContextAndRepositories();
builder.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Management API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseGlobalExceptionHandler();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HRManagement.Infrastructure.Data.HRDbContext>();
    context.Database.EnsureCreated();
}

app.Run();