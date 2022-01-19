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
        public IEnumerable<Order> OrdersLookup(string filter)
        {
            if (filter != null && filter.Trim().Length > 0)
            {
                string[] search = filter.Replace("\"", string.Empty).Replace("[", string.Empty)
                                    .Replace("]", string.Empty).Split(",");
                return FindByCondition( o => o.CustomerID.Contains(search[2]) || o.ShipCountry.Contains(search[2])
                        ||  o.OrderDate.Equals(search[2]) || o.Freight.ToString().Contains(search[2])
                        || o.ShipVia.ToString().Contains(search[2]))
                    .ToList();
            }
            else {
                return FindAll()
                .ToList();
            }
        }
        public Order FindOrder(int Id)
        {
            return FindByCondition(o => o.OrderID == Id )
                .FirstOrDefault();
        }
        public void CreateOrder(Order order)
        {
            order.Id = Guid.NewGuid();
            order.OrderID = FindAll().Max(o => o.OrderID) + 1;
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
