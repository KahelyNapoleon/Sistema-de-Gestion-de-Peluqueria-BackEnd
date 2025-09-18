using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using BLL.Services.Interfaces;
using SistemaGestionPeluqueria.ApiWeb.DTOs;
using Microsoft.JSInterop.Infrastructure;

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
                EstadoTurnoId = turno.EstadoTurnoId, //En el servicio se guarda como EstadoTurnoId = 2.
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
        public async Task<IActionResult> ActualizarEstadoTurno(int id, [FromBody] EstadoTurno estadoTurno)
        {
            var nuevoEstadoTurno = new EstadoTurno { EstadoTurnoId = estadoTurno.EstadoTurnoId};

            var turnoActualizar = await _turnoService.ActualizarEstadoTurno(id, nuevoEstadoTurno);

            if (!turnoActualizar.Success)
            {
                return BadRequest(turnoActualizar.Errors);
            }

            return CreatedAtAction(nameof(GetTurno), new {id = turnoActualizar.Data!.TurnoId }, turnoActualizar.Data);
        }

        [HttpPatch]
        [Route("/api/actualizar/turno/fechayhora/{id}")]
        public async Task<IActionResult> ActualizarFechaYHora(int id, [FromBody] ActualizarTurnoFechaYHoraDTO dto)
        {
            var actualizarTurno = await _turnoService.ActualizarFechaYHoraTurno(id, dto.nuevaFecha, dto.nuevaHora, dto.EstadoTurno);
            if (!actualizarTurno.Success)
            {
                return BadRequest(actualizarTurno.Errors);
            }
            return CreatedAtAction(nameof(GetTurno), new { id = actualizarTurno.Data!.TurnoId}, actualizarTurno.Data);
        }

        [HttpDelete]
        [Route("/api/eliminar/turno/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminarTurno = await _turnoService.Eliminar(id);
            if (!eliminarTurno.Success)
            {
                return BadRequest(eliminarTurno.Errors);
            }

            return Ok(eliminarTurno.Data);
        }
    }
}
