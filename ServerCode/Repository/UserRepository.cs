using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<User> GetAllUsers()
        {
            return FindByCondition(x => x.State == false)
                .OrderBy(u => u.UserId)
                .ToList();
        }
        public User GetUserById(Guid userId) 
        {
            return FindByCondition(u => u.UserId.Equals(userId) && u.State == false)
            .FirstOrDefault();
        }
        public void CreateUser(User user)
        {
            Create(user);
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
