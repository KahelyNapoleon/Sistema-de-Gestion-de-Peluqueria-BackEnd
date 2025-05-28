using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodos();
        Task<T?> ObtenerPorId(int id);
        void Crear(T entity);
        void Actualizar(T entity);
        void Eliminar(int id);
        
        

    }
}
