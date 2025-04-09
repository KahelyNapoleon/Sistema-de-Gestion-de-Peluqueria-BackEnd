using DAL.Data;
using DAL.Interfaces;
using DAL.Repositorios;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
      
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
           
        }

        private ITurnoRepository _turnoRepository;
        public ITurnoRepository TurnoRepository => _turnoRepository ??= new TurnoRepositorio(_context);

        private IHistorialTurnoRepository _historialTurnoRepository;



    }
}
