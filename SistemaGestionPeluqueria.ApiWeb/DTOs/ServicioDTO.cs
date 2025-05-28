using System.ComponentModel.DataAnnotations;

namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class ServicioDTO
    {
        public int ServicioId { get; set; }
        [Required]
        public string Descripcion { get; set; } = null!;
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Duracion { get; set; }

        public string? Observacion { get; set; }
        [Required]
        public int TipoServicioId { get; set; }
    }
}
