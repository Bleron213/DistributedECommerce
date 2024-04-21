using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Utils.Errors
{
    public class CustomError
    {
        public readonly string ErrorKey;
        public readonly string Message;
        public readonly HttpStatusCode StatusCode;

        public CustomError(HttpStatusCode code, string errorKey, string message)
        {
            StatusCode = code;
            ErrorKey = errorKey;
            Message = message;
        }

        public CustomError(string errorKey, string message) : this(HttpStatusCode.BadRequest, errorKey, message)
        {
        }


    }
}
