using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Repositorios;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using SistemaGestionPeluqueria.ApiWeb.DTOs;
using Microsoft.EntityFrameworkCore;
using DAL.Data;

namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MetodoPagosController : Controller
    {
        //CONSTANTE EN TIEMPO DE EJECUCION SIGNIFICA QUE SE LE PUEDE OTORGAR UN VALOR DINAMICO
        //EN ESTE CASO EL VALOR SE LOS DAMOS A TRAVES DEL CONSTRUCTOR...
        private readonly IMetodoPagoService _metodoPagoService;
        

        public MetodoPagosController(IMetodoPagoService metodoPagoService)
        {
           _metodoPagoService = metodoPagoService;
        }

        [HttpGet]
        [Route("/metodospagos")]
        public async Task<IActionResult> GetMetodosPagos()
        {
            var metodosPagos = await _metodoPagoService.ObtenerTodos();
            if (metodosPagos == null || !metodosPagos.Any())
            {
                return Ok("Aun no hay registros");
            }

            return Ok(metodosPagos);
        }

        [HttpGet]
        [Route("/metodopago/{id}")]
        public async Task<IActionResult> GetMetodoPago(int id)
        {
            try
            {
                var metodoPago = await _metodoPagoService.ObtenerPorId(id); //Falta validar que el Id Exista o sea valido
                if (metodoPago == null)
                {
                    return BadRequest($"El registro con id={id} no se encuentra en la base de datos");
                }

                return Ok(metodoPago);
            }
            catch(DbException ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        [Route("/agregar/metodopago")]
        public async Task<IActionResult> AgregarMetodoPago([FromBody] MetodoPagoDTO metodoPago)
        {

            var metodoPagoValidar = new MetodoPago{ Descripcion = metodoPago.Descripcion, };

            //metodoPagoCrear sera de tipo OperationResult<MetodoPago>
            var metodoPagoCrear = await _metodoPagoService.Crear(metodoPagoValidar);
            if (!metodoPagoCrear.Success)
            {
                return BadRequest(metodoPagoCrear.Errors);
            }

            return CreatedAtAction(nameof(GetMetodoPago), new { id = metodoPagoValidar.MetodoPagoId}, metodoPagoValidar);

        }

        [HttpPatch]
        [Route("/metodopago/actualizar/{id}")]
        public async Task<IActionResult> ActualizarMetodoPago([FromBody] MetodoPago actualizarMetodoPago, int id)
        {
            var validarMetodoPago = await _metodoPagoService.Actualizar(actualizarMetodoPago, id);
            if (!validarMetodoPago.Success)
            {
                return BadRequest(validarMetodoPago.Errors);
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("/metodopago/eliminar/{id}")]
        public async Task<IActionResult> EliminarMetodoPago(int id)
        {
            try
            {
                var metodoPagoEliminar = await _metodoPagoService.Eliminar(id);
                if (!metodoPagoEliminar.Success) //Si el resultado booleano del tipo opResult es false.
                {
                    return NotFound(metodoPagoEliminar.Errors);
                }

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
