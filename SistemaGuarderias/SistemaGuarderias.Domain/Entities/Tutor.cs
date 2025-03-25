using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGuarderias.Domain.Entities
{
    public class Tutor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string CorreoElectronico { get; set; }
        public List<Nino> Ninos { get; set; }
    }
}
