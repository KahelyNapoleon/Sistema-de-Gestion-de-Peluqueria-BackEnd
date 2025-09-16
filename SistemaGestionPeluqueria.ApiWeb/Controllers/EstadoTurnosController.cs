using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using DAL.Data;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using BLL.Services.Interfaces;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstadoTurnosController : Controller
    {
        private readonly IEstadoTurnoService _estadoTurnoService;

        public EstadoTurnosController(IEstadoTurnoService estadoTurnoService)
        {
            _estadoTurnoService = estadoTurnoService;
        }

        [HttpGet]
        [Route("/api/estadosturnos")]
        public async Task<IActionResult> GetEstadosTurnos()
        {
            var estadosTurnos = await _estadoTurnoService.ObtenerTodos(); //?? throw new ArgumentNullException("Aun no hay registros");
            if (estadosTurnos.Success)
            {
                return BadRequest(estadosTurnos.Errors);
            }

            return Ok(estadosTurnos.Data);
        }

        [HttpGet]
        [Route("/api/estadoturno/{id}")]
        public async Task<IActionResult> GetEstadoTurno(int id)
        {
            var estadoTurnoExiste = await _estadoTurnoService.ObtenerPorId(id);
            if (!estadoTurnoExiste.Success)
            {
                return BadRequest(estadoTurnoExiste.Errors);
            }

            return Ok(estadoTurnoExiste.Data);

        }


        [HttpPost]
        [Route("/api/agregar/estadoturno")]
        public async Task<IActionResult> Crear([FromBody] EstadoTurno estadoTurno)
        {

            var nuevoEstado = new EstadoTurno
            {
                Descripcion = estadoTurno.Descripcion
            };

            var crearEstado = await _estadoTurnoService.Crear(nuevoEstado);
            if (!crearEstado.Success)
            {
                return BadRequest(crearEstado.Errors);
            }

            return CreatedAtAction(nameof(GetEstadoTurno), new { id = nuevoEstado.EstadoTurnoId }, estadoTurno);

        }

        //CONTINUAR AQUI HACIA ABAJO....

        [HttpPatch]
        [Route("/api/editar/estadoturno/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] EstadoTurno estadoTurno)
        {
            var actualizarEstadoTurno = new EstadoTurno
            {
                Descripcion = estadoTurno.Descripcion
            };

            var actualizarTurno = await _estadoTurnoService.Actualizar(actualizarEstadoTurno, id);
            if (actualizarTurno.Success)
            {
                return NotFound(actualizarTurno.Errors);
            }

            return CreatedAtAction(nameof(GetEstadoTurno), new {id = actualizarEstadoTurno.EstadoTurnoId}, actualizarEstadoTurno);

        }


        [HttpDelete]
        [Route("/api/eliminar/estadoturno/{id}")]
        public async Task<IActionResult> EliminarEstadoTurno(int id)
        {
            var estadoTurnoEliminar = await _estadoTurnoService.Eliminar(id);
            if (!estadoTurnoEliminar.Success)
            {
                return BadRequest(estadoTurnoEliminar.Errors);
            }

            return Ok(estadoTurnoEliminar.Data);
        }


    }
}
