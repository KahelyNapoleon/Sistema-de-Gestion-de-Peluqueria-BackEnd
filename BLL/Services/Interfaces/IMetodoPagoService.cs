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
        Task<IEnumerable<MetodoPago>> ObtenerTodos();
        Task<MetodoPago?> ObtenerPorId(int id);
        Task<OperationResult<MetodoPago>> Crear(MetodoPago metodoPago);
        Task<OperationResult<MetodoPago>> Actualizar(MetodoPago metodoPago, int id);
        Task<bool> Eliminar(int id);
        OperationResult<MetodoPago> ValidarMetodoPago(MetodoPago metodoPago);
    }
}
