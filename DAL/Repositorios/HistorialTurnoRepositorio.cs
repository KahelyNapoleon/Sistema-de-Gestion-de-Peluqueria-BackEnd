using DAL.Data;
using DAL.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    class HistorialTurnoRepositorio : RepositorioGenerico<HistorialTurno> ,IHistorialTurnoRepository
    {
        private readonly ApplicationDbContext _context;

        public HistorialTurnoRepositorio(ApplicationDbContext context) : base(context) 
        {
            _context = context;
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
