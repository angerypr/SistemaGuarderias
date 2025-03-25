namespace SistemaGuarderias.Application.DTOs
{
    public class NinoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public int GuarderiaId { get; set; }
        public int TutorId { get; set; }
    }
}
