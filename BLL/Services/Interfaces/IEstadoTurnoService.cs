using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace BLL.Services.Interfaces
{
    public interface IEstadoTurnoService
    {
        Task<OperationResult<IEnumerable<EstadoTurno>>> ObtenerTodos();
        Task<OperationResult<EstadoTurno>> ObtenerPorId(int id);
        Task<OperationResult<EstadoTurno>> Crear(EstadoTurno estadoTurno);
        Task<OperationResult<EstadoTurno>> Actualizar(EstadoTurno estadoTurno, int id);
        Task<OperationResult<bool>> Eliminar(int id);

        //Validacion
        OperationResult<EstadoTurno> ValidarEstadoTurno(EstadoTurno estadoTurno);
    }
}
