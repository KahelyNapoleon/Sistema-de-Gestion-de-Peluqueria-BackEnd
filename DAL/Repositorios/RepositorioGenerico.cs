


using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    class RepositorioGenerico<TEntity> : IGenericRepository<TEntity>  where TEntity : class
    {
        private readonly ApplicationDbContext _context; //Este campo se le asignara un valor en tiempo de 
        private readonly DbSet<TEntity> _dbSet;         // ejecucion, por medio del constructor.

        public RepositorioGenerico(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual void Delete(int id)
        {
            var entityExist =  _dbSet.Find(id);
            if (entityExist != null)
            {
                _dbSet.Remove(entityExist);
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


    }
}
