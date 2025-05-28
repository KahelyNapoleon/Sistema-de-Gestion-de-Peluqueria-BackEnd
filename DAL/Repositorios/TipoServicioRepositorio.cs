using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    public class TipoServicioRepositorio : RepositorioGenerico<TipoServicio>, ITipoServicioRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoServicioRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<TipoServicio?> BuscarAsync(int id)
        {
            var tipoServicio = await _context.TipoServicios
                .Include(t => t.Servicios)
                .FirstOrDefaultAsync(t => t.TipoServicioId == id);

            return tipoServicio;
        }

    }
}
