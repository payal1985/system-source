using LoginInventoryApi.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBContext
{
    public class LoginContext : DbContext
    {

        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {     
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<Clients>().ToTable("clients");
            modelBuilder.Entity<UserTypes>().ToTable("user_type");
            modelBuilder.Entity<UserPermissions>().HasKey(x=>new { x.client_id, x.user_id });
        }
    }
}
