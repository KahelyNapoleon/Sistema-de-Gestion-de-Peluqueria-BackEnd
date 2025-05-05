


using Microsoft.AspNetCore.Mvc;
using DAL.Data;
using DAL.Repositorios.Interfaces;
using DAL.Repositorios;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {

        private readonly IClienteRepository _IClienteRepositorio;
        private readonly ApplicationDbContext _context;

        public ClientesController(IClienteRepository IClienteRepositorio, ApplicationDbContext context)
        {
            _IClienteRepositorio = IClienteRepositorio;
            _context = context;
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

                return Ok(nuevoCliente);


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

            if (id != cliente.ClienteId)
            {
                return BadRequest("El id no coincide con el Id del Cliente");
            }

            try
            {
                await _IClienteRepositorio.UpdateAsync(cliente);

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
            var clienteExiste = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);
            if (clienteExiste == null)
            {
                return BadRequest($"El Cliente con id={id} no existe en la base de datos");
            }
            try
            {
                await _IClienteRepositorio.Delete(id); 
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;

                return BadRequest($"No es posible eliminar el Cliente: {message}");

            }

            return RedirectToAction(nameof(GetClientes));

        } 

    }
}
