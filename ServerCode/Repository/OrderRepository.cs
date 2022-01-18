using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<Order> OrdersLookup()
        {
            return FindAll()
                .ToList();
        }
        public void CreateOrder(Order order)
        {
            order.Id = Guid.NewGuid();
            Create(order);
        }
    }
}
