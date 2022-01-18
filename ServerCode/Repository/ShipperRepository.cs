using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ShipperRepository : RepositoryBase<Shipper>, IShipperRepository
    {
        public ShipperRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<Shipper> ShippersLookup()
        {
            return FindAll()
                .ToList();
        }
        public void CreateShipper(Shipper shipper)
        {
            shipper.Id = Guid.NewGuid();
            Create(shipper);
        }
    }
}
