using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicioRepository _IServicioRepository;

        public ServiciosController(IServicioRepository servicioRepository, ApplicationDbContext context)
        {
            _context = context;
            _IServicioRepository = servicioRepository;
        }


        [HttpGet]
        [Route("/servicios")]
        public async Task<IActionResult> GetServicios()
        {
            var servicios = await _IServicioRepository.GetAllAsync();
            if (servicios == null || !servicios.Any())
            {
                return Ok("Aun no hay registros de Servicios");             
            }

            return Ok(servicios);
        }

        [HttpGet]
        [Route("/servicio/{id}")]
        public async Task<IActionResult> GetServicio(int id)
        {
            try
            {
                var servicio = await _IServicioRepository.GetByIdAsync(id);
                if (servicio == null)
                {
                    return NotFound("El id no corresponde a un Servicio");
                }
                return Ok(servicio);    
            }
            catch(DbException ex)
            {
                return StatusCode(500 , ex.InnerException?.Message ?? ex.Message);
            }
        }


        [HttpPost]
        [Route("/agregar/servicio")]
        public async Task<IActionResult> AgregarServicio([FromBody] Servicio servicio)
        {
            try
            {
                var servicioNuevo = new Servicio
                {
                    Descripcion = servicio.Descripcion,
                    Precio = (decimal)servicio.Precio,
                    Duracion = servicio.Duracion,
                    Observacion = servicio.Observacion,
                };

                await _IServicioRepository.AddAsync(servicioNuevo);

                return CreatedAtAction(nameof(GetServicio), new {id = servicioNuevo.ServicioId}, servicioNuevo);

            }catch(DbUpdateConcurrencyException ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        [HttpPatch]
        [Route("/editar/servicio/{id}")]
        public async Task<IActionResult> ActualizarServicio([FromBody] Servicio servicio, int id)
        {
            try
            {
                var servicioEditar = await _context.Servicios.FindAsync(id);
                if (servicioEditar == null)
                {
                    return NotFound();
                }

                await _IServicioRepository.UpdateAsync(servicioEditar);
                return CreatedAtAction(nameof(GetServicio), new {id = servicioEditar.ServicioId }, servicioEditar );

            }
            catch(DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete]
        [Route("/eliminar/servicio/{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            try
            {
                var servicioEliminar = await _context.Servicios.FirstOrDefaultAsync(s => s.ServicioId == id);
                if (servicioEliminar == null)
                {
                    return NotFound($"El servicio id={id} no existe en la base de datos.");
                }

                await _IServicioRepository.Delete(id);
                return NoContent();

            }catch(DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }





    }
}
