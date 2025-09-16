using BLL.Services.Interfaces;
using BLL.Services.OperationResult;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EstadoTurnoServicio : IEstadoTurnoService
    {
        private readonly IEstadoTurnoRepository _estadoTurnoRepository;

        public EstadoTurnoServicio(IEstadoTurnoRepository estadoTurnoRepository)
        {
            _estadoTurnoRepository = estadoTurnoRepository;
        }

        //Obtener Todos
        public async Task<OperationResult<IEnumerable<EstadoTurno>>> ObtenerTodos()
        {
            try
            {
                var clientes = await _estadoTurnoRepository.GetAllAsync();
                if (!clientes.Any())
                {
                    return OperationResult<IEnumerable<EstadoTurno>>.Fail("Aun no hay registros de Estados de Turnos");
                }

                return OperationResult<IEnumerable<EstadoTurno>>.Ok(clientes);
            }
            catch (DbException ex)
            {
                return OperationResult<IEnumerable<EstadoTurno>>.Fail("Algo salio mal " + ex.InnerException!.Message ?? ex.Message);
            }
        }



        public async Task<OperationResult<EstadoTurno>> ObtenerPorId(int id)
        {
            try
            {
                var estadoturno = await _estadoTurnoRepository.BuscarAsync(id);
                if (estadoturno == null)
                {
                    return OperationResult<EstadoTurno>.Fail($"El estado de turno con id {id} no existe!");
                }

                return OperationResult<EstadoTurno>.Ok(estadoturno);
            }
            catch (DbException ex)
            {
                return OperationResult<EstadoTurno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }



        public async Task<OperationResult<EstadoTurno>> Crear(EstadoTurno estadoTurno)
        {
            try
            {
                var estadoTurnoCrear = ValidarEstadoTurno(estadoTurno);
                if (!estadoTurnoCrear.Success)
                {
                    return estadoTurnoCrear; // envia un operationResult<EstadoTurno>
                }

                //Si la validacion es exitosa.
                await _estadoTurnoRepository.AddAsync(estadoTurno);//Agrega el nuevo registro a la BD

                return OperationResult<EstadoTurno>.Ok(estadoTurno);//Retorno del tipo del metodo
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<EstadoTurno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }


        public async Task<OperationResult<EstadoTurno>> Actualizar(EstadoTurno estadoTurno, int id)
        {
            try
            {
                var estadoTurnoExiste = await _estadoTurnoRepository.GetByIdAsync(id);
                if (estadoTurnoExiste == null)
                {
                    return OperationResult<EstadoTurno>.Fail($"El Estado de Turno con id {id} no existe!");
                }

                //En caso de que si exista hay que validar los datos que se quieren cambiar.
                var estadoTurnoValidar = ValidarEstadoTurno(estadoTurno);
                if (!estadoTurnoValidar.Success)
                {
                    return estadoTurnoValidar;
                }

                //Si todo sale bien asignar los nuevos datos al registro de estado turno con id = id.
                estadoTurnoExiste.Descripcion = estadoTurno.Descripcion;

                await _estadoTurnoRepository.UpdateAsync(estadoTurnoExiste);

                return OperationResult<EstadoTurno>.Ok(estadoTurnoExiste);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<EstadoTurno>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }
        }



        public async Task<OperationResult<string>> Eliminar(int id)
        {
            try
            {
                var verificarSiExiste = await _estadoTurnoRepository.GetByIdAsync(id);
                if (verificarSiExiste == null)
                {
                    return OperationResult<string>.Fail($"El registro con id {id} no existe.");
                }

                await _estadoTurnoRepository.Delete(verificarSiExiste);

                return OperationResult<string>.Ok("Eliminado con correctamente");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<string>.Fail("Algo salio mal "+ ex.InnerException?.Message);
            }
        }


        //Validacion
        public OperationResult<EstadoTurno> ValidarEstadoTurno(EstadoTurno estadoTurno)
        {
            var errors = new List<string>();

            if (String.IsNullOrEmpty(estadoTurno.Descripcion)) errors.Add("Este campo es obligatorio");

            if (!errors.Any()) return OperationResult<EstadoTurno>.Fail(errors.ToArray());

            return OperationResult<EstadoTurno>.Ok(estadoTurno);
        }

    }
}
