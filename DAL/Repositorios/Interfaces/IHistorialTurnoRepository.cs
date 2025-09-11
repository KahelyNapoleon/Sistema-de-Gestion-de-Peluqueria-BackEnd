using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DAL.Repositorios.Interfaces
{
    public interface IHistorialTurnoRepository : IGenericRepository<HistorialTurno>
    {
        //Task<IEnumerable<HistorialTurno>> ObtenerPorTurnoIdAsync(int turnoId);
        //Task<IEnumerable<HistorialTurno>> ObtenerPorAdministradorAsync(int administradorId);

        //Task<HistorialTurno?> ObtenerPorEstadoActual(HistorialTurno historialTurno, string estadoActual);

    }
}
