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
        private IValidationDictionary _validationDictionary;

        public MetodoPagoServicio(IMetodoPagoRepository metodoPagoRepository, IValidationDictionary validationDictionary) //: base(metodoPagoRepository, validationDictionary) 
        {
            _metodoPagoRepository = metodoPagoRepository;
            _validationDictionary = validationDictionary;
        }

        public async Task<IEnumerable<MetodoPago>> ObtenerTodos()
        {
            return await _metodoPagoRepository.GetAllAsync();
        }

        public async Task<MetodoPago?> ObtenerPorId(int id)
        {
            return await _metodoPagoRepository.GetByIdAsync(id);
        }

        public async Task<bool> Crear(MetodoPago metodoPagoACrear)
        {
            if (!ValidarMetodoPago(metodoPagoACrear))
            {
                return false;
            }

            await _metodoPagoRepository.AddAsync(metodoPagoACrear);
            return true;
        }

        public async Task<bool> Actualizar(MetodoPago metodoPagoActualizar)
        {
            if (!ValidarMetodoPago(metodoPagoActualizar))
            {
                return false;
            }

            await _metodoPagoRepository.UpdateAsync(metodoPagoActualizar);
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            if (await _metodoPagoRepository.VerificarSiExiste(id))
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
        /// <returns>VERDADERO O FALOS EN CASO DE QUE SE HAYA CAPTURADO UN ERROR EN LA VALIDACION DE UN SOLO CAMPO</returns>
        public bool ValidarMetodoPago(MetodoPago metodoPagoValidar)
        {
            if (metodoPagoValidar.Descripcion.Trim().Length == 0) _validationDictionary.AddError("Descripcion", "La descripcion es requerida");

            return _validationDictionary.IsValid;
        }

    }
}
