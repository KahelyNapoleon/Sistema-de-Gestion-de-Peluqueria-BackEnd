using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using BLL.Services.Interfaces;
using SistemaGestionPeluqueria.ApiWeb.DTOs;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class TurnoController : Controller
    {
        private readonly ITurnoService _turnoService;

        public TurnoController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }
        [HttpGet]
        [Route("/api/turnos")]
        public async Task<IActionResult> GetTurnos()
        {
            var turnos = await _turnoService.ObtenerTodos();
            if (!turnos.Success)
            {
                return BadRequest(turnos.Errors);
            }

            return Ok(turnos.Data);
        }

        [HttpGet]
        [Route("/api/turno/{id}")]
        public async Task<IActionResult> GetTurno(int id)
        {
            var turno = await _turnoService.ObtenerPorId(id);
            if (!turno.Success)
            {
                return NotFound(turno.Errors);
            }

            return Ok(turno.Data);
        }

        [HttpPost]
        [Route("/api/crear/turno")]
        public async Task<IActionResult> AgregarTurno([FromBody] TurnoCreateDTO turno)
        {
            var agregarTurno = new Turno
            {
                Detalle = turno.Detalle,
                ServicioId = turno.ServicioId,
                ClienteId = turno.ClienteId,
                EstadoTurnoId = turno.EstadoTurnoId,
                MontoTotal = turno.MontoTotal,
                MetodoPagoId = turno.MetodoPagoId,
                HoraTurno = turno.HoraTurno,
                FechaTurno = turno.FechaTurno
            };

            var validarTurno = await _turnoService.Crear(agregarTurno);
            if (!validarTurno.Success)
            {
                return BadRequest(validarTurno.Errors);
            }

            return Ok(validarTurno.Data);
        }

        [HttpPatch]
        [Route("/api/actualizar/turno/{id}")]
        public async ActualizarTurno(int id)
        {

        }
    }
}
