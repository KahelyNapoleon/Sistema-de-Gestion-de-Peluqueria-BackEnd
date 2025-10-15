using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using BLL.Services.OperationResult;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class HistorialTurnoServicio : IHistorialTurnoService
    {
        //Leer las especificaciones
        //Se reevio que tipo de implementacion debe devolver este servicio...

        private readonly IHistorialTurnoRepository _historialTurnoRepositorio;
        private readonly ILogger<HistorialTurnoServicio> _logger;

        public HistorialTurnoServicio(IHistorialTurnoRepository historialTurnoRepositorio, ILogger<HistorialTurnoServicio> logger)
        {
            _logger = logger;
            _historialTurnoRepositorio = historialTurnoRepositorio;
        }

        //Obtener Todos los Registros
        /// <summary>
        /// Envia los registros de historialTurnos.
        /// </summary>
        /// <returns>Retorna un tipo OperationResult<IEnumerable<HistorialTurnos>> </returns>
        public async Task<OperationResult<IEnumerable<HistorialTurno>>> ObtenerTodos()
        {
            try
            {
                var historiales = await _historialTurnoRepositorio.GetAllAsync();
                if (!historiales.Any())
                {
                    return OperationResult<IEnumerable<HistorialTurno>>.Fail("Aun no hay Historial de Turno Registrado.");
                }

                return OperationResult<IEnumerable<HistorialTurno>>.Ok(historiales);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);

                return OperationResult<IEnumerable<HistorialTurno>>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //Obtener un solo registro por id.
        public async Task<OperationResult<HistorialTurno>> ObtenerPorId(int id)
        {
            try
            {
                var historialTurno = await _historialTurnoRepositorio.GetByIdAsync(id);
                if (historialTurno == null)
                {
                    return OperationResult<HistorialTurno>.Fail($"No existe registro con id {id}.");
                }

                return OperationResult<HistorialTurno>.Ok(historialTurno);
            }
            catch (DbException ex)
            {
                _logger.LogWarning("Algo salio mal: {message}", ex.InnerException?.Message);

                return OperationResult<HistorialTurno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }

        }




    }
}
