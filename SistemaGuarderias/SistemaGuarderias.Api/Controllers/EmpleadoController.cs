using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Infrastructure;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly SistemaGuarderiasDbContext _context;

        public EmpleadosController(SistemaGuarderiasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDTO>>> GetAll()
        {
            var empleados = await _context.Empleados
                .Select(e => new EmpleadoDTO
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    Cedula = e.Cedula,
                    Cargo = e.Cargo,
                    Telefono = e.Telefono,
                    CorreoElectronico = e.CorreoElectronico,
                    GuarderiaId = e.GuarderiaId
                })
                .ToListAsync();

            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDTO>> GetById(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }

            var dto = new EmpleadoDTO
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Cedula = empleado.Cedula,
                Cargo = empleado.Cargo,
                Telefono = empleado.Telefono,
                CorreoElectronico = empleado.CorreoElectronico,
                GuarderiaId = empleado.GuarderiaId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDTO>> Create(EmpleadoDTO dto)
        {
            var empleado = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Cedula = dto.Cedula,
                Cargo = dto.Cargo,
                Telefono = dto.Telefono,
                CorreoElectronico = dto.CorreoElectronico,
                GuarderiaId = dto.GuarderiaId
            };

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            var empleadoDTO = new EmpleadoDTO
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Cedula = empleado.Cedula,
                Cargo = empleado.Cargo,
                Telefono = empleado.Telefono,
                CorreoElectronico = empleado.CorreoElectronico,
                GuarderiaId = empleado.GuarderiaId
            };

            return CreatedAtAction(nameof(GetById), new { id = empleado.Id }, empleadoDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmpleadoDTO dto)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            empleado.Nombre = dto.Nombre;
            empleado.Apellido = dto.Apellido;
            empleado.Cedula = dto.Cedula;
            empleado.Cargo = dto.Cargo;
            empleado.Telefono = dto.Telefono;
            empleado.CorreoElectronico = dto.CorreoElectronico;
            empleado.GuarderiaId = dto.GuarderiaId;

            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
