using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoHosting.Core.Exceptions
{
    public class InvalidDataException:Exception
    {
        public InvalidDataException()
        {

        }
        public InvalidDataException(string message,Exception innerException = null):base(message,innerException)
        {

        }
    }
}
