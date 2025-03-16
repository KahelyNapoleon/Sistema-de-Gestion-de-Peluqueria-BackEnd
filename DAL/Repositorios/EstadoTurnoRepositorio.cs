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
    class EstadoTurnoRepositorio : IGenericRepository<EstadoTurno>
    {
        private readonly ApplicationDbContext _context;

        public EstadoTurnoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EstadoTurno>> GetAllAsync()
        {
            return await _context.EstadoTurnos.ToListAsync();
        }

        public async Task<EstadoTurno?> GetByIdAsync(int id)
        {
            return await _context.EstadoTurnos.FindAsync(id);
        }

        public async Task UpdateAsync(EstadoTurno estadoTurno)
        {
            _context.EstadoTurnos.Update(estadoTurno);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(EstadoTurno estadoTurno)
        {
            _context.EstadoTurnos.Add(estadoTurno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var estadoTurnoExiste = await _context.EstadoTurnos.FindAsync(id);

            if (estadoTurnoExiste != null)
            {
                _context.EstadoTurnos.Remove(estadoTurnoExiste);
                await _context.SaveChangesAsync();
            }
        }

    }
}
