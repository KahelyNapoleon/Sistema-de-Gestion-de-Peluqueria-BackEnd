using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;

namespace BLL.Services.Interfaces
{
    public interface ITipoServicioService
    {
        //ObtenerTodos
        Task<OperationResult<IEnumerable<TipoServicio>>> ObtenerTodos();
        //ObtenerPorId
        Task<OperationResult<TipoServicio>> ObtenerPorId(int id);
        //Crear
        Task<OperationResult<TipoServicio>> Crear(TipoServicio tipoServicio);
        //Actualizar
        Task<OperationResult<TipoServicio>> Actualizar(TipoServicio tipoServicio, int id);
        //Eliminar
        Task<OperationResult<string>> Eliminar(int id);



        //Validar TipoServicio
        OperationResult<TipoServicio> ValidarTipoServicio(TipoServicio tipoServicio);
    }
}
