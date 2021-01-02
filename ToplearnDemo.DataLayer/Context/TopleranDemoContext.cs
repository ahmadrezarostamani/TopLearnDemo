using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToplearnDemo.DataLayer.Models.User;
using ToplearnDemo.DataLayer.Models.Transaction;

namespace ToplearnDemo.DataLayer.Context
{
    public class TopleranDemoContext:DbContext
    {
        public TopleranDemoContext(DbContextOptions<TopleranDemoContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
