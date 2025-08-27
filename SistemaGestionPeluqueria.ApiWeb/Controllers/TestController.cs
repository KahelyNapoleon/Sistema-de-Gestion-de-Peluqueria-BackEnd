using Microsoft.AspNetCore.Mvc;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new InvalidOperationException("Excepcion de prueba.");
        }
    }
}
