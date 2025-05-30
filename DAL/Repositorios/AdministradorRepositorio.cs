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

        public async Task<Administrador?> ObtenerPorUsuarioCorreoAsync(string usuarioCorreo)
        {
            return await _context.Administradores
                .FirstOrDefaultAsync(a => a.Usuario == usuarioCorreo || a.Correo == usuarioCorreo);
        }

        public async Task<bool> ExisteCorreo(string correo)
        {
            return await _context.Administradores.AnyAsync(a => a.Correo == correo);
                
        }
    }
}
