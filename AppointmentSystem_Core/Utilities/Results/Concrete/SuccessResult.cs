using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Utilities.Results.Concrete
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message)
        {

        }

        // Paremetresiz methodunda result'a sadece true değerini göndeririz.
        public SuccessResult() : base(true)
        {

        }
    }
}
