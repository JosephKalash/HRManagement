// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;

// // If your appsettings.json is in a different project (like API project)
// namespace HRManagement.Infrastructure.Data
// {
//     public class CrossProjectDbContextFactory : IDesignTimeDbContextFactory<HRDbContext>
//     {
//         public HRDbContext CreateDbContext(string[] args)
//         {
//             // Look for appsettings.json in the API project
//             var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "HRManagement.API");

//             var configuration = new ConfigurationBuilder()
//                 .SetBasePath(basePath)
//                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                 .AddJsonFile("appsettings.Development.json", optional: true)
//                 .Build();

//             var optionsBuilder = new DbContextOptionsBuilder<HRDbContext>();
//             var connectionString = configuration.GetConnectionString("DefaultConnection");

//             optionsBuilder.UseSqlServer(connectionString, b =>
//                 b.MigrationsAssembly("HRManagement.Infrastructure"));

//             return new HRDbContext(optionsBuilder.Options);
//         }
//     }
// }