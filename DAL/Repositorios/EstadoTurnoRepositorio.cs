using DAL.Data;
using DAL.Repositorios;
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
    public class EstadoTurnoRepositorio :  RepositorioGenerico<EstadoTurno>, IEstadoTurnoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstadoTurnoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
