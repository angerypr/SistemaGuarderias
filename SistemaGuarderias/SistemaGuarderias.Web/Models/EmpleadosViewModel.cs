namespace SistemaGuarderias.Web.Models
{
    public class EmpleadosViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public int GuarderiaId { get; set; }
        public GuarderiasViewModel Guarderia { get; set; }
    }
}
