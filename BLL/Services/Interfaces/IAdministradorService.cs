using BLL.Services.OperationResult;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IAdministradorService
    {
        Task<OperationResult<IEnumerable<Administrador>>> ObtenerTodos();
        Task<OperationResult<Administrador>> ObtenerPorId(int id);
        Task<OperationResult<Administrador>> Crear(Administrador admin);
        Task<OperationResult<Administrador>> Actualizar(Administrador admin, int id);
        Task<OperationResult<string>> Eliminar(int id);
        OperationResult<Administrador> ValidarAdministrador(Administrador admin);
        Task<OperationResult<Administrador>> IniciarSesion(string correo, string contrasenia);
    }
}
