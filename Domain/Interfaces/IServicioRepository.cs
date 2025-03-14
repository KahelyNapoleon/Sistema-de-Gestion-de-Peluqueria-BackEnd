using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IServicioRepository : IGenericRepository<Servicio>
    {
        Task<List<Servicio>> ObtenerPorTipoServicioAsync(int tipoServicioId); 
    }
}
