using HRManagement.Application.Interfaces;
using AspNetCoreWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace HRManagement.Infrastructure.Services
{
    public class WebHostEnvironmentAdapter(AspNetCoreWebHostEnvironment webHostEnvironment) : IWebHostEnvironment
    {
        private readonly AspNetCoreWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public string WebRootPath => _webHostEnvironment.WebRootPath;
    }
} 