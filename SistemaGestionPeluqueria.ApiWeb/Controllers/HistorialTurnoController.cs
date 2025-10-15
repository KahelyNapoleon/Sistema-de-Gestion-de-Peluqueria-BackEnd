using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using BLL.Services.Interfaces;


namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HistorialTurnoController : Controller
    {
        private readonly IHistorialTurnoService _historialTurnoService;

        public HistorialTurnoController(IHistorialTurnoService historialTurnoService)
        {
            _historialTurnoService = historialTurnoService;
        }

        [HttpGet]
        [Route("/api/historialturnos")]
        public async Task<IActionResult> GetHistorialturnos()
        {
            var historialTurnos = await _historialTurnoService.ObtenerTodos();
            if (!historialTurnos.Success)
            {
                return BadRequest(historialTurnos.Errors);
            }

            return Ok(historialTurnos.Data);
        }

        [HttpGet]
        [Route("/api/historialturno/{id}")]
        public async Task<IActionResult> GetHistorialTurno(int id)
        {
            var historialTurno = await _historialTurnoService.ObtenerPorId(id);
            if (historialTurno.Success)
            {
                return BadRequest(historialTurno.Errors);
            }

            return Ok(historialTurno.Data);
        }
    }
}
