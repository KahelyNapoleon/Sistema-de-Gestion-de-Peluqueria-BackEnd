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
        [Route("/api/metodospagos")]
        public async Task<IActionResult> GetMetodosPagos()
        {
            var metodosPagos = await _metodoPagoService.ObtenerTodos();
            if (!metodosPagos.Success)
            {
                return BadRequest(metodosPagos.Errors);
            }

            return Ok(metodosPagos.Data);
        }

        [HttpGet]
        [Route("/api/metodopago/{id}")]
        public async Task<IActionResult> GetMetodoPago(int id)
        {
            var metodoPago = await _metodoPagoService.ObtenerPorId(id);
            if (!metodoPago.Success)
            {
                return NotFound(metodoPago.Errors);
            }

            return Ok(metodoPago.Data);
        }

        [HttpPost]
        [Route("/api/agregar/metodopago")]
        public async Task<IActionResult> AgregarMetodoPago([FromBody] MetodoPagoDTO metodoPago)
        {

            var metodoPagoValidar = new MetodoPago { Descripcion = metodoPago.Descripcion, };

            //metodoPagoCrear es de tipo OperationResult<MetodoPago>
            var metodoPagoCrear = await _metodoPagoService.Crear(metodoPagoValidar);
            if (!metodoPagoCrear.Success)
            {
                return BadRequest(metodoPagoCrear.Errors);
            }

            return CreatedAtAction(nameof(GetMetodoPago), new { id = metodoPagoValidar.MetodoPagoId }, metodoPagoValidar);

        }

        [HttpPatch]
        [Route("/api/actualizar/metodopago/{id}")]
        public async Task<IActionResult> ActualizarMetodoPago([FromBody] MetodoPago actualizarMetodoPago, int id)
        {
            var validarMetodoPago = await _metodoPagoService.Actualizar(actualizarMetodoPago, id);
            if (!validarMetodoPago.Success)
            {
                return BadRequest(validarMetodoPago.Errors);
            }

            return CreatedAtAction(nameof(GetMetodoPago), new { id = validarMetodoPago.Data!.MetodoPagoId}, validarMetodoPago.Data);
        }

        [HttpDelete]
        [Route("/api/eliminar/metodopago/{id}")]
        public async Task<IActionResult> EliminarMetodoPago(int id)
        {
            var metodoPagoEliminar = await _metodoPagoService.Eliminar(id);
            if (!metodoPagoEliminar.Success) //Si el resultado booleano del tipo opResult es false.
            {
                return BadRequest(metodoPagoEliminar.Errors);
            }

            return Ok(metodoPagoEliminar.Data);
        }

    }
}
