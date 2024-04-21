using BoxCommerce.Utils.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Utils.Errors.CoreErrors
{
    public class GenericErrors
    {
        public static readonly CustomError IntegrationError = new CustomError(HttpStatusCode.InternalServerError, "something_went_wrong", "Something went wrong");
        public static readonly CustomError ThirdPartyFailure = new CustomError(HttpStatusCode.InternalServerError, "third_party_failure", "Third Party Failure");
        public static readonly CustomError NotFound = new CustomError(HttpStatusCode.NotFound, "not_found", "Not found");

    }
}
