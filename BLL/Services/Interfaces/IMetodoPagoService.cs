using BLL.Services.OperationResult;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMetodoPagoService 
    {
        Task<OperationResult<IEnumerable<MetodoPago>>> ObtenerTodos();
        Task<OperationResult<MetodoPago>> ObtenerPorId(int id);
        Task<OperationResult<MetodoPago>> Crear(MetodoPago metodoPago);
        Task<OperationResult<MetodoPago>> Actualizar(MetodoPago metodoPago, int id);
        Task<OperationResult<string>> Eliminar(int id);
        OperationResult<MetodoPago> ValidarMetodoPago(MetodoPago metodoPago);
    }
}
