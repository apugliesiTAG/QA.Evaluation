using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> CustomersLookup();
        void CreateCustomer(Customer customer);
    }
}
