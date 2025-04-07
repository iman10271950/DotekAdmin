using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.BaseEntities
{
    public class BaseResult_VM<T>: IBaseResult
    {
        public T Result { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public BaseResult_VM(T result, int code, string? message = null, bool success = false)
        {
            Result = result;
            Code = code;
            Message = message;
            Success = success;
      
        }

        public BaseResult_VM()
            : this(default(T), 0, "", success: false)
        {
        }
    }
}
