using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IValidationDictionary
    {
        bool Success { get; set; }
        List<string> Errors { get; set; }
        //static OperationResult Ok();
        //static OperationResult Fail(params string[] errors);
    }
}
