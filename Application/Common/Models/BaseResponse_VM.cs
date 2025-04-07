using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class BaseResponse_VM<T>
    {
        public BaseResponse_VM(T response, bool success, Error? error)
        {
            Response = response;
            Success = success;
            Error = error;
        }
        public T Response { get; set; }
        public bool Success { get; set; }
        public Error? Error { get; set; }

    }

    public class Error
    {
        public int ErrorId { get; set; }
        public string[]? Messages { get; set; }
    }
}
