


using Microsoft.AspNetCore.Mvc;
using DAL.Data;
using DAL.Repositorios.Interfaces;
using DAL.Repositorios;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {

        private readonly IClienteRepository _IClienteRepositorio;

        public ClientesController(IClienteRepository IClienteRepositorio)
        {
            _IClienteRepositorio = IClienteRepositorio;
        }

        [HttpGet]
        [Route("/Clientes")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _IClienteRepositorio.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet]
        [Route("/Cliente/{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            try
            {
                var cliente = await _IClienteRepositorio.GetByIdAsync(id);
                return Ok(cliente);
            }
            catch(Exception ex)
            {
                //var problemDetails = new ProblemDetails
                //{
                //    Title = "Cliente no encontrado",
                //    Status = StatusCodes.Status404NotFound,
                //    Detail = ex.Message,
                //    Instance = HttpContext.Request.Path
                //};

                //return NotFound(problemDetails); // Retorna JSON con estructura estándar
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPost]
        [Route("/AgregarCliente")]
        public async Task<IActionResult> AgregarCliente([FromBody] Cliente cliente)
        {
            try
            {
                var nuevoCliente = new Cliente
                {
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    NroCelular = cliente.NroCelular,
                    FechaNacimiento = cliente.FechaNacimiento,
                    CorreoElectronico = cliente.CorreoElectronico
                };

                await _IClienteRepositorio.AddAsync(nuevoCliente);

                return Ok(cliente);


            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(nameof(cliente));
            }
        }

        [Route("/ActualizarCliente/{id}")]
        [HttpPatch]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (id != cliente.ClienteId)
                {
                    return BadRequest();
                }

                await _IClienteRepositorio.UpdateAsync(cliente);


            }catch(DbUpdateConcurrencyException)
            {
                if (_IClienteRepositorio.)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


    }
}
