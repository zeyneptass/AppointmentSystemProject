using AppointmentSystem_Core.DTOs.Department;
using AppointmentSystem_Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _deparmentService;
        public DepartmentController(IDepartmentService deparmentService)
        {
            _deparmentService = deparmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _deparmentService.GetAllDepartmentAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var result = await _deparmentService.GetDepartmentByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(CreateDepartmentDTO createDepartmentDTO)
        {
            var result = await _deparmentService.AddDepartmentAsync(createDepartmentDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var result = await _deparmentService.DeleteDepartmentAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDTO updateDepartmentDTO)
        {
            var result = await _deparmentService.UpdateDepartmentAsync(updateDepartmentDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
