using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models;
using DAL.Data;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;

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
            var estadosTurnos = await _IEstadoTurnoRepositorio.GetAllAsync();
            if (estadosTurnos == null || !estadosTurnos.Any())
            {
                return BadRequest("La lista de estadoTurnos se encuentra vacia");
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
                return BadRequest($"No existe registro con id={id} en la tabla EstadoTurno");
            }

            try
            {
                var estadoTurno = _IEstadoTurnoRepositorio.GetByIdAsync(id);
                return Ok(estadoTurno);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

        }


    }
}
