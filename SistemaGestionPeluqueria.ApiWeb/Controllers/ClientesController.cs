using Microsoft.AspNetCore.Mvc;
using DAL.Data;
using DAL.Repositorios.Interfaces;
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
        [Route("/Cliente/{id}")]
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
        [Route("/AgregarCliente")]
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
        [Route("/ActualizarCliente/{id}")]
        [HttpPatch]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteDTO cliente)
        {
            //buscar Id del cliente
            var clienteExiste = await _IClienteService.ObtenerPorId(id);
            if (!clienteExiste.Success) //Aqui el servicio que obtiene el cliente por id, si el id no existe
            {                           //retorna un OperationResult<Cliente>.Fail($"El cliente con id {id} no existe")
                return NotFound(clienteExiste.Errors);
            }

            try
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

                await _IClienteService.Actualizar(clienteActualizar, id);

            }catch(DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;
                return BadRequest($"Algo salio mal {message}");
            }

            return NoContent();
        }


        //NO SE ELIMINA, PROBADO POR POSTMAN, 
        //VER QUE CUANDO INGRESAMOS EL ID EN LA SOLICITUD DEL ENDPOINT, NOS REDIRIGUE AL METODO GETALL PERO NO ELIMINA 
        //LA ENTIDAD CON EL ID INGRSADO
        [Route("/eliminar/cliente/{id}")]
        [HttpDelete]
        public async Task<IActionResult> EliminarCliente(int id)
        {

            try
            {
                var eliminacionCompletada = await _IClienteService.Eliminar(id);
                if (!eliminacionCompletada.Success)
                {
                    return NotFound(eliminacionCompletada.Errors);
                }
              
                return Ok(eliminacionCompletada.Data);
                //return RedirectToAction(nameof(GetClientes));
                
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;

                return BadRequest($"No es posible eliminar el Cliente: {message}");

            }
        } 

    }
}
