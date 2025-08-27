using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using BLL.Services.OperationResult;
using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;

namespace BLL.Services
{
    public class ServicioServicio : IServicioService
    {
        private readonly IServicioRepository _IServicioRepository;

        public ServicioServicio(IServicioRepository servicioRepository)
        {
            _IServicioRepository = servicioRepository;
        }

        //GetAll
        public async Task<OperationResult<IEnumerable<Servicio>>> ObtenerTodos()
        {
            var servicios = await _IServicioRepository.GetAllAsync();
            if (!servicios.Any())
            {
                return OperationResult<IEnumerable<Servicio>>.Fail("Aun no hay servicios Registrados.");
            }

            return OperationResult<IEnumerable<Servicio>>.Ok(servicios);
        }

        //GetById
        public async Task<OperationResult<Servicio>> ObtenerPorId(int id)
        {
            var servicio = await _IServicioRepository.GetByIdAsync(id);
            if (servicio == null)
            {
                return OperationResult<Servicio>.Fail($"No existe registgro con id {id}");
            }
            
            return OperationResult<Servicio>.Ok(servicio);
        }

        //Crear
        public async Task<OperationResult<Servicio>> Crear(Servicio servicio)
        {
            var validarServicio = ValidarServicio(servicio);
            if (!validarServicio.Success)
            {
                return OperationResult<Servicio>.Fail(Convert.ToString(validarServicio.Errors)!.ToString()); ;
            }

            await _IServicioRepository.AddAsync(servicio);

            return OperationResult<Servicio>.Ok(servicio);
        }

        //ACTUALIZAR 
        public async Task<OperationResult<Servicio>> Actualizar(Servicio servicio, int id)
        {
            var validarServicio = ValidarServicio(servicio);
            if (!validarServicio.Success)
            {
                return OperationResult<Servicio>.Fail(String.Join(" ,",Convert.ToString(validarServicio.Errors)));
            }

            //Recuperar el servicio original y compararlo
            var servicioExiste = await _IServicioRepository.GetByIdAsync(id);
            if (servicioExiste == null)
            {
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

        //ELIMINAR
        public async Task<OperationResult<string>> Eliminar(int id)
        {
            var eliminarServicio = await _IServicioRepository.GetByIdAsync(id);
            if (eliminarServicio == null)
            {
                return OperationResult<string>.Fail($"No existe servicio id {id}");
            }

            _IServicioRepository.Delete(eliminarServicio);

            return OperationResult<string>.Ok("Servicio eliminado");

        }


        //Validar Servicio 
        public OperationResult<Servicio> ValidarServicio(Servicio servicio)
        {
            var errors = new List<string>();

            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.Duracion))) errors.Add("Debe ingresar una duracion estimada del servicio.");
            if (!String.IsNullOrWhiteSpace(servicio.Descripcion)) errors.Add("Debe llenar la descripcion del servicio");
            if (!String.IsNullOrWhiteSpace(servicio.Observacion)) errors.Add("Rellene este campo.");
            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.TipoServicioId))) errors.Add("Ingrese un Tipo de Servicio");
            if (!String.IsNullOrWhiteSpace(Convert.ToString(servicio.Precio))) errors.Add("Ingrese un Tipo de Servicio");

            if (errors.Any())
            {
                
                return OperationResult<Servicio>.Fail(errors.ToArray());
            }

            return OperationResult<Servicio>.Ok(servicio);



        }
    }
}
