


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
    class RepositorioGenerico<T> : IGenericRepository<T>  where T : class
    {
        private ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public RepositorioGenerico(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entityExist = await _dbSet.FindAsync(id);
            if (entityExist != null)
            {
                _dbSet.Remove(entityExist);
                await _context.SaveChangesAsync();
            }
        }

    }
}
