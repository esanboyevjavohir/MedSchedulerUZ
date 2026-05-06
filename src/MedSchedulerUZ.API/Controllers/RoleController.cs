using MedSchedulerUZ.Application.Models.ResponseModel;
using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSchedulerUZ.API.Controllers
{
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleModel model)
        {
            var result = await _roleService.CreateAsync(model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleModel model)
        {
            var result = await _roleService.UpdateAsync(id, model);
            if (!result.Succedded)
                return BadRequest(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _roleService.GetByIdAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteAsync(id);
            if (!result.Succedded)
                return NotFound(result.Errors);
            return Ok(result.Result);
        }
    }
}
