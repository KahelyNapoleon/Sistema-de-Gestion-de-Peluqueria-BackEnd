using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AdministradorServicio : IAdministradorService
    {
        public readonly IAdministradorRepository _administradorRepository;

        public AdministradorServicio(IAdministradorRepository administradorRepository)
        {
            _administradorRepository = administradorRepository;
        }

        public async Task<IEnumerable<Administrador>> ObtenerTodos()
        {
            return await _administradorRepository.GetAllAsync();
        }

        public async Task<Administrador?> ObtenerPorId(int id)
        {
            return await _administradorRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult<Administrador>> Crear(Administrador admin)
        {
            var validarAdmin = ValidarAdministrador(admin);
            if (!validarAdmin.Success)
                 return validarAdmin;
            
            await _administradorRepository.AddAsync(admin);
            return OperationResult<Administrador>.Ok(admin);
        }

        public async Task<OperationResult<Administrador>> Actualizar(Administrador admin, int id)
        {
            var adminValidar = ValidarAdministrador(admin);
            if (!adminValidar.Success)
            {
                return adminValidar;
            }

            var adminExiste = await _administradorRepository.BuscarAsync(id);
            if (adminExiste == null)
            {
                return OperationResult<Administrador>.Fail("El registro no existe en la base de datos.");
            }

            await _administradorRepository.UpdateAsync(adminExiste);
            return OperationResult<Administrador>.Ok(adminExiste);
        }

        //FALTA METODOS 'Eliminar' y 'IniciarSesion'.

        //Validar Administrador
        public OperationResult<Administrador> ValidarAdministrador(Administrador administrador)
        {
            var errores = new List<string>();

            if (!string.IsNullOrWhiteSpace(administrador.Usuario)) errores.Add("El nombre de Usuario es requerido");

            if (!string.IsNullOrWhiteSpace(administrador.Correo)) errores.Add("El correo es requerido");
            else if (!Regex.IsMatch(administrador.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add("El formato del Correo es invalido");

            if (!string.IsNullOrWhiteSpace(administrador.Contrasena)) errores.Add("La contrasenia es requerida");
            else if (administrador.Contrasena.Length < 6) errores.Add("La contrasenia debe tener al menos 6 caracteres");

            if (errores.Any()) return OperationResult<Administrador>.Fail(errores.ToArray());

                return OperationResult<Administrador>.Ok(administrador);
        }
    }
}
