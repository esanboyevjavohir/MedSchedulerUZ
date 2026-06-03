using MedSchedulerUZ.Application.Models.SpecializationModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class SpecializationController : ApiController
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateSpecializationModel model)
        {
            var result = await _specializationService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSpecializationModel model)
        {
            var result = await _specializationService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _specializationService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByDepartment(Guid departmentId)
        {
            var result = await _specializationService.GetByDepartmentAsync(departmentId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _specializationService.GetAllAsync();
            return Ok(result);
        }
    }
}
