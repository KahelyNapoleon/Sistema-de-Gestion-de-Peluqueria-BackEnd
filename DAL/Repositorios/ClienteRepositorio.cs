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
    public class ClienteRepositorio : RepositorioGenerico<Cliente> , IClienteRepository
    {
        //inyeccion de dependencia

        private readonly ApplicationDbContext _context;
       
        public ClienteRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //Obtener un cliente por {id} [HttpGet]
        //public override async Task<Cliente?> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        var clienteExiste = await _context.Clientes.FindAsync(id);
        //        if (clienteExiste != null)
        //        {
        //            return clienteExiste;
        //        }
        //        throw new KeyNotFoundException(nameof(clienteExiste));
        //    }
        //    catch
        //    {
        //        throw new KeyNotFoundException($"El cliente con id {id} no se encuentra en la base de datos");
        //    }
        //}

        //public override void Delete(Cliente cliente)
        //{
        //    //Incluye los turnos del cliente.
        //    var entityExist = _context.Clientes
        //        .Include(c => c.Turnos)
        //        .FirstOrDefault(c => c.ClienteId == cliente.ClienteId);
        //    if (entityExist != null)
        //    {
        //        _context.Clientes.Remove(entityExist);
        //    }
        //}


    }
}
