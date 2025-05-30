using DAL.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DomainLayer.Models;

namespace BLL.Services
{
    public class MetodoPagoServicio : IMetodoPagoService
    {
        private readonly IMetodoPagoRepository _metodoPagoRepository;
       

        public MetodoPagoServicio(IMetodoPagoRepository metodoPagoRepository) //: base(metodoPagoRepository, validationDictionary) 
        {
            _metodoPagoRepository = metodoPagoRepository;
        }

        public async Task<IEnumerable<MetodoPago>> ObtenerTodos()
        {
            return await _metodoPagoRepository.GetAllAsync();
        }

        public async Task<MetodoPago?> ObtenerPorId(int id)
        {
            return await _metodoPagoRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult<MetodoPago>> Crear(MetodoPago metodoPagoACrear)
        {
            var validacionMetodoPagoACrear = ValidarMetodoPago(metodoPagoACrear);

            if (!validacionMetodoPagoACrear.Success) //Corregir
            {
                return validacionMetodoPagoACrear;
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

            metodoPagoExiste.Descripcion = metodoPagoValidar.Descripcion;
            await _metodoPagoRepository.UpdateAsync(metodoPagoExiste);

            return OperationResult<MetodoPago>.Ok(metodoPagoExiste);
        }

        public async Task<bool> Eliminar(int id)
        {
            
            if (!await _metodoPagoRepository.VerificarSiExiste(id))
            {
                return false;
            }
            await _metodoPagoRepository.Delete(id);
            return true;
        }

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
                var resultado = OperationResult<MetodoPago>.Fail(errores.ToArray());
                return resultado;
            }
               
           
            return OperationResult<MetodoPago>.Ok(metodoPagoValidar);
        }

    }
}
