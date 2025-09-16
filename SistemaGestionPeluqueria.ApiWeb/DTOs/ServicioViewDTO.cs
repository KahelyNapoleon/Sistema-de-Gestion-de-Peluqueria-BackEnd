using DomainLayer.Models;

namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class ServicioViewDTO
    {
        public int ServicioId { get; set; }

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Duracion { get; set; }

        public string? Observacion { get; set; }
       
    }
}
