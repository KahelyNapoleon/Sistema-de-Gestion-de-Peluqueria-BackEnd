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
    [Route("[Controller]")]
    public class ServiciosController : Controller
    {
        private readonly IServicioService _IServicioService;

        public ServiciosController(IServicioService servicioService)
        {
            _IServicioService = servicioService;
        }

        [HttpGet]
        [Route("/api/servicios")]
        public async Task<IActionResult> GetServicios()
        {
            var servicios = await _IServicioService.ObtenerTodos();
            if (!servicios.Success)
            {
                return BadRequest(servicios.Errors);
            }

            return Ok(servicios.Data);
        }

        [HttpGet]
        [Route("/api/servicio/{id}")]
        public async Task<IActionResult> GetServicio(int id)
        {
            var servicio = await _IServicioService.ObtenerPorId(id);
            if (!servicio.Success)
            {
                return NotFound(servicio.Errors);
            }
            return Ok(servicio.Data);
        }


        [HttpPost]
        [Route("/api/agregar/servicio")]
        public async Task<IActionResult> AgregarServicio([FromBody] ServicioCreateDTO servicio)
        {
            var servicioNuevo = new Servicio
            {
                Descripcion = servicio.Descripcion,
                Precio = (decimal)servicio.Precio,
                Duracion = servicio.Duracion,
                Observacion = servicio.Observacion,
                TipoServicioId = servicio.TipoServicioId
            };

            var agregarServicio = await _IServicioService.Crear(servicioNuevo);
            if (!agregarServicio.Success)
            {
                return BadRequest(agregarServicio.Errors);
            }

            return CreatedAtAction(nameof(GetServicio), new { id = servicioNuevo.ServicioId }, servicioNuevo);
        }


        [HttpPatch]
        [Route("/api/editar/servicio/{id}")]
        public async Task<IActionResult> ActualizarServicio([FromBody] ServicioCreateDTO servicio, int id)
        {
            var servicioEditar = new Servicio
            {
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                Duracion = servicio.Duracion,
                Observacion = servicio.Observacion,
                TipoServicioId = servicio.TipoServicioId
            };

            var actualizarServicio = await _IServicioService.Actualizar(servicioEditar, id);
            if (!actualizarServicio.Success)
            {
                return BadRequest(actualizarServicio.Errors);
            }

            return CreatedAtAction(nameof(GetServicio), new { id = servicioEditar.ServicioId }, servicioEditar);
        }


        [HttpDelete]
        [Route("/api/eliminar/servicio/{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            var servicioEliminar = await _IServicioService.Eliminar(id);
            if (!servicioEliminar.Success)
            {
                return BadRequest(servicioEliminar.Errors);
            }
            return Ok(servicioEliminar.Data);
        }





    }
}
