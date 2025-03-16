using DAL.Data;
using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    class AdministradorRepositorio : IGenericRepository<Administradores>
    {
        private ApplicationDbContext _context;

        public AdministradorRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Administradores?> GetByIdAsync(int id)
        {
            return await _context.Administradores.FindAsync(id);
        }

        public async Task<List<Administradores>> GetAllAsync()
        {
            return await _context.Administradores.ToListAsync();
        }

        public async Task AddAsync(Administradores administrador)
        {
            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Administradores administrador)
        {
            _context.Administradores.Update(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var administradorExiste = await _context.Administradores.FindAsync(id);
            if (administradorExiste != null)
            {
                _context.Administradores.Remove(administradorExiste);
                await _context.SaveChangesAsync();
            }
        }
    }
}
