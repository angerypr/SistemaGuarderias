namespace SistemaGuarderias.Web.Models
{
    public class NinosViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public int GuarderiaId { get; set; }
        public GuarderiasViewModel Guarderia { get; set; }
        public int TutorId { get; set; }
        public TutoresViewModel Tutor { get; set; }
    }
}
