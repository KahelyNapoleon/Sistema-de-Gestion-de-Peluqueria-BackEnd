using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SistemaGestionPeluqueria.ApiWeb.DTOs;
using BLL.Services.Interfaces;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoServiciosController : Controller
    {
        private readonly ITipoServicioService _tiposervicioService;

        public TipoServiciosController(ITipoServicioService tipoServicioService)
        {
            _tiposervicioService = tipoServicioService;
        }

        [HttpGet]
        [Route("/api/tiposervicios")]
        public async Task<IActionResult> GetTipoServicios()
        {

            var tipoServicios = await _tiposervicioService.ObtenerTodos();
            if (!tipoServicios.Success)
            {
                return BadRequest(tipoServicios.Errors);
            }
            return Ok(tipoServicios.Data);
        }

        [HttpGet]
        [Route("/api/tiposervicio/{id}")]
        public async Task<IActionResult> GetTipoServicio(int id)
        {
            var tipoServicio = await _tiposervicioService.ObtenerPorId(id);
            if (!tipoServicio.Success)
            {
                return NotFound(tipoServicio.Errors);
            }

            return Ok(tipoServicio.Data);
        }

        [HttpPost]
        [Route("/api/agregar/tiposervicio")]
        public async Task<IActionResult> AgregarTipoServicio([FromBody] TipoServicioCreateDTO tipoServicio)
        {
            var tipoServicioCreate = new TipoServicio { Descripcion = tipoServicio.Descripcion};

            var crearTipoServicio = await _tiposervicioService.Crear(tipoServicioCreate);
            if (!crearTipoServicio.Success)
            {
                return BadRequest(crearTipoServicio.Errors);
            }

            return CreatedAtAction(nameof(GetTipoServicio), new { id = crearTipoServicio.Data!.TipoServicioId }, crearTipoServicio.Data);

        }

        [HttpPatch]
        [Route("/api/actualizar/tipoServicio/{id}")]
        public async Task<IActionResult> ActualizarTipoServicio([FromBody] TipoServicioUpdateDTO tipoServicio, int id)
        {
            var tipoServicioActualizar = new TipoServicio { Descripcion = tipoServicio.Descripcion };

            var tipoServicioExiste = await _tiposervicioService.Actualizar(tipoServicioActualizar, id);
            if (!tipoServicioExiste.Success)
            {
                return BadRequest(tipoServicioExiste.Errors);
            }

            return CreatedAtAction(nameof(GetTipoServicio), new { id = tipoServicioExiste.Data!.TipoServicioId }, tipoServicioExiste.Data);
        }

        [HttpDelete]
        [Route("/api/eliminar/tiposervicio/{id}")]
        public async Task<IActionResult> EliminarTipoServicio(int id)
        {
            var tipoServicioEliminar = await _tiposervicioService.Eliminar(id);
            if (!tipoServicioEliminar.Success)
            {
                return BadRequest(tipoServicioEliminar.Errors);
            }

            return Ok(tipoServicioEliminar.Data); 

        }

    }
}
