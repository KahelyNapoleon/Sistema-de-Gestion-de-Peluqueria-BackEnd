using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;
using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class ServicioServicio : IServicioService
    {
        private readonly IServicioRepository _IServicioRepository;
        private readonly ILogger<ServicioServicio> _logger;

        public ServicioServicio(IServicioRepository servicioRepository, ILogger<ServicioServicio> logger)
        {
            _logger = logger;
            _IServicioRepository = servicioRepository;
        }

        //GetAll
        public async Task<OperationResult<IEnumerable<Servicio>>> ObtenerTodos()
        {
            try
            {
                var servicios = await _IServicioRepository.GetAllAsync();
                if (!servicios.Any())
                {
                    _logger.LogInformation("No hay Servicios registrados.");
                    return OperationResult<IEnumerable<Servicio>>.Fail("Aun no hay servicios Registrados.");
                }

                return OperationResult<IEnumerable<Servicio>>.Ok(servicios);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<IEnumerable<Servicio>>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //GetById
        public async Task<OperationResult<Servicio>> ObtenerPorId(int id)
        {
            try
            {
                var servicio = await _IServicioRepository.GetByIdAsync(id);
                if (servicio == null)
                {
                    _logger.LogInformation("No existe el servicio registrado id:{id}.", id);
                    return OperationResult<Servicio>.Fail($"No existe registgro con id {id}");
                }

                return OperationResult<Servicio>.Ok(servicio);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<Servicio>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }
        }

        //Crear
        public async Task<OperationResult<Servicio>> Crear(Servicio servicio)
        {
            try
            {
                var validarServicio = ValidarServicio(servicio);
                if (!validarServicio.Success)
                {
                    return validarServicio;
                }

                await _IServicioRepository.AddAsync(servicio);

                return OperationResult<Servicio>.Ok(servicio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<Servicio>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //ACTUALIZAR 
        public async Task<OperationResult<Servicio>> Actualizar(Servicio servicio, int id)
        {
            try
            {
                var validarServicio = ValidarServicio(servicio);
                if (!validarServicio.Success)
                {
                    return OperationResult<Servicio>.Fail(String.Join(" ,", Convert.ToString(validarServicio.Errors)));
                }

                //Recuperar el servicio original y compararlo
                var servicioExiste = await _IServicioRepository.GetByIdAsync(id);
                if (servicioExiste == null)
                {
                    _logger.LogWarning("Servicio id:{id} no se encuentra", id);
                    return OperationResult<Servicio>.Fail($"El servicio con id {id} no existe");
                }

                servicioExiste.Duracion = servicio.Duracion;
                servicioExiste.Observacion = servicio.Observacion;
                servicioExiste.Precio = servicio.Precio;
                servicioExiste.Descripcion = servicio.Descripcion;
                servicioExiste.TipoServicioId = servicio.TipoServicioId;

                await _IServicioRepository.UpdateAsync(servicioExiste);

                return OperationResult<Servicio>.Ok(servicioExiste);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<Servicio>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //ELIMINAR
        public async Task<OperationResult<string>> Eliminar(int id)
        {
            try
            {
                var eliminarServicio = await _IServicioRepository.GetByIdAsync(id);
                if (eliminarServicio == null)
                {
                    _logger.LogWarning("Id:{id} no se encuentra.",id);
                    return OperationResult<string>.Fail($"No existe servicio id {id}");
                }

                await _IServicioRepository.Delete(eliminarServicio);

                return OperationResult<string>.Ok("Servicio eliminado");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);
                return OperationResult<string>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }


        //Validar Servicio 
        public OperationResult<Servicio> ValidarServicio(Servicio servicio)
        {
            var errors = new List<string>();

            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.Duracion))) errors.Add("Debe ingresar una duracion estimada del servicio.");
            if (!String.IsNullOrWhiteSpace(servicio.Descripcion)) errors.Add("Debe llenar la descripcion del servicio");
            if (!String.IsNullOrWhiteSpace(servicio.Observacion)) errors.Add("Rellene este campo.");
            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.TipoServicioId))) errors.Add("Ingrese un Tipo de Servicio");
            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.Precio))) errors.Add("Ingrese un Valor");

            if (errors.Any())
            {
                _logger.LogWarning("Datos invalidos: {errors}", errors.ToArray());
                return OperationResult<Servicio>.Fail(errors.ToArray());
            }

            return OperationResult<Servicio>.Ok(servicio);
        }
    }
}
