using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using DAL.Data;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstadoTurnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEstadoTurnoRepository _IEstadoTurnoRepositorio;

        public EstadoTurnosController(ApplicationDbContext context, IEstadoTurnoRepository IEstadoTurnoRepositorio)
        {
            _context = context;
            _IEstadoTurnoRepositorio = IEstadoTurnoRepositorio;
        }

        [HttpGet]
        [Route("/estadosturnos")]
        public async Task<IActionResult> GetEstadosTurno()
        {
            var estadosTurnos = await _IEstadoTurnoRepositorio.GetAllAsync(); //?? throw new ArgumentNullException("Aun no hay registros");
            if (estadosTurnos == null || !estadosTurnos.Any())
            {
                return Ok("Aun no hay Estados de turnos Registrados");
            }

            return Ok(estadosTurnos);
        }

        [HttpGet]
        [Route("/estadoturno/{id}")]
        public async Task<IActionResult> GetEstadoTurno(int id)
        {
            var estadoTurnoExiste = await _IEstadoTurnoRepositorio.VerificarSiExiste(id);
            if (!estadoTurnoExiste)
            {
                return BadRequest($"No existe un registro de Estado con id={id}");
            }

            try
            {
                var estadoTurno = await _IEstadoTurnoRepositorio.GetByIdAsync(id);
                return Ok(estadoTurno);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("/agregar/estadoturno")]
        public async Task<IActionResult> AgregarEstadoTurno([FromBody]EstadoTurno estadoTurno)
        {
            try
            {
                var nuevoEstado = new EstadoTurno
                {
                    Descripcion = estadoTurno.Descripcion
                };

                await _IEstadoTurnoRepositorio.AddAsync(nuevoEstado);

                return Ok(nuevoEstado);
            }
            catch(Exception ex)
            {
                return BadRequest($"Algo salio mal :(: {ex.Message}");
            }
        }


        [HttpPatch]
        [Route("/editar/estadoturno/{id}")]
        public async Task<IActionResult> EditarEstadoTurno(int id, [FromBody] EstadoTurno estadoTurno)
        {
           
            if (id != estadoTurno.EstadoTurnoId)
            {
                return BadRequest("El id ingresado no pertenece a un registro existente");
            }

            try
            {
                await _IEstadoTurnoRepositorio.UpdateAsync(estadoTurno);

                return NoContent();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;

                return BadRequest($"Algo salio mal {message}");

            }
        }


        [HttpDelete]
        [Route("/eliminar/estadoturno/{id}")]
        public async Task<IActionResult> EliminarEstadoTurno(int id)
        {
            //CHEQUEAR ESTA PARTE YA QUE UN TURNO DEBERIA SER UNA DESCRIPCION PROPIA USADA
            //POR EL REGISTRO DE TURNOS, POR ENDE SI SE QUIERE ELIMINAR SE VERA AFECTADO
            //LOS TURNOS QUE LA ESTEN UTILIZANDO.
            var estadoTurnoExiste = await _context.EstadoTurnos.FirstOrDefaultAsync(e => e.EstadoTurnoId == id);
            if (estadoTurnoExiste == null)
            {
                return BadRequest($"El registro con id={id} no existe");
            }

            try
            {
                await _IEstadoTurnoRepositorio.Delete(id);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;

                return BadRequest($"Algo salio mal {message}");
            }

            return RedirectToAction(nameof(GetEstadosTurno));
  
        }


    }
}
