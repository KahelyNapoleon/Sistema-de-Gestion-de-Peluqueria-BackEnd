namespace SistemaGestionPeluqueria.ApiWeb.DTOs
{
    public class AdminCreateDTO
    {
        public string Usuario { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;
    }
}
