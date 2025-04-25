using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositorios
{
    public class MetodoPagoRepositorio : RepositorioGenerico<MetodoPago>, IMetodoPagoRepository
    {
        private readonly ApplicationDbContext _context;

        public MetodoPagoRepositorio(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }      
      
    }
}
