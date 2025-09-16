using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DomainLayer.Models;
using BLL.Services.OperationResult;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

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
        public async Task<OperationResult<IEnumerable<MetodoPago>>> ObtenerTodos()
        {
            try
            {
                var metodosPagos = await _metodoPagoRepository.GetAllAsync();
                if (!metodosPagos.Any())
                {
                    return OperationResult<IEnumerable<MetodoPago>>.Fail("Aun no hay registros");
                }

                return OperationResult<IEnumerable<MetodoPago>>.Ok(metodosPagos);
            }
            catch (DbException ex)
            {
                return OperationResult<IEnumerable<MetodoPago>>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }

        }

        /// <summary>
        /// OBTIENE UN REGISTRO POR MEDIO DE SU ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns> DICHA ENTIDAD QUE COINCIDA CON EL ID INGRESADO </returns>
        public async Task<OperationResult<MetodoPago>> ObtenerPorId(int id)
        {
            try
            {
                var metodoPago = await _metodoPagoRepository.GetByIdAsync(id);
                if (metodoPago == null)
                {
                    return OperationResult<MetodoPago>.Fail($"No existe registor id{id}");
                }

                return OperationResult<MetodoPago>.Ok(metodoPago);
            }
            catch (DbException ex)
            {
                return OperationResult<MetodoPago>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }

        }

        /// <summary>
        /// CREA UN NUEVO REGISTRO DE TIPO METODOPAGO
        /// </summary>
        /// <param name="metodoPagoACrear"> UN OBJETO DE TIPO MetodoPago </param>
        /// <returns> RETORNARA UN RESULTADO DE TIPO OperationResult<MetodoPago>  </returns>
        public async Task<OperationResult<MetodoPago>> Crear(MetodoPago metodoPagoACrear)
        {
            try
            {
                var validarMetodoPago = ValidarMetodoPago(metodoPagoACrear);

                if (!validarMetodoPago.Success) 
                { 
                    return validarMetodoPago;   //Retorna un tipo OperationResult
                }

                await _metodoPagoRepository.AddAsync(metodoPagoACrear);
                return OperationResult<MetodoPago>.Ok(metodoPagoACrear);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<MetodoPago>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }
        }

        public async Task<OperationResult<MetodoPago>> Actualizar(MetodoPago metodoPagoValidar, int id)
        {
            try
            {
                var validarMetodoPagoActualizar = ValidarMetodoPago(metodoPagoValidar);
                if (!validarMetodoPagoActualizar.Success) //Corregir
                {
                    return validarMetodoPagoActualizar;
                }

                var metodoPagoExiste = await _metodoPagoRepository.GetByIdAsync(id); //Se obtiene por id [A]
                if (metodoPagoExiste == null)
                {
                    return OperationResult<MetodoPago>.Fail("El registro no se encuentra.");
                }

               
                metodoPagoExiste.Descripcion = metodoPagoValidar.Descripcion; //Se actualiza el metodo obtenido por el id.[B]

                //Se Guarda en la base de datos a traves del repositorio
                await _metodoPagoRepository.UpdateAsync(metodoPagoExiste);

                return OperationResult<MetodoPago>.Ok(metodoPagoExiste);//Se retorna el metodo actualizado por el id.[C]
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<MetodoPago>.Fail("Algo salio mal" + ex.InnerException?.Message);
            }
        }

        public async Task<OperationResult<string>> Eliminar(int id)
        {
            try
            {
                var metodoPagoExiste = await _metodoPagoRepository.GetByIdAsync(id);
                if (metodoPagoExiste == null)
                {
                    return OperationResult<string>.Fail("Metodo de pago que quieres eliminar no existe!"); ;
                }
                await _metodoPagoRepository.Delete(metodoPagoExiste);
                return OperationResult<string>.Ok("Eliminado con Exito!");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<string>.Fail("Algo salio mal " + ex.InnerException?.Message);
            }
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
                return OperationResult<MetodoPago>.Fail(errores.ToArray());//CONVIERTE UN TIPO LIST<T> EN UN ARRAY...
            }

            return OperationResult<MetodoPago>.Ok(metodoPagoValidar);
        }
    }
}
