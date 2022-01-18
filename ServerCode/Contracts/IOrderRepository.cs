using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IOrderRepository
    {
        IEnumerable<Order> OrdersLookup();
        void CreateOrder(Order order);    }
}
