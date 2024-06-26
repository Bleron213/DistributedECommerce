﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Application.Common.Infrastructure
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public Guid UserGuid { get; }
        public string Username { get; }
    }
}
