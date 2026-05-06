using MedSchedulerUZ.Application.Models.ScheduleModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleModel model)
        {
            var result = await _scheduleService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScheduleModel model)
        {
            var result = await _scheduleService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _scheduleService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _scheduleService.GetAllAsync();
            return Ok(result.Result);
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(Guid departmentId)
        {
            var result = await _scheduleService.GetByDepartmentIdAsync(departmentId);
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _scheduleService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }
    }
}
