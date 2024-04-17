using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Common.Application.Services
{
    public interface IComponentHashingService
    {
        string HashComponentCodes(string productCode, List<string> componentCodes);
    }
}
