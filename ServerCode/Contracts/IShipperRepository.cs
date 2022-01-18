using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IShipperRepository
    {
        IEnumerable<Shipper> ShippersLookup();
        void CreateShipper(Shipper shipper);
    }
}
