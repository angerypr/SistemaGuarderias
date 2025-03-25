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

            return Ok(tutores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorDTO>> GetById(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
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
                return NotFound();
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
                return NotFound();
            }

            _context.Tutores.Remove(tutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
