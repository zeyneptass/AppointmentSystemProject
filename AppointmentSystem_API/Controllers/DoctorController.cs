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
    }
}
