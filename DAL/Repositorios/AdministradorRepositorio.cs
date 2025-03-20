



using DAL.Data;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositorios
{
    class AdministradorRepositorio : RepositorioGenerico<Administrador>
    {
       private ApplicationDbContext _context;
        public AdministradorRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
