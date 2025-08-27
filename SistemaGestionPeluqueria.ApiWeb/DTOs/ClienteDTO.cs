namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class ClienteDTO
    {
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string NroCelular { get; set; } = null!;

        public string? CorreoElectronico { get; set; }

        public DateOnly? FechaNacimiento { get; set; }
    }
}
