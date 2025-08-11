using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeciciTSweb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [AllowAnonymous] // Test için geçici
        public async Task<IActionResult> GetAll([FromQuery] int? consoleId = null)
        {
            if (consoleId.HasValue)
            {
                var units = await _unitService.GetByConsoleIdAsync(consoleId.Value);
                return Ok(units);
            }
            else
            {
                var units = await _unitService.GetAllAsync();
                return Ok(units);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _unitService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateUnitDto dto) => Ok(await _unitService.CreateAsync(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _unitService.DeleteAsync(id));
    }
}

