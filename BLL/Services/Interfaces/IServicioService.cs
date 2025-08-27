using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.OperationResult;
using DomainLayer.Models;

namespace BLL.Services.Interfaces
{
    public interface IServicioService
    {
        //GetAll
        Task<OperationResult<IEnumerable<Servicio>>> ObtenerTodos();
        //GetById
        Task<OperationResult<Servicio>> ObtenerPorId(int id);
        //Crear
        Task<OperationResult<Servicio>> Crear(Servicio servicio);
        //Actualizar
        Task<OperationResult<Servicio>> Actualizar(Servicio servicio, int id);
        //Eliminar
        Task<OperationResult<string>> Eliminar(int id);
        
        //Validar Servicio
        OperationResult<Servicio> ValidarServicio(Servicio servicio);
    }
}
