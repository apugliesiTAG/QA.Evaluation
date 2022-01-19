using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IOrderRepository
    {
        IEnumerable<Order> OrdersLookup();
        Order FindOrder(int Id);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
    }
}
