namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class TipoServicioDTO
    {
        public int TipoServicioId { get; set; }
        public string Descripcion { get; set; } = null!;

        public List<ServicioDTO> Servicios { get; set; } = new List<ServicioDTO>();
    }
}
