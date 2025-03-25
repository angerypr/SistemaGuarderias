using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGuarderias.Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public int GuarderiaId { get; set; }
        public Guarderia Guarderia { get; set; }
    }
}
