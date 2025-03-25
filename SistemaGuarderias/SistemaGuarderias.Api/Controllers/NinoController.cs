using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Infrastructure;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NinosController : ControllerBase
    {
        private readonly SistemaGuarderiasDbContext _context;

        public NinosController(SistemaGuarderiasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NinoDTO>>> GetAll()
        {
            var ninos = await _context.Ninos
                .Select(n => new NinoDTO
                {
                    Id = n.Id,
                    Nombre = n.Nombre,
                    Apellido = n.Apellido,
                    Edad = n.Edad,
                    GuarderiaId = n.GuarderiaId,
                    TutorId = n.TutorId
                })
                .ToListAsync();

            if (ninos.Count == 0)
            {
                return NotFound(new { mensaje = "No se encontraron niños en la base de datos." });
            }

            return Ok(ninos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NinoDTO>> GetById(int id)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound(new { mensaje = $"No se encontró un niño con el ID {id}." });
            }

            var dto = new NinoDTO
            {
                Id = nino.Id,
                Nombre = nino.Nombre,
                Apellido = nino.Apellido,
                Edad = nino.Edad,
                GuarderiaId = nino.GuarderiaId,
                TutorId = nino.TutorId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<NinoDTO>> Create(NinoDTO dto)
        {
            if (await _context.Ninos.AnyAsync(n => n.Id == dto.Id))
            {
                return BadRequest(new { mensaje = "El ID del niño ya existe en la base de datos." });
            }

            if (!await _context.Guarderias.AnyAsync(g => g.Id == dto.GuarderiaId))
            {
                return BadRequest(new { mensaje = "No se encontró una guardería con el ID especificado." });
            }

            if (!await _context.Tutores.AnyAsync(t => t.Id == dto.TutorId))
            {
                return BadRequest(new { mensaje = "No se encontró un tutor con el ID especificado." });
            }

            var nino = new Nino
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Edad = dto.Edad,
                GuarderiaId = dto.GuarderiaId,
                TutorId = dto.TutorId
            };

            _context.Ninos.Add(nino);
            await _context.SaveChangesAsync();

            var createdDto = new NinoDTO
            {
                Id = nino.Id,
                Nombre = nino.Nombre,
                Apellido = nino.Apellido,
                Edad = nino.Edad,
                GuarderiaId = nino.GuarderiaId,
                TutorId = nino.TutorId
            };

            return CreatedAtAction(nameof(GetById), new { id = nino.Id }, new { mensaje = "Niño agregado correctamente.", nino });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NinoDTO dto)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound(new { mensaje = $"No se encontró un niño con el ID {id}." });
            }

            if (!await _context.Guarderias.AnyAsync(g => g.Id == dto.GuarderiaId))
            {
                return BadRequest(new { mensaje = "No se encontró una guardería con el ID especificado." });
            }

            if (!await _context.Tutores.AnyAsync(t => t.Id == dto.TutorId))
            {
                return BadRequest(new { mensaje = "No se encontró un tutor con el ID especificado." });
            }

            nino.Nombre = dto.Nombre;
            nino.Apellido = dto.Apellido;
            nino.Edad = dto.Edad;
            nino.GuarderiaId = dto.GuarderiaId;
            nino.TutorId = dto.TutorId;

            _context.Entry(nino).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Niño actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound(new { mensaje = $"No se encontró un niño con el ID {id}." });
            }

            _context.Ninos.Remove(nino);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Niño eliminado correctamente." });
        }
    }
}
