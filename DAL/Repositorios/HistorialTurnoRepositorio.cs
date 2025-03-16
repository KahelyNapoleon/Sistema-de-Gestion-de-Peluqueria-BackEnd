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
    class HistorialTurnoRepositorio : IHistorialTurnoRepository
    {
        private readonly ApplicationDbContext _context;

        public HistorialTurnoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        //implementaciones metodos CRUD basicos de IGenericRepository<Historialturno>

        public async Task<List<HistorialTurno>> GetAllAsync()
        {
            return await _context.HistorialTurnos.ToListAsync();
        }

        public async Task<HistorialTurno?> GetByIdAsync(int id)
        {
            return await _context.HistorialTurnos.FindAsync(id);
        }

        public async Task AddAsync(HistorialTurno nuevoHistorial)
        {
            _context.HistorialTurnos.Add(nuevoHistorial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HistorialTurno actualizarHistorial)
        {
            _context.HistorialTurnos.Update(actualizarHistorial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var historialExiste = await _context.HistorialTurnos.FindAsync(id);
            if (historialExiste != null)
            {
                _context.HistorialTurnos.Remove(historialExiste);
                await _context.SaveChangesAsync();
            }
          
        }

        //Metodos agregados a la Interfaz HistorialTurno
        public async Task<IEnumerable<HistorialTurno>> ObtenerPorAdministradorAsync(int administradorId)
        {
            return await _context.HistorialTurnos
                .Include(h => h.Administrador)
                .Where(h => h.AdministradorId == administradorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialTurno>> ObtenerPorTurnoAsync(int turnoId)
        {
            return await _context.HistorialTurnos
                .Include(h => h.Turno)
                .Where(h => h.TurnoId == turnoId)
                .ToListAsync();
        }


    }
}
