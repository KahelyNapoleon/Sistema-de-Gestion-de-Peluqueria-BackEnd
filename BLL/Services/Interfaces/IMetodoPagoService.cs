using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMetodoPagoService //: IGenericService<MetodoPago> 
    {
        Task<IEnumerable<MetodoPago>> ObtenerTodos();
        Task<MetodoPago?> ObtenerPorId(int id);
        Task<bool> Crear(MetodoPago metodoPago);
        Task<bool> Actualizar(MetodoPago metodoPago);
        Task<bool> Eliminar(int id);
        bool ValidarMetodoPago(MetodoPago metodoPago);
    }
}
