using MedSchedulerUZ.Application.Models.HospitalModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class HospitalController : ApiController
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHospitalModel model)
        {
            var result = await _hospitalService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHospitalModel model)
        {
            var result = await _hospitalService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _hospitalService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hospitalService.GetAllAsync();
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _hospitalService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }
    }
}
