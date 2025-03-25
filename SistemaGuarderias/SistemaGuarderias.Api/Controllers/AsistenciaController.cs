using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Infrastructure;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly SistemaGuarderiasDbContext _context;

        public AsistenciasController(SistemaGuarderiasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsistenciaDTO>>> GetAll()
        {
            var asistencias = await _context.Asistencias
                .Select(a => new AsistenciaDTO
                {
                    Id = a.Id,
                    NinoId = a.NinoId,
                    GuarderiaId = a.GuarderiaId,
                    Fecha = a.Fecha,
                    Presente = a.Presente
                })
                .ToListAsync();

            if (!asistencias.Any())
                return NotFound("No se encontraron asistencias en la base de datos.");

            return Ok(asistencias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AsistenciaDTO>> GetById(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);

            if (asistencia == null)
                return NotFound($"No se encontró una asistencia con ID {id}.");

            return Ok(new AsistenciaDTO
            {
                Id = asistencia.Id,
                NinoId = asistencia.NinoId,
                GuarderiaId = asistencia.GuarderiaId,
                Fecha = asistencia.Fecha,
                Presente = asistencia.Presente
            });
        }

        [HttpPost]
        public async Task<ActionResult<AsistenciaDTO>> Create(AsistenciaDTO dto)
        {
            if (await _context.Asistencias.AnyAsync(a => a.Id == dto.Id))
                return Conflict($"Ya existe una asistencia con ID {dto.Id}.");

            if (!await _context.Ninos.AnyAsync(n => n.Id == dto.NinoId))
                return NotFound($"No se encontró un niño con ID {dto.NinoId}.");

            if (!await _context.Guarderias.AnyAsync(g => g.Id == dto.GuarderiaId))
                return NotFound($"No se encontró una guardería con ID {dto.GuarderiaId}.");

            var asistencia = new Asistencia
            {
                NinoId = dto.NinoId,
                GuarderiaId = dto.GuarderiaId,
                Fecha = dto.Fecha,
                Presente = dto.Presente
            };

            _context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = asistencia.Id }, new
            {
                Message = "Asistencia agregada correctamente.",
                Asistencia = new AsistenciaDTO
                {
                    Id = asistencia.Id,
                    NinoId = asistencia.NinoId,
                    GuarderiaId = asistencia.GuarderiaId,
                    Fecha = asistencia.Fecha,
                    Presente = asistencia.Presente
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AsistenciaDTO dto)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null)
                return NotFound($"No se encontró una asistencia con ID {id}.");

            if (!await _context.Ninos.AnyAsync(n => n.Id == dto.NinoId))
                return NotFound($"No se encontró un niño con ID {dto.NinoId}.");

            if (!await _context.Guarderias.AnyAsync(g => g.Id == dto.GuarderiaId))
                return NotFound($"No se encontró una guardería con ID {dto.GuarderiaId}.");

            asistencia.Presente = dto.Presente;
            _context.Entry(asistencia).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Asistencia actualizada correctamente.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null)
                return NotFound($"No se encontró una asistencia con ID {id}.");

            _context.Asistencias.Remove(asistencia);
            await _context.SaveChangesAsync();

            return Ok("Asistencia eliminada correctamente.");
        }
    }
}
