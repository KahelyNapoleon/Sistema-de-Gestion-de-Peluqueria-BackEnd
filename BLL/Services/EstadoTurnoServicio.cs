using BLL.Services.Interfaces;
using BLL.Services.OperationResult;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
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
            var clientes = await _estadoTurnoRepository.GetAllAsync();
            if (clientes == null || !clientes.Any())
            {
                return OperationResult<IEnumerable<EstadoTurno>>.Fail("Aun no hay registros de Estados de Turnos");
            }

            return OperationResult<IEnumerable<EstadoTurno>>.Ok(clientes);
        }

       

        public async Task<OperationResult<EstadoTurno>> ObtenerPorId(int id)
        {
            var estadoturno = await _estadoTurnoRepository.BuscarAsync(id);
            if(estadoturno == null)
            {
                return OperationResult<EstadoTurno>.Fail($"El estado de turno con id {id} no existe!");
            }

            return OperationResult<EstadoTurno>.Ok(estadoturno);
        }

        

        public async Task<OperationResult<EstadoTurno>> Crear(EstadoTurno estadoTurno)
        {
            var estadoTurnoCrear = ValidarEstadoTurno(estadoTurno);
            if (!estadoTurnoCrear.Success)
            {
                return OperationResult<EstadoTurno>.Fail(String.Join(",",estadoTurnoCrear.Errors!));//Aca garantizo que la lista de errores no sera vacia
            }

            //Si la validacion es exitosa.
            await _estadoTurnoRepository.AddAsync(estadoTurno);//Agrega el nuevo registro a la BD

            return OperationResult<EstadoTurno>.Ok(estadoTurno);//Retorno del tipo del metodo

        }

        
        public async Task<OperationResult<EstadoTurno>> Actualizar(EstadoTurno estadoTurno, int id)
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
                return OperationResult<EstadoTurno>.Fail(String.Join(" ,",estadoTurnoValidar.Errors!));//Aca garantizo que la lista de errores no sera vacia
            }

            //Si todo sale bien asignar los nuevos datos al registro de estado turno con id = id.
            estadoTurnoExiste.Descripcion = estadoTurno.Descripcion;

            await _estadoTurnoRepository.UpdateAsync(estadoTurnoExiste);

            return OperationResult<EstadoTurno>.Ok(estadoTurnoExiste);
        }

       
        
        public async Task<OperationResult<bool>> Eliminar(int id)
        {
            var verificarSiExiste = await _estadoTurnoRepository.GetByIdAsync(id);
            if (verificarSiExiste == null)
            {
                return OperationResult<bool>.Fail($"El registro con id {id} no existe.");
            }

            _estadoTurnoRepository.Delete(verificarSiExiste);

            return OperationResult<bool>.Ok(true);
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
