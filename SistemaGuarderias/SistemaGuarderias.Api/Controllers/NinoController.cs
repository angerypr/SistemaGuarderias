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

            return Ok(ninos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NinoDTO>> GetById(int id)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound();
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

            return CreatedAtAction(nameof(GetById), new { id = nino.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NinoDTO dto)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound();
            }

            nino.Nombre = dto.Nombre;
            nino.Apellido = dto.Apellido;
            nino.Edad = dto.Edad;
            nino.GuarderiaId = dto.GuarderiaId;
            nino.TutorId = dto.TutorId;

            _context.Entry(nino).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nino = await _context.Ninos.FindAsync(id);
            if (nino == null)
            {
                return NotFound();
            }

            _context.Ninos.Remove(nino);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
