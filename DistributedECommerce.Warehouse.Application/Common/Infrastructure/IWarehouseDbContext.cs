﻿using DistributedECommerce.Warehouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Application.Common.Infrastructure
{
    public interface IWarehouseDbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<DistributedECommerce.Warehouse.Domain.Entities.Component> Components { get; set; }
        public DbSet<DistributedECommerce.Warehouse.Domain.Entities.Product> Products { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }
    }
}
