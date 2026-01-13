using AppointmentSystem_Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Utilities.Results.Concrete
{
    public class DataResult<T>:Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message) : base(success, message) // succes ve message'ı tekrar yazmamıza gerek yoktur base'de onları alır zaten
        {
            Data = data;
        }
        // Mesaj döndürmek istemediğimiz durumlarda kullanmak için method overloading
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
