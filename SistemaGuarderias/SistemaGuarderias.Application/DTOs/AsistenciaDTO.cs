namespace SistemaGuarderias.Application.DTOs
{
    public class AsistenciaDTO
    {
        public int Id { get; set; }
        public int NinoId { get; set; }
        public int GuarderiaId { get; set; }
        public DateTime Fecha { get; set; }
        public bool Presente { get; set; }
    }
}