using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ServicioGenerico<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _IGenericRepository;
        private IValidationDictionary _IValidationDictionary;
     
        
        public ServicioGenerico(IGenericRepository<TEntity> genericRepository, IValidationDictionary validationDictionary)
        {
            _IGenericRepository = genericRepository;   
            _IValidationDictionary = validationDictionary;
        }

        public virtual async Task<IEnumerable<TEntity>> ObtenerTodos() => await _IGenericRepository.GetAllAsync();
        public virtual async Task<TEntity?> ObtenerPorId(int id) => await _IGenericRepository.GetByIdAsync(id);    
        public virtual void Crear(TEntity entity) => _IGenericRepository.AddAsync(entity);
        public virtual void Actualizar(TEntity entity) => _IGenericRepository.UpdateAsync(entity);
        public virtual void Eliminar(int id) =>  _IGenericRepository.Delete(id);
        

      

    }
}
