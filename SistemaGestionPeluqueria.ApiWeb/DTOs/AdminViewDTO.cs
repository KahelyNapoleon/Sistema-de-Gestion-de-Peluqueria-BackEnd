namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class AdminViewDTO
    {
        public int AdministradorId { get; set; }

        public string Usuario { get; set; } = null!;

        public string Correo { get; set; } = null!;
    }
}
