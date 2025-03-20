﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositorios
{
    class ClienteRepositorio : RepositorioGenerico<Cliente>
    {
        //inyeccion de dependencia

        private readonly ApplicationDbContext _context;

        public ClienteRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


      
    }
}
