using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    internal interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerTodos();
        Task<OperationResult<Cliente>> ObtenerPorId();
        Task<OperationResult<Cliente>> Crear(Cliente cliente);
        Task<OperationResult<Cliente>> Actualizar(Cliente cliente, int id);
        Task<OperationResult<bool>> Eliminar(int id);
        
        //Validacion
        OperationResult<Cliente> ValidarCliente(Cliente cliente);
        
    }
}
