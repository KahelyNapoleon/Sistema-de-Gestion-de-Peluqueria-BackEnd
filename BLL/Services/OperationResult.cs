using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.Services
{
    public class OperationResult<T> //:IValidationDictionary
    {
        //  ARREGLAR LOS ERRORES DE COMPILACION
        public bool Success { get; set; } = true;
        public List<string> Errors { get; set; } = new();
        public T? Data { get; private set; }

        public OperationResult() { }

        public static OperationResult<T> Ok(T data)
        {
            return new OperationResult<T> { Success = true, Data = data };
        }

        public static OperationResult<T> Fail(params string[] errors)
        {
            return new OperationResult<T> { Success = false, Errors = errors.ToList() };
        }
    }
}
