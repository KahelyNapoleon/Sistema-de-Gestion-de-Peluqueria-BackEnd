using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositorios
{
    class MetodoPagoRepositorio : RepositorioGenerico<MetodoPago>
    {
        private readonly ApplicationDbContext _context;

        public MetodoPagoRepositorio(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }      
      
    }
}
