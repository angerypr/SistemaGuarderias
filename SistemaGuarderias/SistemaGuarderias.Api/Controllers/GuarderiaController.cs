using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Infrastructure;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuarderiasController : ControllerBase
    {
        private readonly SistemaGuarderiasDbContext _context;

        public GuarderiasController(SistemaGuarderiasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuarderiaDTO>>> GetAll()
        {
            var guarderias = await _context.Guarderias
                .Select(g => new GuarderiaDTO
                {
                    Id = g.Id,
                    Nombre = g.Nombre,
                    Direccion = g.Direccion
                })
                .ToListAsync();

            return Ok(guarderias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuarderiaDTO>> GetById(int id)
        {
            var guarderia = await _context.Guarderias.FindAsync(id);
            if (guarderia == null)
            {
                return NotFound();
            }

            var dto = new GuarderiaDTO
            {
                Id = guarderia.Id,
                Nombre = guarderia.Nombre,
                Direccion = guarderia.Direccion
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<GuarderiaDTO>> Create(GuarderiaDTO dto)
        {
            var guarderia = new Guarderia
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion
            };

            _context.Guarderias.Add(guarderia);
            await _context.SaveChangesAsync();

            var createdDto = new GuarderiaDTO
            {
                Id = guarderia.Id,
                Nombre = guarderia.Nombre,
                Direccion = guarderia.Direccion
            };

            return CreatedAtAction(nameof(GetById), new { id = guarderia.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GuarderiaDTO dto)
        {
            var guarderia = await _context.Guarderias.FindAsync(id);
            if (guarderia == null)
            {
                return NotFound();
            }

            guarderia.Nombre = dto.Nombre;
            guarderia.Direccion = dto.Direccion;

            _context.Entry(guarderia).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var guarderia = await _context.Guarderias.FindAsync(id);
            if (guarderia == null)
            {
                return NotFound();
            }

            _context.Guarderias.Remove(guarderia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
