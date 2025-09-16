namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class TipoServicioDTO
    {
        public int TipoServicioId { get; set; }
        public string Descripcion { get; set; } = null!;

        public List<ServicioViewDTO> Servicios { get; set; } = new List<ServicioViewDTO>();
    }
}
