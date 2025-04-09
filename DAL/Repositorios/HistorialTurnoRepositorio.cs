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
    class HistorialTurnoRepositorio : RepositorioGenerico<HistorialTurno>, IHistorialTurnoRepository
    {
        private readonly ApplicationDbContext _context; //Una constante llamada readonly 
                                                        //tiene la capacidad de ser declarada en 
                                                        //tiempo de ejecucion.

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

        public async Task<IEnumerable<HistorialTurno>> ObtenerPorTurnoIdAsync(int turnoId)
        {
            try
            {
                var historialturnoExiste = await _context.HistorialTurnos.FindAsync(turnoId);
                if (historialturnoExiste != null)
                {
                    return await _context.HistorialTurnos
                   .Include(h => h.Turno)
                   .Where(h => h.TurnoId == turnoId)
                   .ToListAsync();
                }
                throw new InvalidOperationException("No se encuentra HistorialTurno en la operacion");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw new ArgumentNullException(nameof(HistorialTurno), "El valor del historial es Nulo");

            }

        }

        public async Task<HistorialTurno?> ObtenerPorEstadoActual(HistorialTurno historialTurno, string estadoActual)
        {
            try
            {
                var historialTurnoExiste = await _context.HistorialTurnos.FindAsync(historialTurno.HistorialTurnoId);
                if (historialTurnoExiste != null)
                {
                    return await _context.HistorialTurnos.
                        Include(h => h.Turno).
                           ThenInclude(t => t.EstadoTurno).
                        Where(h => h.Turno.EstadoTurno.Descripcion == estadoActual).
                        FirstOrDefaultAsync();
                }
                throw new InvalidOperationException("No se encuentra el Historial de turno especificado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw new ArgumentNullException(nameof(HistorialTurno), "No se encuentra HistorialTurno porque su valor es Nulo");
            }
        }


    }
}
