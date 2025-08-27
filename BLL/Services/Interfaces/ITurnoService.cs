using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;

namespace BLL.Services.Interfaces
{
    public interface ITurnoService
    {
        //ObtenerTurnos
        Task<OperationResult<IEnumerable<Turno>>> ObtenerTodos();
        //Obtener turno Por id
        Task<OperationResult<Turno>> ObtenerPorId(int id);
        //Crear
        Task<OperationResult<Turno>> Crear(Turno turno);
        //Actualizar EstadoTurno
        Task<OperationResult<Turno>> ActualizarEstadoTurno(int turnoId, EstadoTurno estadoTurno);
        //Actualizar FechaTurno
        Task<OperationResult<Turno>> ActualizarFechaYHoraTurno(int turnoId, DateOnly nuevaFecha, TimeOnly nuevaHora, EstadoTurno nuevoEstado);
        //Eliminar
        Task<OperationResult<string>> Eliminar(int turnoId);


        //VALIDAR
        OperationResult<Turno> ValidarTurno(Turno turno);
    }
}
