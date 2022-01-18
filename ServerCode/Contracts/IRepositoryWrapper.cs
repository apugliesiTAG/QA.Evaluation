using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public  interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IOwnerRepository Owner { get; }
        IAccountRepository Account { get; }
        IShipperRepository Shipper { get; }
        ICustomerRepository Customer { get; }
        IOrderRepository Order { get; }
        void Save();
    }
}
