using DomainLayer.Models;

namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class ActualizarTurnoFechaYHoraDTO
    {
        public DateOnly nuevaFecha { get; set; }
        public TimeOnly nuevaHora { get; set; }
        public EstadoTurno EstadoTurno { get; set; } = null!;
    }
}
