using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;
using DAL.Repositorios.Interfaces;
using BLL.Services.Interfaces;

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
            var tipoServicios = await _tipoServicioRepository.GetAllAsync();
            if (!tipoServicios.Any())
            {
                return OperationResult<IEnumerable<TipoServicio>>.Fail("No se registran datos de tipo servicio");
            }

            return OperationResult<IEnumerable<TipoServicio>>.Ok(tipoServicios);
        }

        //ObtenerPorId
        public async Task<OperationResult<TipoServicio>> ObtenerPorId(int id)
        {
            var tipoServicio = await _tipoServicioRepository.GetByIdAsync(id);
            if (tipoServicio == null)
            {
                return OperationResult<TipoServicio>.Fail($"No existe registro con id {id}");
            }

            return OperationResult<TipoServicio>.Ok(tipoServicio);
        }

        //Crear
        public async Task<OperationResult<TipoServicio>> Crear(TipoServicio tipoServicio)
        {
            var tipoServicioValidar = ValidarTipoServicio(tipoServicio);
            if (!tipoServicioValidar.Success)
            {
                return OperationResult<TipoServicio>.Fail(tipoServicioValidar.Errors!.ToArray());
            }

            await _tipoServicioRepository.AddAsync(tipoServicio);

            return OperationResult<TipoServicio>.Ok(tipoServicio);
        }

        //Actualizar
        public async Task<OperationResult<TipoServicio>> Actualizar(TipoServicio tipoServicio, int id)
        {
            var tipoServicioValidar = ValidarTipoServicio(tipoServicio);
            if (!tipoServicioValidar.Success)
            {
                return OperationResult<TipoServicio>.Fail(tipoServicioValidar.Errors!.ToArray());
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

        //Eliminar
        public async Task<OperationResult<string>> Eliminar(int id)
        {
            var tipoServicioEliminar = await _tipoServicioRepository.GetByIdAsync(id);
            if (tipoServicioEliminar == null)
            {
                return OperationResult<string>.Fail($"Tipo servicio con id {id} no existe en los registros.");
            }

            _tipoServicioRepository.Delete(tipoServicioEliminar);

            return OperationResult<string>.Ok("Registro Eliminado.");
        }

        //ValidarTipoServicio
        public OperationResult<TipoServicio> ValidarTipoServicio(TipoServicio tipoServicio)
        {
            var errors = new List<string>();

            if (!String.IsNullOrWhiteSpace(tipoServicio.Descripcion)) errors.Add("Complete este campo.");

            if (errors.Any())
            {
                return OperationResult<TipoServicio>.Fail(errors.ToArray());
            }

            return OperationResult<TipoServicio>.Ok(tipoServicio);
        }
    }
}
