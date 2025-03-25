using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGuarderias.Domain.Entities
{
    public class Nino
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public int GuarderiaId { get; set; }
        public Guarderia Guarderia { get; set; }
        public int TutorId { get; set; }
        public Tutor Tutor { get; set; }
        public List<Asistencia> Asistencias { get; set; }
    }
}
