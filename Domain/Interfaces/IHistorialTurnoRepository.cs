using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IHistorialTurnoRepository : IGenericRepository<HistorialTurno>
    {
        Task<IEnumerable<HistorialTurno>> ObtenerPorTurnoAsync(int turnoId);
        Task<IEnumerable<HistorialTurno>> ObtenerPorAdministradorAsync(int administradorId);

    }
}
