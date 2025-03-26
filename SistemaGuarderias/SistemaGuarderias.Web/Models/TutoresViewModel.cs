using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGuarderias.Web.Models
{
    public class TutoresViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(20, ErrorMessage = "La cédula no puede superar los 20 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string CorreoElectronico { get; set; }

        public List<NinosViewModel> Ninos { get; set; } = new List<NinosViewModel>();
    }
}
