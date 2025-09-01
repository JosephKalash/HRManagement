using AutoMapper;
using HRManagement.API.Models;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeeSignaturesController(IEmployeeSignatureService employeeSignatureService, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeSignatureService _employeeSignatureService = employeeSignatureService;
        private readonly IMapper _mapper = mapper;





        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeSignatureDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeSignatureDto>>>> GetEmployeeSignatures([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeSignatureService.GetPaged(pageNumber, pageSize);
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var dtos = pagedResult.Items.Select(signature =>
                {
                    var dto = _mapper.Map<EmployeeSignatureDto>(signature);
                    return dto;
                }).ToList();

                return Ok(ApiResponse<PagedResult<EmployeeSignatureDto>>.SuccessResult(new PagedResult<EmployeeSignatureDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee signatures retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeSignatureDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeSignatureDto>>> GetEmployeeSignature(long id)
        {
            try
            {
                var signature = await _employeeSignatureService.GetById(id);
                if (signature == null)
                {
                    return NotFound(ApiResponse<EmployeeSignatureDto>.ErrorResult($"Employee signature with ID {id} not found"));
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                if (!string.IsNullOrEmpty(signature.FilePath))
                {
                    signature.FilePath = baseUrl + '/' + signature.FilePath;
                }

                return Ok(ApiResponse<EmployeeSignatureDto>.SuccessResult(signature, "Employee signature retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeSignatureDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeSignatureDto>>> GetEmployeeSignatureByEmployeeId(long employeeId)
        {
            try
            {
                var signature = await _employeeSignatureService.GetByEmployeeId(employeeId);
                if (signature == null)
                {
                    return NotFound(ApiResponse<EmployeeSignatureDto>.ErrorResult($"Employee signature for employee ID {employeeId} not found"));
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                if (!string.IsNullOrEmpty(signature.FilePath))
                {
                    signature.FilePath = baseUrl + '/' + signature.FilePath;
                }

                return Ok(ApiResponse<EmployeeSignatureDto>.SuccessResult(signature, "Employee signature retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }







        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeSignatureDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<ApiResponse<EmployeeSignatureDto>>> CreateEmployeeSignature([FromForm] CreateEmployeeSignatureRequest signature)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createDto = _mapper.Map<CreateEmployeeSignatureDto>(signature);

                if (createDto == null)
                {
                    return BadRequest(ApiResponse<EmployeeSignatureDto>.ErrorResult("Invalid signature data"));
                }

                using var stream = signature.Image?.OpenReadStream();
                var createdSignature = await _employeeSignatureService.Create(createDto, stream, signature.Image?.FileName);

                return CreatedAtAction(nameof(GetEmployeeSignature), new { id = createdSignature.Id },
                    ApiResponse<EmployeeSignatureDto>.SuccessResult(createdSignature, "Employee signature created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeSignatureDto>.ErrorResult(ex.Message));
            }
        }







        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeSignatureDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeSignatureDto>>> UpdateEmployeeSignature(long id, UpdateEmployeeSignatureDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<EmployeeSignatureDto>.ErrorResult("Validation failed", errors));
                }

                var signature = await _employeeSignatureService.Update(id, updateDto);
                return Ok(ApiResponse<EmployeeSignatureDto>.SuccessResult(signature, "Employee signature updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeSignatureDto>.ErrorResult(ex.Message));
            }
        }







        [HttpPatch("{id}/image")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeSignatureDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeSignatureDto>>> UpdateEmployeeSignatureImage(long id, IFormFile image)
        {
            try
            {
                if (image == null)
                    return BadRequest(ApiResponse<EmployeeSignatureDto>.ErrorResult("Image file is required"));

                using var stream = image.OpenReadStream();
                var signature = await _employeeSignatureService.UpdateSignatureImageAsync(id, stream, image.FileName);
                return Ok(ApiResponse<EmployeeSignatureDto>.SuccessResult(signature, "Employee signature image updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeSignatureDto>.ErrorResult(ex.Message));
            }
        }






        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteEmployeeSignature(long id)
        {
            try
            {
                await _employeeSignatureService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
        }
    }
}
