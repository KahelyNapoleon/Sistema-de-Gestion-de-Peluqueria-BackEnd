using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id); //Obtiene un registro por su Id | HttpGEt
        Task<List<T>> GetAllAsync(); //Obtiene todos los registros | HttpGET
        Task AddAsync(T entity); //Crea un nuevo registro de tipo T(objeto)
        Task UpdateAsync(T entity);//Actualiza un registro de tipo T(objeto)
        void Delete(int id);//Elimina un registro por su ID.

        bool VerificarSiExiste(int id);
    }
}
