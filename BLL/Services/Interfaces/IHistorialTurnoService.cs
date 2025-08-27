using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.OperationResult;
using DomainLayer;
using DomainLayer.Models;

namespace BLL.Services.Interfaces
{
    public interface IHistorialTurnoService
    {
        Task<OperationResult<IEnumerable<HistorialTurno>>> ObtenerTodos();
        Task<OperationResult<HistorialTurno>> ObtenerPorId(int id);

        //ESTOS METODOS VAN EN EL SERVICIO DE TURNO:
        //Task<OperationResult<HistorialTurno>> RegistrarCambioEstado(int turnoId, int estadoAnterior, int estadoActual, int AdministradorId);
        //Task<OperationResult<HistorialTurno>> RegistrarCambioFecha(int turnoId, DateTime nuevaFecha, int administradorId);

    }
}
