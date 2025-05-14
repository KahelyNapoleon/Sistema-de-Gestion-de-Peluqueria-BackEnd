using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITipoServicioRepository _ITipoServicioRepository;

        public TipoServiciosController(ApplicationDbContext context, ITipoServicioRepository ITipoServicioRepository)
        {
            _context = context;
            _ITipoServicioRepository = ITipoServicioRepository;
        }

        [HttpGet]
        [Route("/tiposervicios")]
        public async Task<IActionResult> GetTipoServicios()
        {
            try
            {
                var tipoServicios = await _ITipoServicioRepository.GetAllAsync();
                return Ok(tipoServicios);
            }
            catch(DbException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
            
        }

        [HttpGet]
        [Route("/tiposervicio/{id}")]
        public async Task<IActionResult> GetTipoServicio(int id)
        {
            try
            {
                //var tipoServicioExiste = await _ITipoServicioRepository.GetByIdAsync(id);
                var tipoServicioExiste = await _context.TipoServicios
                    .Include(t => t.Servicios)
                    .Where(t => t.TipoServicioId == id)
                    .ToListAsync();
                    

                if (tipoServicioExiste == null)
                {
                    return NotFound($"El id:{id} no coincide con un TipoServicio Existente");
                }
                return Ok(tipoServicioExiste);
            }
            catch (DbException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        [Route("/agregar/tiposervicio")]
        public async Task<IActionResult> PostTipoServicio([FromBody] TipoServicio tipoServicio)
        {
            try
            {
                var agregarTipoServicio = new TipoServicio
                {
                    Descripcion = tipoServicio.Descripcion
                };
                await _ITipoServicioRepository.AddAsync(agregarTipoServicio);
                return CreatedAtAction(nameof(GetTipoServicio), new{ id= agregarTipoServicio.TipoServicioId}, agregarTipoServicio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
            catch(DbException ex)
            {
                return StatusCode(500,ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPatch]
        [Route("/actualizar/tipoServicio/{id}")]
        public async Task<IActionResult> ActualizarTipoServicio([FromBody]TipoServicio tipoServicio, int id)
        {
            try
            {
                var tipoServicioExiste = await _context.TipoServicios.FindAsync(id);
                if (tipoServicioExiste == null)
                {
                    return NotFound($"No existe tipo de servicio con id={id}");
                }
                await _ITipoServicioRepository.UpdateAsync(tipoServicioExiste);
                return CreatedAtAction(nameof(GetTipoServicio), new { id = tipoServicioExiste.TipoServicioId }, tipoServicioExiste);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete]
        [Route("/eliminar/tiposervicio/{id}")]
        public async Task<IActionResult> EliminarTipoServicio(int id)
        {
            try
            {
                var tipoServicio = await _context.TipoServicios
                    .Include(t => t.Servicios)
                    .FirstOrDefaultAsync(t => t.TipoServicioId == id);
                if (tipoServicio == null)
                {
                    return NotFound($"El tipo de servicio con id={id} no existe en la base de datos");
                }

                await _ITipoServicioRepository.Delete(tipoServicio.TipoServicioId);
                return NoContent();
               
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
            catch (DbException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
