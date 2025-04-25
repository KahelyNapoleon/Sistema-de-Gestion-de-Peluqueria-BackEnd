using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using DAL.Repositorios.Interfaces;

namespace DAL.Repositorios
{
    public class ServicioRepositorio : RepositorioGenerico<Servicio> ,IServicioRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicioRepositorio(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

      //Este repositorio tiene una interfaz propia para futuras implementaciones.

        
    }
}
