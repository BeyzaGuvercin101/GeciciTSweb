using Microsoft.AspNetCore.Mvc;

using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;


namespace GeciciTSweb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsolesController : Controller
    {
        private readonly IConsoleService _consoleService;

        public ConsolesController(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consoles = await _consoleService.GetAllAsync();
            return Ok(consoles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var console = await _consoleService.GetByIdAsync(id);
            return Ok(console);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateConsoleDto dto)
        {
            var id = await _consoleService.CreateAsync(dto);
            return Ok(id);
        }
    }
}