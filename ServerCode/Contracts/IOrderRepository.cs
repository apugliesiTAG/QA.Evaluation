using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IOrderRepository
    {
        IEnumerable<Order> OrdersLookup(string filter, int skip, int take);
        Order FindOrder(int Id);
        int totalCount();
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
