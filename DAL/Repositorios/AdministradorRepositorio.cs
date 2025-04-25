



using DAL.Data;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositorios;


namespace DAL.Repositorios
{
    public class AdministradorRepositorio : RepositorioGenerico<Administrador>, IAdministradorRepository
    {
       private ApplicationDbContext _context;
        public AdministradorRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
