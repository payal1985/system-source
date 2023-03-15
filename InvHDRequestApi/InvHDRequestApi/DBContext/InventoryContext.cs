using InvHDRequestApi.DBModels.InventoryDBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBContext
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryImages> InventoryImages { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
       public DbSet<InventoryFloors> InventoryFloors { get; set; }
        public DbSet<InventoryItemCondition> InventoryItemConditions { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            modelBuilder.Entity<InventoryImages>().ToTable("InventoryImages");


            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<InventoryFloors>().ToTable("InventoryFloors");
            modelBuilder.Entity<InventoryItemCondition>().ToTable("InventoryItemCondition");
            modelBuilder.Entity<InventoryHistory>().ToTable("InventoryHistory");
            modelBuilder.Entity<OrderType>().ToTable("OrderType");


        }
    }
}
