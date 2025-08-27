using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using BLL.Services.OperationResult;

namespace BLL.Services
{
    public class MetodoPagoServicio : IMetodoPagoService
    {
        private readonly IMetodoPagoRepository _metodoPagoRepository;
       

        public MetodoPagoServicio(IMetodoPagoRepository metodoPagoRepository) //: base(metodoPagoRepository, validationDictionary) 
        {
            _metodoPagoRepository = metodoPagoRepository;
        }

        /// <summary>
        /// RECUPERA TODOS LOS REGISTROS DETALLADOS DE LA BASE DE DATOS 
        /// </summary>
        /// <returns> LISTA DE REGISTROS DE TIPO METODOPAGO </returns>
        public async Task<IEnumerable<MetodoPago>> ObtenerTodos()
        {
            return await _metodoPagoRepository.GetAllAsync();
        }

        /// <summary>
        /// OBTIENE UN REGISTRO POR MEDIO DE SU ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns> DICHA ENTIDAD QUE COINCIDA CON EL ID INGRESADO </returns>
        public async Task<MetodoPago?> ObtenerPorId(int id)
        {
            return await _metodoPagoRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// CREA UN NUEVO REGISTRO DE TIPO METODOPAGO
        /// </summary>
        /// <param name="metodoPagoACrear"> UN OBJETO DE TIPO MetodoPago </param>
        /// <returns> RETORNARA UN RESULTADO DE TIPO OperationResult<MetodoPago>  </returns>
        public async Task<OperationResult<MetodoPago>> Crear(MetodoPago metodoPagoACrear)
        {
            var validacionMetodoPagoACrear = ValidarMetodoPago(metodoPagoACrear);

            if (!validacionMetodoPagoACrear.Success) //Corregir?  Si el estado de exito del metodo ValidarMetodoPago() es 'False' entonces
                                                       //         ingresa a la condicion y retorna el tipo 
            {
                return validacionMetodoPagoACrear;   //Este tipo No Deberia retornar una lsita de errores?
            }

            await _metodoPagoRepository.AddAsync(metodoPagoACrear);
            return OperationResult<MetodoPago>.Ok(metodoPagoACrear);
        }

        public async Task<OperationResult<MetodoPago>> Actualizar(MetodoPago metodoPagoValidar, int id)
        {
            var validacionMetodoPagoActualizar = ValidarMetodoPago(metodoPagoValidar); 
            if (!validacionMetodoPagoActualizar.Success) //Corregir
            {
                return validacionMetodoPagoActualizar;
            }

            var metodoPagoExiste = await _metodoPagoRepository.BuscarAsync(id);
            if (metodoPagoExiste == null)
            {
                return OperationResult<MetodoPago>.Fail("El registro no se encuentra en la base de datos.");
            }

            //Unico Campo que se debe actualizar de MetodoPago - Solo la descripcion del metodo
            metodoPagoExiste.Descripcion = metodoPagoValidar.Descripcion;

            //Se Guarda en la base de datos a traves del repositorio
            await _metodoPagoRepository.UpdateAsync(metodoPagoExiste);

            return OperationResult<MetodoPago>.Ok(metodoPagoExiste);
        }

        public async Task<OperationResult<string>> Eliminar(int id)
        {
            var metodoPagoExiste = await _metodoPagoRepository.GetByIdAsync(id);
            if (metodoPagoExiste == null)
            {
                return OperationResult<string>.Fail("Metodo que quieres eliminar no existe!"); ;
            }
            _metodoPagoRepository.Delete(metodoPagoExiste);
            return OperationResult<string>.Ok("Eliminado con Exito!");
        }




            //VALIDACION DE UN OBJETO MetodoPago... ... ...





        /// <summary>
        /// VALIDA LOS CAMPOS DE LA ENTIDAD
        /// </summary>
        /// <param name="metodoPagoValidar"></param>
        /// <returns>Retorna un resultado de tipo OperationResult<MetodoPago>, que pueden ser, Ok():Success=True | Fail(Params string[] errors): Success:False y Error=errors | Un tipo MetodoPago</returns>
        public OperationResult<MetodoPago> ValidarMetodoPago(MetodoPago metodoPagoValidar)
        {
            var errores = new List<string>();

            if (string.IsNullOrEmpty(metodoPagoValidar.Descripcion))
                errores.Add("La descripcion es requerida");

            if (errores.Any())
            {
                var resultado = OperationResult<MetodoPago>.Fail(errores.ToArray());//CONVIERTE UN TIPO LIST<T> EN UN ARRAY...
                return resultado;
            }
               
           
            return OperationResult<MetodoPago>.Ok(metodoPagoValidar);
        }

    }
}
