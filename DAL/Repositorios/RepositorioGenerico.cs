


using DAL.Data;
using DAL.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    public class RepositorioGenerico<TEntity> : IGenericRepository<TEntity>  where TEntity : class
    {
        private readonly ApplicationDbContext _context; //Este campo se le asignara un valor en tiempo de 
        private readonly DbSet<TEntity> _dbSet;         // ejecucion, por medio del constructor.

        public RepositorioGenerico(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// OBTIENE Y CONVIERTE A LISTA LOS REGISTROS DE DICHA ENTIDAD.
        /// </summary>
        /// <returns>UNA LISTA DE REGISTROS</returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); 
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
           
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entityExist =  await _dbSet.FindAsync(id);
            if (entityExist != null)
            {
                _dbSet.Remove(entityExist);
                await _context.SaveChangesAsync();
            }

        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                 _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        } 

        public virtual async Task<bool> VerificarSiExiste(int id)
        {
            var entidad = await _dbSet.FindAsync(id);

            if (entidad == null)
            {
                return false;
            }

            return true;
        }

        public virtual async Task<TEntity?> BuscarAsync(int id)
        {
            var entidad = await _dbSet.FindAsync(id);

            if (entidad == null)
            {
                return null;
            }

            return entidad;
        }


    }
}
