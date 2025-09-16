namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class TurnoCreateDTO
    {
        public string? Detalle { get; set; }

        public int ServicioId { get; set; }

        public int ClienteId { get; set; }

        public int EstadoTurnoId { get; set; }

        public int MontoTotal { get; set; }

        public int MetodoPagoId { get; set; }

        public int AdministradorId { get; set; }

        public TimeOnly HoraTurno { get; set; }

        public DateOnly FechaTurno { get; set; }
    }
}
