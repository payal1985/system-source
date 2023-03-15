using Microsoft.EntityFrameworkCore;
//using InventoryApi.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryApi.DBModels.InventoryDBModels;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InventoryApi.DBContext
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
        //public DbSet<InventoryItemStatusHistory> InventoryItemStatusHistory { get; set; }
        //public DbSet<Client> Clients { get; set; }
        //public DbSet<Customers> Customers { get; set; }
        //public DbSet<Login> Logins { get; set; }
        //public DbSet<InventoryBuildings> InventoryBuildings { get; set; }//Commented due to change buidling from inventory db to ssidb location tables.
        public DbSet<InventoryFloors> InventoryFloors { get; set; }
        public DbSet<ItemTypes> ItemTypes { get; set; }
        public DbSet<SpaceTypes> SpaceTypes { get; set; }
        public DbSet<InventoryOwners> InventoryOwners { get; set; }
        public DbSet<InventoryItemCondition> InventoryItemConditions { get; set; }

        public DbSet<ItemTypeOptions> ItemTypeOptions { get; set; }
        public DbSet<ItemTypeOptionLines> ItemTypeOptionLines { get; set; }
        public DbSet<ItemTypeAttribute> ItemTypeAttribute { get; set; }
        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            modelBuilder.Entity<InventoryImages>().ToTable("InventoryImages");

            //modelBuilder.Entity<Inventory>().HasMany(s => s.InventoryItems).WithOne(s => s.Inventory).HasForeignKey(fk=>fk.InventoryID);
            //modelBuilder.Entity<InventoryItem>().HasMany(s => s.InventoryItemImages).WithOne(s => s.InventoryItem).HasForeignKey(fk=>fk.InventoryItemID);

            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            //modelBuilder.Entity<InventoryItemStatusHistory>().ToTable("InventoryItemStatusHistory");
            //modelBuilder.Entity<Client>().ToTable("Client");
            //modelBuilder.Entity<Customers>().ToTable("Customers");
            //modelBuilder.Entity<Login>().ToTable("Users");
            //modelBuilder.Entity<InventoryBuildings>().ToTable("InventoryBuildings"); //Commented due to change buidling from inventory db to ssidb location tables.
            modelBuilder.Entity<InventoryFloors>().ToTable("InventoryFloors");
            modelBuilder.Entity<ItemTypes>().ToTable("ItemTypes");
            modelBuilder.Entity<SpaceTypes>().ToTable("SpaceTypes");
            modelBuilder.Entity<InventoryOwners>().ToTable("InventoryOwners");
            modelBuilder.Entity<InventoryItemCondition>().ToTable("InventoryItemCondition");
            modelBuilder.Entity<ItemTypeOptions>().ToTable("ItemTypeOptions");
            modelBuilder.Entity<ItemTypeOptionLines>().ToTable("ItemTypeOptionLines");
            modelBuilder.Entity<ItemTypeAttribute>().ToTable("ItemTypeAttribute");
            modelBuilder.Entity<Manufacturers>().ToTable("Manufacturers");
            modelBuilder.Entity<InventoryHistory>().ToTable("InventoryHistory");
            modelBuilder.Entity<OrderType>().ToTable("OrderType");
            modelBuilder.Entity<Status>().ToTable("Status");

            //modelBuilder.Entity<InventoryHistory>()
            //.Property(e => e.BatchTransactionID)
            //.ValueGeneratedOnAdd()
            //.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

        }
    }
}
