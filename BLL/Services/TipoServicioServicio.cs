using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;
using DAL.Repositorios.Interfaces;
using BLL.Services.Interfaces;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
 

namespace BLL.Services
{
    public class TipoServicioServicio : ITipoServicioService
    {
        private readonly ITipoServicioRepository _tipoServicioRepository;

        public TipoServicioServicio(ITipoServicioRepository tipoServicioRepository)
        {
            _tipoServicioRepository = tipoServicioRepository;
        }

        //ObtenerTodo
        public async Task<OperationResult<IEnumerable<TipoServicio>>> ObtenerTodos()
        {
            try
            {
                var tipoServicios = await _tipoServicioRepository.GetAllAsync();
                if (!tipoServicios.Any())
                {
                    return OperationResult<IEnumerable<TipoServicio>>.Fail("No se registran datos de tipo servicio");
                }

                return OperationResult<IEnumerable<TipoServicio>>.Ok(tipoServicios);
            }
            catch (DbException ex)
            {
                return OperationResult<IEnumerable<TipoServicio>>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //ObtenerPorId
        public async Task<OperationResult<TipoServicio>> ObtenerPorId(int id)
        {
            try
            {
                var tipoServicio = await _tipoServicioRepository.BuscarAsync(id);
                if (tipoServicio == null)
                {
                    return OperationResult<TipoServicio>.Fail($"No existe registro con id {id}");
                }

                var tipoServicioView = new TipoServicio
                {
                    TipoServicioId = tipoServicio.TipoServicioId,
                    Descripcion = tipoServicio.Descripcion,
                    Servicios = tipoServicio.Servicios.Select(s => new Servicio
                    {
                        ServicioId = s.ServicioId,
                        Descripcion = s.Descripcion,
                        Precio = s.Precio,
                        Duracion = s.Duracion,
                        Observacion = s.Observacion
                    }).ToList()
                };

                return OperationResult<TipoServicio>.Ok(tipoServicioView);
            }
            catch (DbException ex)
            {
                return OperationResult<TipoServicio>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }
        }

        //Crear
        public async Task<OperationResult<TipoServicio>> Crear(TipoServicio tipoServicio)
        {
            try
            {
                var tipoServicioValidar = ValidarTipoServicio(tipoServicio);
                if (!tipoServicioValidar.Success)
                {
                    return tipoServicioValidar;
                }

                await _tipoServicioRepository.AddAsync(tipoServicio);

                return OperationResult<TipoServicio>.Ok(tipoServicio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<TipoServicio>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
            catch (DbException ex)
            {
                return OperationResult<TipoServicio>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //Actualizar
        public async Task<OperationResult<TipoServicio>> Actualizar(TipoServicio tipoServicio, int id)
        {
            try
            {
                var tipoServicioValidar = ValidarTipoServicio(tipoServicio);
                if (!tipoServicioValidar.Success)
                {
                    return tipoServicioValidar;
                }

                var tipoServicioExiste = await _tipoServicioRepository.GetByIdAsync(id);
                if (tipoServicioExiste == null)
                {
                    return OperationResult<TipoServicio>.Fail($"El servicio con id {id} no existe en los registros.");
                }

                //DESCRIPCION
                tipoServicioExiste.Descripcion = tipoServicio.Descripcion;

                await _tipoServicioRepository.UpdateAsync(tipoServicioExiste);

                return OperationResult<TipoServicio>.Ok(tipoServicio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<TipoServicio>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //Eliminar
        public async Task<OperationResult<string>> Eliminar(int id)
        {
            try
            {
                var tipoServicioEliminar = await _tipoServicioRepository.GetByIdAsync(id);
                if (tipoServicioEliminar == null)
                {
                    return OperationResult<string>.Fail($"Tipo servicio con id {id} no existe en los registros.");
                }

                await _tipoServicioRepository.Delete(tipoServicioEliminar);

                return OperationResult<string>.Ok("Registro Eliminado.");
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return OperationResult<string>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //ValidarTipoServicio
        public OperationResult<TipoServicio> ValidarTipoServicio(TipoServicio tipoServicio)
        {
            var errors = new List<string>();

            if (String.IsNullOrWhiteSpace(tipoServicio.Descripcion)) errors.Add("Complete este campo.");

            if (errors.Any())
            {
                return OperationResult<TipoServicio>.Fail(errors.ToArray());
            }

            return OperationResult<TipoServicio>.Ok(tipoServicio);
        }
    }
}
