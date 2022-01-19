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
        public Order FindOrder(int Id)
        {
            return FindByCondition(o => o.OrderID == Id )
                .FirstOrDefault();
        }
        public void CreateOrder(Order order)
        {
            order.Id = Guid.NewGuid();
            Create(order);
        }
        public void UpdateOrder(Order order)
        {
            Update(order);
        }
        public void DeleteOrder(Order order)
        {
            Delete(order);
        }
    }
}
