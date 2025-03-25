using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGuarderias.Domain.Entities
{
    public class Asistencia
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int NinoId { get; set; }
        public Nino Nino { get; set; }
        public int GuarderiaId { get; set; }  
        public Guarderia Guarderia { get; set; } 
        public bool Presente { get; set; }
    }
}
