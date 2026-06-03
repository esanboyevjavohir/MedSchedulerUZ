using MedSchedulerUZ.Application.Models.DepartmentModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentModel model)
        {
            var result = await _departmentService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentModel model)
        {
            var result = await _departmentService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("hospital/{hospitalId}")]
        [Authorize(Roles = "Employee,DeptHead,HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByHospital(Guid hospitalId)
        {
            var result = await _departmentService.GetByHospitalIdAsync(hospitalId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "HospitalAdmin,SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result);
        }
    }
}
