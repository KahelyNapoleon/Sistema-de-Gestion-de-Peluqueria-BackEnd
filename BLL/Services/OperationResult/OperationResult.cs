using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.Services.OperationResult
{
    public class OperationResult<T> 
    {
        //  ARREGLAR LOS ERRORES DE COMPILACION
        /// <summary>
        /// Esta Clase tiene como propiedades de Estado 3 campos encapsulados
        /// 1- Un valor de Verdad 
        /// 2- Una Lista de tipo String que describe Errores
        /// 3- Y una propiedad Generica que almacena un tipo de dato de la entidad que se quiere Validar
        /// </summary>
        public bool Success { get; set; } = true; 
        public List<string>? Errors { get; set; } = new();
        public T? Data { get; private set; } //Data puede ser NULL

        public OperationResult() { }

        public static OperationResult<T> Ok(T data)
        {
            return new OperationResult<T> { Success = true, Data = data };
        }

        public static OperationResult<T> Fail(params string[] errors)//lo que hace params es que puedo aniadir una cantidad variable
                                                                     //de argumentos.
        {
            return new OperationResult<T> { Success = false, Errors = errors.ToList() };
        }
    }
}
