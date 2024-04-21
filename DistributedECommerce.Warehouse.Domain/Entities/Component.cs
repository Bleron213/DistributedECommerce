using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.Domain.Enums;
using DistributedECommerce.Warehouse.Domain.Errors.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Domain.Entities
{
    public class Component : BaseEntity
    {
        public Component(string code, Guid? productId)
        {
            Id = Guid.NewGuid();
            Code = code;
            ProductId = productId;
            Status = ComponentStatus.SCHEDULED;
        }

        public Component(string code )
        {
            Id = Guid.NewGuid();
            Code = code;
            Status = ComponentStatus.SCHEDULED;
        }

        public string Code { get; private set; }
        public string? SerialCode { get; private set; }
        public ComponentStatus Status { get; private set; }
        public Guid? ProductId { get; private set; }
        #region Navigation
        public Product Product { get; set; }
        #endregion

        public void SetProductId(Guid productId)
        {
            ProductId = productId;
        }


        public void ComponentStateChange(ComponentStatus newStatus)
        {
            switch (newStatus)
            {
                case ComponentStatus.IN_PROCESS:
                    {
                        ComponentScheduled();
                        break;
                    }
                case ComponentStatus.READY_TO_ASSEMBLE:
                    {
                        ComponentReady();
                        break;
                    }
                case ComponentStatus.ASSEMBLED:
                    {
                        ComponentAssembled();
                        break;
                    }
                case ComponentStatus.CANCELLED:
                    {
                        ComponentCanceled();
                        break;
                    }
            }
        }

        public void ComponentCanceled()
        {
            switch (Status)
            {
                case ComponentStatus.SCHEDULED:
                    ProductId = null;
                    Status = ComponentStatus.CANCELLED;
                    break;
                case ComponentStatus.IN_PROCESS:
                    ProductId = null;
                    Status = ComponentStatus.CANCELLED;
                    break;
                case ComponentStatus.READY_TO_ASSEMBLE:
                    ProductId = null;
                    break;
            }
        }

        public void ComponentScheduled()
        {
            if(Status != ComponentStatus.IN_PROCESS || Status != ComponentStatus.CANCELLED)
            {
                throw new AppException(WarehouseErrors.InvalidStatusMove(Status.ToString(), ComponentStatus.SCHEDULED.ToString()));
            }

            Status = ComponentStatus.SCHEDULED;
        }

        public void ComponentInProcess()
        {
            if (Status != ComponentStatus.SCHEDULED || Status != ComponentStatus.CANCELLED)
            {
                throw new AppException(WarehouseErrors.InvalidStatusMove(Status.ToString(), ComponentStatus.SCHEDULED.ToString()));
            }

            Status = ComponentStatus.IN_PROCESS;
        }

        public void ComponentReady()
        {
            if(Status != ComponentStatus.IN_PROCESS)
            {
                throw new AppException(WarehouseErrors.InvalidStatusMove(Status.ToString(), ComponentStatus.SCHEDULED.ToString()));
            }

            Status = ComponentStatus.READY_TO_ASSEMBLE;

        }

        public void ComponentAssembled()
        {
            if (Status != ComponentStatus.READY_TO_ASSEMBLE)
            {
                throw new AppException(WarehouseErrors.InvalidStatusMove(Status.ToString(), ComponentStatus.SCHEDULED.ToString()));
            }

            Status = ComponentStatus.ASSEMBLED;

        }

    }
}
