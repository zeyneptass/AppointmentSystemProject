using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Utilities.Results.Concrete
{
    public class Result : IResult
    {
        public bool Success { get; }
        public string Message { get; }
        public Result(bool success, string message) : this(success)
        //:this(...), aynı sınıf içindeki başka bir constructor'ı çağırmak için kullanılır.
        {
            Message = message;
        }
        // Method overloading
        public Result(bool success)
        {
            Success = success;
        }
    }
}
