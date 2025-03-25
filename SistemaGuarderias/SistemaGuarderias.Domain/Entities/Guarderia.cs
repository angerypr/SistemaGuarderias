using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGuarderias.Domain.Entities
{
    public class Guarderia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Nino> Ninos { get; set; }
        public List<Asistencia> Asistencias { get; set; }
    }
}
