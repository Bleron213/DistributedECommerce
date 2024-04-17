using BoxCommerce.Warehouse.Application.Common.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Services
{
    public class ComponentHashingService : IComponentHashingService
    {
        public string? HashComponentCodes(string productCode, List<string> componentCodes)
        {
            if (componentCodes.Count == 0)
                return null;

            StringBuilder sb = new();
            sb.Append(productCode);
            foreach (var code in componentCodes)
            {
                sb.Append(code);
            }

            // Convert the string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(sb.ToString());

            // Compute the hash
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(inputBytes);
            }

            // Convert the hash bytes to a hexadecimal string
            sb = sb.Clear();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
