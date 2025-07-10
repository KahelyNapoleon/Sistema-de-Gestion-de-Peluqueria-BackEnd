using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using System.Text.RegularExpressions;

namespace BLL.Services
{
    public class ClienteServicio : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteServicio(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodos()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<OperationResult<Cliente>> ObtenerPorId(int id)
        {
            var clienteExiste = await _clienteRepository.VerificarSiExiste(id);
            if (!clienteExiste)
            {
                return OperationResult<Cliente>.Fail($"EL Cliente con id={id} no Existe");
            }

            var cliente = await _clienteRepository.GetByIdAsync(id);

            return OperationResult<Cliente>.Ok(cliente!);
        }

        public async Task<OperationResult<Cliente>> Crear(Cliente cliente)
        {
            var validarCliente = ValidarCliente(cliente);
            if (!validarCliente.Success)
                return OperationResult<Cliente>.Fail(validarCliente.Errors!.ToArray());

            cliente.
        }




        //Validacion de un Tipo Cliente
        public OperationResult<Cliente> ValidarCliente(Cliente cliente)
        {
            //Lista de errores
            var errores = new List<string>();

            //Nombre
            if (String.IsNullOrWhiteSpace(cliente.Nombre)) errores.Add("El nombre es requerido.");
            //Apellido
            if (String.IsNullOrWhiteSpace(cliente.Apellido)) errores.Add("El apellido es necesario");

            //Celular
            if (string.IsNullOrWhiteSpace(cliente.NroCelular))
                errores.Add("El número de celular es obligatorio.");
            else if (!cliente.NroCelular.All(char.IsDigit))
                errores.Add("El número de celular solo debe contener dígitos.");
            else if (cliente.NroCelular.Length < 7 || cliente.NroCelular.Length > 15)
                errores.Add("El número de celular debe tener entre 7 y 15 dígitos.");

            //Correo Electronico.
            if (String.IsNullOrWhiteSpace(cliente.CorreoElectronico))
                errores.Add("El correo es requerido");
            else if (!Regex.IsMatch(cliente.CorreoElectronico, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add("Formato invalido!");

            //Fecha nacimiento
            if (cliente.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
                errores.Add("La fecha de nacimiento no puede ser una fecha futura.");

            //Si hay al menos un error... 
            if (errores.Any())
                return OperationResult<Cliente>.Fail(errores.ToArray());

            //Si todo sale bien.
            return OperationResult<Cliente>.Ok(cliente);


        }

    }
}
