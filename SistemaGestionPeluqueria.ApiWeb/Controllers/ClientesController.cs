using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using SistemaGestionPeluqueria.ApiWeb.DTOs;


namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {

        private readonly IClienteService _IClienteService;

        public ClientesController(IClienteService clienteService)
        {
            _IClienteService = clienteService;
        }

        [HttpGet]
        [Route("/api/clientes")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _IClienteService.ObtenerTodos();
            if (!clientes.Success)
            {
                return BadRequest(clientes.Errors);
            }
            return Ok(clientes.Data);
        }



        [HttpGet]
        [Route("/api/cliente/{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _IClienteService.ObtenerPorId(id);
            if (!cliente.Success)
            {
                return BadRequest(cliente.Errors);
            }

            return Ok(cliente.Data);
        }


        [HttpPost]
        [Route("/api/agregarcliente")]
        public async Task<IActionResult> AgregarCliente([FromBody] ClienteDTO cliente)
        {

            var nuevoCliente = new Cliente
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NroCelular = cliente.NroCelular,
                FechaNacimiento = cliente.FechaNacimiento,
                CorreoElectronico = cliente.CorreoElectronico
            };

            var resultadoCliente = await _IClienteService.Crear(nuevoCliente);

            if (!resultadoCliente.Success)
            {
                return BadRequest(resultadoCliente.Errors);
            }

            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.ClienteId }, nuevoCliente);
        }


        /// <summary>
        /// Actualizacion datos de un Cliente registrado.
        /// </summary>
        /// <param name="id">ClienteId de la tabla clientes- se utiliza para buscarlo en la DB</param>
        /// <param name="cliente">Datos que la UI envia para actualizar los datos.</param>
        /// <returns>Un accionResult </returns>
        [Route("/api/actualizarcliente/{id}")]
        [HttpPatch]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteDTO cliente)
        {
            var clienteActualizar = new Cliente
            {
                ClienteId = id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NroCelular = cliente.NroCelular,
                FechaNacimiento = cliente.FechaNacimiento,
                CorreoElectronico = cliente.CorreoElectronico
            };

            var clienteActualizado = await _IClienteService.Actualizar(clienteActualizar, id);
            if (!clienteActualizado.Success)
            {
                return BadRequest(clienteActualizado.Errors);
            }

            return NoContent();
        }


        [Route("/api/eliminar/cliente/{id}")]
        [HttpDelete]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var eliminacionCompletada = await _IClienteService.Eliminar(id);
            if (!eliminacionCompletada.Success)
            {
                return NotFound(eliminacionCompletada.Errors);
            }

            return Ok(eliminacionCompletada.Data);
            //return RedirectToAction(nameof(GetClientes));
        }

    }
}
