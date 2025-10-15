using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using System.Text.RegularExpressions;
using BLL.Services.OperationResult;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{

    //VER COMO DESACOPLAR EL METODO DE VALIDACION DE CLIENTE PARA FUTURAS MIGRACIONES...
    //ENTENDER BIEN EL FLUJO Y REEVEER ALGUNAS ACCIONES DE LOS METODOS, CREO QUE PUEDEN MEJORAR...
    //VER EL TEMA DEL MIDDLEWARE SI ES NECESARIO INTEGRARLO AHORA O LUEGO DE TERMINAR LOS CRUDs DE CADA MODELO.
    public class ClienteServicio : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<ClienteServicio> _logger;

        public ClienteServicio(IClienteRepository clienteRepository, ILogger<ClienteServicio> logger)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
        }

        public async Task<OperationResult<IEnumerable<Cliente>>> ObtenerTodos()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();

                if (!clientes.Any())
                {
                    _logger.LogInformation("Aun no hay Clientes registrados");
                    return OperationResult<IEnumerable<Cliente>>.Fail("Aun no hay clientes registrados!");
                }

                return OperationResult<IEnumerable<Cliente>>.Ok(clientes);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal {message}", ex.InnerException?.Message);
                return OperationResult<IEnumerable<Cliente>>.Fail("Algo salio mal " + ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<OperationResult<Cliente>> ObtenerPorId(int id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);
                if (cliente == null)
                {
                    _logger.LogInformation("El Cliente de id{id} no existe", id);
                    return OperationResult<Cliente>.Fail($"El cliente con id {id} no existe");
                }

                return OperationResult<Cliente>.Ok(cliente);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<Cliente>.Fail("Algo salio mal " + ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<OperationResult<Cliente>> Crear(Cliente cliente)
        {
            try
            {
                var validarCliente = ValidarCliente(cliente);
                if (!validarCliente.Success)
                    return validarCliente;//Los errores de la validacion? 

                await _clienteRepository.AddAsync(cliente);

                return OperationResult<Cliente>.Ok(cliente);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);

                return OperationResult<Cliente>.Fail(ex.InnerException!.Message);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);

                return OperationResult<Cliente>.Fail(ex.Message);
            }

        }


        public async Task<OperationResult<Cliente>> Actualizar(Cliente cliente, int id)
        {
            try
            {
                var clienteExiste = await _clienteRepository.BuscarAsync(id);
                if (clienteExiste == null)
                {
                    _logger.LogWarning("Id:{id} no existe",id);
                    return OperationResult<Cliente>.Fail("El id no corresponde a un cliente existente");
                }

                var clienteValidar = ValidarCliente(cliente);
                if (!clienteValidar.Success)
                {
                    return clienteValidar;
                }

                clienteExiste.Nombre = cliente.Nombre;
                clienteExiste.Apellido = cliente.Apellido;
                clienteExiste.NroCelular = cliente.NroCelular;
                clienteExiste.CorreoElectronico = cliente.CorreoElectronico;
                clienteExiste.FechaNacimiento = cliente.FechaNacimiento;

                await _clienteRepository.UpdateAsync(clienteExiste);

                return OperationResult<Cliente>.Ok(clienteExiste);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<Cliente>.Fail("Hubo un error de excepcion", ex.InnerException!.Message);
            }
        }


        /// <summary>
        /// ELIMINAR UN REGISTRO DE CLIENTE
        /// </summary>
        /// <param name="id">ELIMINA TAL REGISTRO A PATIR DE ESTE PAREMTRO QUE ES EL ID DEL CLIENTE</param>
        /// <returns> UN TIPO OperationResult<bool>.Value que sera TRUE O FALSE </returns>
        public async Task<OperationResult<string>> Eliminar(int id)
        {
            try
            {
                var clienteExiste = await _clienteRepository.GetByIdAsync(id);
                if (clienteExiste == null)
                {
                    return OperationResult<string>.Fail($"Id {id} no existe.");

                }

                await _clienteRepository.Delete(clienteExiste);

                return OperationResult<string>.Ok("Registro eliminado correctamente.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                
                return OperationResult<string>.Fail($"Algo salio mal {ex.InnerException?.Message}");
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);

                return OperationResult<string>.Fail($"Algo salio mal" + ex.InnerException?.Message ?? ex.Message);
            }

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
            else if (cliente.NroCelular.Length < 7 && cliente.NroCelular.Length > 15)
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
            {
                _logger.LogWarning("Error de validacion: {errores}", errores.ToArray());
                return OperationResult<Cliente>.Fail(errores.ToArray());
            }

            //Si todo sale bien.
            return OperationResult<Cliente>.Ok(cliente);
        }

    }
}
