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
            _context.HistorialTurnos.Remove(id);
            await _context.SaveChangesAsync();
        }

        //Metodos agregados a la Interfaz HistorialTurno
        //public async Task<IEnumerable<HistorialTurno>> ObtenerPorAdministradorAsync(int id)
        //{
        //    var admnistradorExiste = await _context.Administradores.FindAsync(id);
        //    if(admnistradorExiste != null)
        //    {
        //        return _context.HistorialTurnos
        //            .Where();

        //    }
            
        //}
       

    }
}
