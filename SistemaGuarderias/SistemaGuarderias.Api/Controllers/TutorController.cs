using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Infrastructure;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutoresController : ControllerBase
    {
        private readonly SistemaGuarderiasDbContext _context;

        public TutoresController(SistemaGuarderiasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDTO>>> GetAll()
        {
            var tutores = await _context.Tutores
                .Select(t => new TutorDTO
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Apellido = t.Apellido,
                    Telefono = t.Telefono,
                    Cedula = t.Cedula,
                    CorreoElectronico = t.CorreoElectronico
                })
                .ToListAsync();

            if (tutores.Count == 0)
            {
                return NotFound(new { mensaje = "No se encontraron tutores en la base de datos." });
            }

            return Ok(tutores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorDTO>> GetById(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null)
            {
                return NotFound(new { mensaje = $"No se encontró un tutor con el ID {id}." });
            }

            var dto = new TutorDTO
            {
                Id = tutor.Id,
                Nombre = tutor.Nombre,
                Apellido = tutor.Apellido,
                Telefono = tutor.Telefono,
                Cedula = tutor.Cedula,
                CorreoElectronico = tutor.CorreoElectronico
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TutorDTO>> Create(TutorDTO dto)
        {
            if (await _context.Tutores.AnyAsync(t => t.Cedula == dto.Cedula))
            {
                return BadRequest(new { mensaje = "Ya existe un tutor con esta cédula." });
            }

            var tutor = new Tutor
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono,
                Cedula = dto.Cedula,
                CorreoElectronico = dto.CorreoElectronico
            };

            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();

            var createdDto = new TutorDTO
            {
                Id = tutor.Id,
                Nombre = tutor.Nombre,
                Apellido = tutor.Apellido,
                Telefono = tutor.Telefono,
                Cedula = tutor.Cedula,
                CorreoElectronico = tutor.CorreoElectronico
            };

            return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TutorDTO dto)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null)
            {
                return NotFound(new { mensaje = $"No se encontró un tutor con el ID {id}." });
            }

            if (await _context.Tutores.AnyAsync(t => t.Id != id && t.Cedula == dto.Cedula))
            {
                return BadRequest(new { mensaje = "Ya existe otro tutor con esta cédula." });
            }

            tutor.Nombre = dto.Nombre;
            tutor.Apellido = dto.Apellido;
            tutor.Telefono = dto.Telefono;
            tutor.Cedula = dto.Cedula;
            tutor.CorreoElectronico = dto.CorreoElectronico;

            _context.Entry(tutor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null)
            {
                return NotFound(new { mensaje = $"No se encontró un tutor con el ID {id}." });
            }

            _context.Tutores.Remove(tutor);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tutor eliminado correctamente." });
        }
    }
}