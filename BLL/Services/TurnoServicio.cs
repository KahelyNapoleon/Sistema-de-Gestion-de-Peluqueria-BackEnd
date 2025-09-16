using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositorios;
using DAL.UnitOfWork;
using BLL.Services.OperationResult;
using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;
using DAL.UnitOfWork.Interfaces;
using DomainLayer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BLL.Services
{
    public class TurnoServicio : ITurnoService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly ITurnoService _turnoService;

        public TurnoServicio(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //ObtenerTodos
        public async Task<OperationResult<IEnumerable<Turno>>> ObtenerTodos()
        {
            try
            {
                var turnos = await _unitOfWork.TurnoRepository.GetAllAsync();
                if (!turnos.Any())
                {
                    return OperationResult<IEnumerable<Turno>>.Fail("Aun no hay turnos registrados.");
                }

                return OperationResult<IEnumerable<Turno>>.Ok(turnos);
            }
            catch (DbException ex)
            {
                return OperationResult<IEnumerable<Turno>>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //ObtenerPorId
        public async Task<OperationResult<Turno>> ObtenerPorId(int id)
        {
            try
            {
                var turno = await _unitOfWork.TurnoRepository.GetByIdAsync(id);
                if (turno == null)
                {
                    return OperationResult<Turno>.Fail($"El registro con id {id} no se encuentra.");
                }

                return OperationResult<Turno>.Ok(turno);
            }
            catch (DbException ex)
            {
                return OperationResult<Turno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
        }

        //Crear
        public async Task<OperationResult<Turno>> Crear(Turno turno)
        {
            try
            {
                //1-PRIMERO SE VALIDAN LOS DATOS DEL TURNO
                var validarTurno = ValidarTurno(turno);
                if (!validarTurno.Success)
                {
                    return validarTurno;
                }

                //2-LUEGO SE INICIA UNA TRANSACCION DE LOS DATOS PARA ENVIARLOS ATOMICAMENTE A LA BASE DE DATOS
                await _unitOfWork.IniciarTransaccionAsync();

                turno.EstadoTurnoId = 2; //EstadoTurno = 2 -> A Confirmar;

                //3-AGREGO LOS DATOS DEL NUEVO REGISTRO DEL TURNO Y LOS GUARDO en memoria.
                await _unitOfWork.TurnoRepository.AddAsync(turno);

                await _unitOfWork.GuardarCambiosAsync();

                //4-CREO UN NUEVO OBJETO DE TIPO 'HistorialTurno' PARA CREAR UN NUEVO REGISTRO A PARTIR DEL TURNO CREADO
                var nuevoHistorial = new HistorialTurno
                {
                    EstadoAnterior = 1, //Estadoturno = 1 -> Disponible 
                    EstadoActual = turno.EstadoTurnoId,
                    TurnoId = turno.TurnoId,
                    AdministradorId = turno.AdministradorId,
                    FechaActual = turno.FechaTurno,
                    FechaAnterior = null,
                    HoraActual = turno.HoraTurno,
                    HoraAnterior = null,
                };

                //5-GUARDO LOS CAMBIOS EN MEMORIA, DEL NUEVO REGISTRO CREADO DEL HISTORIAL TURNO
                await _unitOfWork.HistorialTurnoRepository.AddAsync(nuevoHistorial);
                await _unitOfWork.GuardarCambiosAsync();

                //6-CONFIRMO LOS FATOS CON UN CommitAsync(). guardandolos en la Base de datos.
                await _unitOfWork.ConfirmarCambios();

                return OperationResult<Turno>.Ok(turno);


            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.DeshacerCambiosAsync();
                return OperationResult<Turno>.Fail("Ocurrio un error", ex.Message);
            }

        }

        //Actualizar Turno/EstadoTurno
        public async Task<OperationResult<Turno>> ActualizarEstadoTurno(int turnoId, EstadoTurno estadoTurno)
        {
            try
            {
                var turnoExiste = await _unitOfWork.TurnoRepository.GetByIdAsync(turnoId);
                if (turnoExiste == null)
                {
                    return OperationResult<Turno>.Fail($"No se encuentra el registro de id {turnoId}");
                }

                await _unitOfWork.IniciarTransaccionAsync();

                var estadoAnteriorId = turnoExiste.EstadoTurnoId; //REVISAR ESTOS DOS MOVIMIENTOS

                turnoExiste.EstadoTurnoId = estadoTurno.EstadoTurnoId; //ESTE TAMBIEN. EL NUEVO ESTADO DEBE SER SIGUIENTE? O ANTERIOR?

                await _unitOfWork.GuardarCambiosAsync();

                var nuevoHistorialTurno = new HistorialTurno
                {
                    EstadoAnterior = estadoAnteriorId,
                    EstadoActual = turnoExiste.EstadoTurnoId,
                    TurnoId = turnoExiste.TurnoId,
                    AdministradorId = turnoExiste.AdministradorId,
                    FechaActual = turnoExiste.FechaTurno,
                    FechaAnterior = turnoExiste.FechaTurno,
                    HoraActual = turnoExiste.HoraTurno,
                    HoraAnterior = turnoExiste.HoraTurno
                };

                await _unitOfWork.HistorialTurnoRepository.AddAsync(nuevoHistorialTurno);
                await _unitOfWork.GuardarCambiosAsync();

                await _unitOfWork.ConfirmarCambios();

                return OperationResult<Turno>.Ok(turnoExiste);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.DeshacerCambiosAsync();
                return OperationResult<Turno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }

        }

        //Actualizar FechaTurno y HoraTurno
        public async Task<OperationResult<Turno>> ActualizarFechaYHoraTurno(int turnoId, DateOnly nuevaFecha, TimeOnly nuevaHora, EstadoTurno nuevoEstado)
        {
            try
            {
                var turnoExiste = await _unitOfWork.TurnoRepository.GetByIdAsync(turnoId);
                if (turnoExiste == null)
                {
                    return OperationResult<Turno>.Fail($"El turno con id {turnoId} no se encuentra.");
                }

                //VALIDAR FECHA Y HORA!
                var fechaYHora = ValidarFechaYHora(nuevaFecha, nuevaHora);
                if (!fechaYHora.Success)
                {
                    return OperationResult<Turno>.Fail(fechaYHora.Errors!.ToArray());
                }

                //INICIO DE TRANSACCION
                await _unitOfWork.IniciarTransaccionAsync();

                //SE GUARDAN LOS DATOS ANTIGUOS DE LA FECHA, HORA Y ESTADO DEL TURNO 
                var fechaAnterior = turnoExiste.FechaTurno;
                var horaAnterior = turnoExiste.HoraTurno;
                var estadoAnterior = turnoExiste.EstadoTurnoId; //Estado Anterior

                //SE CAMBIAN LOS DATOS DE FECHA, HORA Y ESTADO.
                turnoExiste.EstadoTurnoId = nuevoEstado.EstadoTurnoId;
                turnoExiste.FechaTurno = nuevaFecha;
                turnoExiste.HoraTurno = nuevaHora;

                //SE ACTUALIZAN LOS DATOS DEL TURNO Y SE GUARDAN LOS CAMBIOS
                await _unitOfWork.TurnoRepository.UpdateAsync(turnoExiste);

                //SE GUARDAN LOS CAMBIOS DE LA TRANSACCION
                await _unitOfWork.GuardarCambiosAsync();

                //[ATENCION - PENDIENTE] !!!!!
                //HISTORIAL TURNO.

                //EN ESTE PUNTO SE DEBERA TENER EN CUENTA QUE EL TURNO ESTA 'CONFIRMADO' O 'RESERVADO' PARA REALIZAR EL CAMBIO. [PENDIENTE DE OBSERVACION]
                var nuevoHistorialTurno = new HistorialTurno
                {
                    EstadoAnterior = estadoAnterior,
                    EstadoActual = turnoExiste.EstadoTurnoId,
                    TurnoId = turnoExiste.TurnoId,
                    AdministradorId = turnoExiste.AdministradorId,
                    FechaActual = turnoExiste.FechaTurno,
                    FechaAnterior = turnoExiste.FechaTurno,
                    HoraActual = turnoExiste.HoraTurno,
                    HoraAnterior = turnoExiste.HoraTurno
                };
                //SE CREA UN NUEVO HISTORIAL A PARTIR DE LOS CAMBIOS QUE SE REALIZARON Y SE GUARDAN LOS CAMBIOS EN LA BASE DE DATOS.
                await _unitOfWork.HistorialTurnoRepository.AddAsync(nuevoHistorialTurno);
                await _unitOfWork.GuardarCambiosAsync();

                //AQUI SE CONFIRMAN LOS CAMBIOS QUE SE REALIZARON EN LA BASE DE DATOS Y LOS REALIZA COMO UNA SOLA OPERACION UNICA.
                await _unitOfWork.ConfirmarCambios();


                return OperationResult<Turno>.Ok(turnoExiste);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.DeshacerCambiosAsync();

                return OperationResult<Turno>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }

        }

        //Eliminar-Turno
        public async Task<OperationResult<string>> Eliminar(int turnoId)
        {
            try
            {
                var turnoEliminar = await _unitOfWork.TurnoRepository.GetByIdAsync(turnoId);
                if (turnoEliminar == null)
                {
                    return OperationResult<string>.Fail($"Id {turnoId} no hallado.");
                }

                await _unitOfWork.TurnoRepository.Delete(turnoEliminar);

                return OperationResult<string>.Ok("Turno Eliminado.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var message = ex.InnerException?.Message;
                return OperationResult<string>.Fail("Algo salio mal "+ message);
            }
        }



        //VALIDAR TURNO
        public OperationResult<Turno> ValidarTurno(Turno turno)
        {
            var errors = new List<string>();

            //validar fecha:
            if (turno.FechaTurno < DateOnly.FromDateTime(DateTime.Today)) errors.Add("La fecha debe ser mayor o igual a la fecha actual");
            //validarHora
            if (turno.HoraTurno.Hour < 9 || turno.HoraTurno.Hour > 20) errors.Add("La hora del turno debe encontrarse dentro del horario de apertura (9:00hs-20:00hs).");

            //AQUI SE DEBE CORREGIR LAS VALIDACIONES, VER FLUENTVALIDATION

            //OBSERVACION QUE LOS CAMPOS SEAN MAYORES IGUALES A CERO SOLO ESPECIFICA QUE EL CAMPO DEBE SER COMPLETADO.
            if (turno.ServicioId >= 0) errors.Add("El servicio debe especificarse");

            if (turno.ClienteId >= 0) errors.Add("Debe ingresar un cliente registrado.");

            if (turno.EstadoTurnoId >= 0) errors.Add("Debe ingresar estado del turno");

            if (turno.Servicio.Precio >= 0) errors.Add("Debe ingresar un monto");

            if (turno.MetodoPagoId >= 0) errors.Add("Ingrese el metodo de pago");



            if (errors.Any())
            {
                return OperationResult<Turno>.Fail(errors.ToArray());
            }

            return OperationResult<Turno>.Ok(turno);

        }


        public OperationResult<bool> ValidarFechaYHora(DateOnly fecha, TimeOnly hora)
        {
            var errors = new List<string>();

            if (fecha < DateOnly.FromDateTime(DateTime.Today)) errors.Add("La fecha debe ser mayor o igual a la fecha actual");

            if (hora.Hour < 9 || hora.Hour > 20) errors.Add("La hora del turno debe encontrarse dentro del horario de apertura (9:00hs-20:00hs).");

            if (errors.Any())
            {
                return OperationResult<bool>.Fail(errors.ToArray());
            }

            return OperationResult<bool>.Ok(true);


        }
    }
}
