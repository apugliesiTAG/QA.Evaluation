using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<Customer> CustomersLookup()
        {
            return FindAll()
                .ToList();
        }
        public void CreateCustomer(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            Create(customer);
        }
    }
}
