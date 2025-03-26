using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Web.Models
{
    public class GuarderiasViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Nino> Ninos { get; set; }
        public List<Asistencia> Asistencias { get; set; }
    }
}
