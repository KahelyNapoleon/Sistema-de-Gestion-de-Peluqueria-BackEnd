using DAL.Data;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;
using DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context; //CONSTANTE EN TIEMPO DE EJECUCION
        private IDbContextTransaction _transaction; //VARIABLE DE INSTANCIA PRIVADA.

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }

        //VARIABLES DE INSTANCIA 
        private ITurnoRepository _turnoRepository;
        public ITurnoRepository TurnoRepository => _turnoRepository ??= new TurnoRepositorio(_context);

        private IHistorialTurnoRepository _historialTurnoRepository;
        public IHistorialTurnoRepository HistorialTurnoRepository => _historialTurnoRepository ??= new HistorialTurnoRepositorio(_context);


        public async Task IniciarTransaccionAsync()
        {
            var iniciarTransaccion = await _context.Database.BeginTransactionAsync();
            if (_transaction != null)
            {
                return;
            }
            _transaction = iniciarTransaccion;
        }
        
        public async Task GuardarCambiosAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null!;

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (_transaction != null)
                {
                    await _transaction!.RollbackAsync();
                    _transaction.Dispose();
                    _transaction = null!;
                }
                throw;
            }          
        }


        public async Task DeshacerCambiosAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null!;
            }

        }


        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
