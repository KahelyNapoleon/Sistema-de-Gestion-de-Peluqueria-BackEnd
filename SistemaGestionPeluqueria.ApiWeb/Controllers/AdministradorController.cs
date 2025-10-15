using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using SistemaGestionPeluqueria.ApiWeb.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Threading.Tasks;


namespace SistemaGestionPeluqueria.ApiWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministradorController : Controller
    {
        private readonly IAdministradorService _administradorService;

        public AdministradorController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        [HttpGet]
        [Route("/api/administradores")]
        public async Task<IActionResult> GetAdministradores()
        {
            var administradores = await _administradorService.ObtenerTodos();
            if (!administradores.Success)
            {
                return BadRequest(administradores.Errors);
            }

            return Ok(administradores.Data);
        }

        [HttpGet]
        [Route("/api/administrador/{id}")]
        public async Task<IActionResult> GetAdministrador(int id)
        {
            var admin = await _administradorService.ObtenerPorId(id);
            if (!admin.Success)
            {
                return NotFound(admin.Errors);
            }
            return Ok(admin.Data);
        }

        [HttpPost]
        [Route("/api/administrador/crear")]
        public async Task<IActionResult> Crear([FromBody] AdminCreateDTO administrador)
        {
            var nuevoAdmin = new Administrador
            {
                Usuario = administrador.Usuario,
                Correo = administrador.Correo,
                Contrasena = administrador.Contrasena
            };

            var resultadoAdmin = await _administradorService.Crear(nuevoAdmin);
            if (!resultadoAdmin.Success)
            {
                return BadRequest(resultadoAdmin.Errors);
            }

            return CreatedAtAction(nameof(GetAdministrador), new { id = nuevoAdmin.AdministradorId }, nuevoAdmin);

        }

        [HttpPatch]
        [Route("/api/actualizar/administrador/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AdminUpdateDTO admin)
        {
           
            var adminActualizar = new Administrador
            {
                Correo = admin.Correo,
                Usuario = admin.Usuario,
                Contrasena = admin.Contrasena
            };

            var adminExiste = await _administradorService.Actualizar(adminActualizar, id);

            if (!adminExiste.Success)
            {
                return BadRequest(adminExiste.Errors);
            }

            //return CreatedAtAction(nameof(GetAdministrador), new { id = adminExiste.Data!.AdministradorId}, adminExiste.Data);
            return NoContent();
        }

        [HttpDelete]
        [Route("/api/eliminar/administrador/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var adminEliminar = await _administradorService.Eliminar(id);
            if (adminEliminar.Success)
            {
                return NotFound(adminEliminar.Data); //La IAdministradorService declara como tipo de resultado
                                                     //al metodo de eliminar como OperationResult<string>
            }

            //return Ok(adminEliminar.Data); //Aqui data retorna un mensaje que verifica que se elimino el registro.
            return NoContent();
        }

    }
}
