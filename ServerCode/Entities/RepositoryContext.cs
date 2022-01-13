using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasKey(e => e.UserId);
            });

            builder.Entity<Owner>(entity => {
                entity.HasKey(e => e.Id);
            });

            builder.Entity<Account>(entity => {
                entity.HasKey(e => e.Id);
            });

            builder.Entity<Account>()
                .HasOne(o => o.Owner)
                .WithMany(a => a.Accounts)
                .HasForeignKey(oa => oa.OwnerId);
        }
    }
}
