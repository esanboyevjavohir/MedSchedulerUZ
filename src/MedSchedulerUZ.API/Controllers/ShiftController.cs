using MedSchedulerUZ.Application.Models.ShiftModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class ShiftController : ApiController
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShiftModel model)
        {
            var result = await _shiftService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShiftModel model)
        {
            var result = await _shiftService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _shiftService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _shiftService.GetAllAsync();
            return Ok(result.Result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var result = await _shiftService.GetByUserIdAsync(userId);
            return Ok(result.Result);
        }

        [HttpGet("schedule/{scheduleId}")]
        public async Task<IActionResult> GetBySchedule(Guid scheduleId)
        {
            var result = await _shiftService.GetByScheduleIdAsync(scheduleId);
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _shiftService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }
    }
}
