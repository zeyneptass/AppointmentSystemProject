using AppointmentSystem_Core.DTOs.Patient;
using AppointmentSystem_Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _patientService.GetAllPatientsAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var result = await _patientService.GetPatientByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> Update(PatientUpdateDTO dto)
        {
            var result = await _patientService.UpdatePatientAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _patientService.DeletePatientAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
