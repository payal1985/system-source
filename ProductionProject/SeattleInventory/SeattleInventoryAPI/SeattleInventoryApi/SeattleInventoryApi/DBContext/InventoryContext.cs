using Microsoft.EntityFrameworkCore;
using SeattleInventoryApi.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBContext
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemImages> InventoryItemImages { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<InventoryItemRevisionLog> InventoryItemRevisionLogs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<InventoryBuildings> InventoryBuildings { get; set; }
        public DbSet<InventoryFloors> InventoryFloors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Inventory>().ToTable("Inventory");
            //modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            //modelBuilder.Entity<InventoryItemImages>().ToTable("InventoryItemImages");

            modelBuilder.Entity<Inventory>().HasMany(s => s.InventoryItems).WithOne(s => s.Inventory).HasForeignKey(fk=>fk.inventory_id);
            modelBuilder.Entity<InventoryItem>().HasMany(s => s.InventoryItemImages).WithOne(s => s.InventoryItem).HasForeignKey(fk=>fk.inv_item_id);

            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<InventoryItemRevisionLog>().ToTable("InventoryItemRevisionLog");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Customers>().ToTable("Customers");
            modelBuilder.Entity<Login>().ToTable("Users");
            modelBuilder.Entity<InventoryFloors>().ToTable("InventoryFloors");

        }
    }
}
