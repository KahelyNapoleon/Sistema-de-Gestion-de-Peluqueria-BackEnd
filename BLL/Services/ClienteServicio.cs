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

    //VER COMO DESACOPLAR EL METODO DE VALIDACION DE CLIENTE PARA FUTURAS MIGRACIONES...
    //ENTENDER BIEN EL FLUJO Y REEVEER ALGUNAS ACCIONES DE LOS METODOS, CREO QUE PUEDEN MEJORAR...
    //VER EL TEMA DEL MIDDLEWARE SI ES NECESARIO INTEGRARLO AHORA O LUEGO DE TERMINAR LOS CRUDs DE CADA MODELO.
    public class ClienteServicio : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteServicio(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<OperationResult<IEnumerable<Cliente>>> ObtenerTodos()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            if ( clientes == null || !clientes.Any() )
            {
                return OperationResult<IEnumerable<Cliente>>.Fail("Aun no clientes registrados!");
            }

            return OperationResult<IEnumerable<Cliente>>.Ok(clientes);
        }

        public async Task<OperationResult<Cliente>> ObtenerPorId(int id)
        {
            var clienteExiste = await _clienteRepository.VerificarSiExiste(id);
            if (!clienteExiste)
            {
                return OperationResult<Cliente>.Fail($"El cliente con id {id} no existe");
            }

            var cliente = await _clienteRepository.GetByIdAsync(id);

            return OperationResult<Cliente>.Ok(cliente!);
        }

        public async Task<OperationResult<Cliente>> Crear(Cliente cliente)
        {
            var validarCliente = ValidarCliente(cliente);
            if (!validarCliente.Success)
                return validarCliente;//Los errores de la validacion? 

            await _clienteRepository.AddAsync(cliente);

            return OperationResult<Cliente>.Ok(cliente);
        }


        public async Task<OperationResult<Cliente>> Actualizar(Cliente cliente, int id)
        {
            var clienteExiste = await _clienteRepository.BuscarAsync(id);
            if (clienteExiste == null)
            {
                return OperationResult<Cliente>.Fail("El id no corresponde con un cliente existente");
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

        public async Task<OperationResult<bool>> Eliminar(int id)
        {
            var clienteExiste = await _clienteRepository.VerificarSiExiste(id);
            if (!clienteExiste)
            {
                return OperationResult<bool>.Fail("Id Incorrecto.");
                
            }

            await _clienteRepository.Delete(id);

            return OperationResult<bool>.Ok(true);
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
