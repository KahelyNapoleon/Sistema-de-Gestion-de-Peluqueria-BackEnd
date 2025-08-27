using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositorios
{
    public class TurnoRepositorio : RepositorioGenerico<Turno> , ITurnoRepository
    {
        private readonly ApplicationDbContext _context;

        public TurnoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

     

        //Metodos ITurnoRepository -- busquedas y consultas
        public async Task<IEnumerable<Turno>> ObtenerTurnoPorCliente(int clienteId)
        {
            return await _context.Turnos
                .Include(t =>t.Cliente)
                .Where(t => t.ClienteId == clienteId)
                .ToListAsync();

        }

        public async Task<IEnumerable<Turno>> ObtenerTurnoPorEstadoTurno(int estadoTurnoId)
        {
            return await _context.Turnos
                .Include(t => t.EstadoTurno)
                .Where(t => t.EstadoTurnoId == estadoTurnoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Turno>> ObtenerTurnoPorFecha(DateOnly fechaTurno)
        {
            return await _context.Turnos
                .Where(t => t.FechaTurno == fechaTurno)
                .ToListAsync() ;
        }

        public async Task<IEnumerable<Turno>> ObtenerTurnoPorServicio(int servicioId)
        {
            return await _context.Turnos
                .Include(t => t.Servicio)
                .Where(t => t.ServicioId == servicioId)
                .ToListAsync();
        }

       
    }
}
