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
    class EstadoTurnoRepositorio :  RepositorioGenerico<EstadoTurno>
    {
        private readonly ApplicationDbContext _context;

        public EstadoTurnoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
