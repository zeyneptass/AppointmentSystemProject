using AppointmentSystem_Core.DTOs.Doctor;
using AppointmentSystem_Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpPost("add-doctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorByAdminDTO addDoctorByAdminDto)
        {
            var result = await _doctorService.AddDoctorAsync(addDoctorByAdminDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _doctorService.GetAllDoctorsAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var result = await _doctorService.GetDoctorByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorDTO updateDoctorDto)
        {
            var result = await _doctorService.UpdateDoctorAsync(updateDoctorDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
