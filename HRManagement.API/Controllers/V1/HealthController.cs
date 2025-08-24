using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        
        
        
        
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public ActionResult<ApiResponse<object>> GetHealth()
        {
            var healthInfo = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            };

            return Ok(ApiResponse<object>.SuccessResult(healthInfo, "API is running"));
        }
    }
}